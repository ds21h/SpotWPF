﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using NntpBase.Exceptions;
using Microsoft.Extensions.Logging;
using NntpBase.Nntp.Parsers;
using NntpBase.Nntp.Responses;
using NntpBase.Util;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using NewsWPF.NntpBase.Exceptions;

namespace NntpBase.Nntp
{
    /// <summary>
    /// A standard implementation of an NNTP connection.
    /// Based on Kristian Hellang's NntpLib.Net project https://github.com/khellang/NntpLib.Net.
    /// </summary>
    /// <remarks>This implementation of the <see cref="INntpConnection"/> interface does support SSL encryption but
    /// does not support compressed multi-line results.</remarks>
    public class NntpConnection : INntpConnection
    {
        private readonly ILogger log = Logger.Create<NntpConnection>();
        private readonly TcpClient client = new TcpClient();
        // JB: Removed autoflush from writer. Not working anymore (win 11 / .net 6.0).
        private StreamWriter writer;
        private NntpStreamReader reader;

        /// <inheritdoc/>
        public CountingStream Stream { get; private set; }

        /// <inheritdoc/>
        // JB: Added sync version to use on seperate thread
        public TResponse Connect<TResponse>(string hostname, int port, bool useSsl, IResponseParser<TResponse> parser)
        {
            log.LogInformation("Connecting: {hostname} {port} (Use SSl = {useSsl})", hostname, port, useSsl);
            client.Connect(hostname, port);
            Stream = GetStream(hostname, useSsl);
//            log.LogInformation("Stream aquired");
            writer = new StreamWriter(Stream, UsenetEncoding.Default);
            reader = new NntpStreamReader(Stream, UsenetEncoding.Default);
            return GetResponse(parser);
        }

        public async Task<TResponse> ConnectAsync<TResponse>(string hostname, int port, bool useSsl, IResponseParser<TResponse> parser) {
            log.LogInformation("Connecting: {hostname} {port} (Use SSl = {useSsl})", hostname, port, useSsl);
            await client.ConnectAsync(hostname, port).ConfigureAwait(false);
            //            log.LogInformation("Connected: {hostname} {port} (Use SSl = {useSsl})", hostname, port, useSsl);
            Stream = await GetStreamAsync(hostname, useSsl).ConfigureAwait(false);
            //            log.LogInformation("Stream aquired");
            writer = new StreamWriter(Stream, UsenetEncoding.Default);
            reader = new NntpStreamReader(Stream, UsenetEncoding.Default);
            return GetResponse(parser);
        }

        /// <inheritdoc/>
        public TResponse Command<TResponse>(string command, IResponseParser<TResponse> parser)
        {
            ThrowIfNotConnected();
            log.LogInformation("Sending command: {Command}",command.StartsWith("AUTHINFO PASS", StringComparison.Ordinal) ? "AUTHINFO PASS [omitted]" : command);
            writer.WriteLine(command);
            // JB: Added because autoflush not working anymore (win 11 / .net 6.0).
            writer.Flush();
            return GetResponse(parser);
        }

        /// <inheritdoc/>
        public TResponse MultiLineCommand<TResponse>(string command, IMultiLineResponseParser<TResponse> parser) //, bool decompress = false)
        {
            //            IEnumerable<string> dataBlock;
            List<string> dataBlock;
            string line;
                
            NntpResponse response = Command(command, new ResponseParser());

            dataBlock = new List<string>();
            if (parser.IsSuccessResponse(response.Code)) {
                while ((line = reader.ReadLine()) != null) {
                    dataBlock.Add(line);
                }
//                dataBlock = ReadMultiLineDataBlock();
            }
            //else {
            //    dataBlock = new string[0];
            //}
            //IEnumerable<string> dataBlock = parser.IsSuccessResponse(response.Code)
            //    ? ReadMultiLineDataBlock()
            //    : new string[0];

            return parser.Parse(response.Code, response.Message, dataBlock);
        }
        public TResponse MultiLineRawCommand<TResponse>(string command, IMultiLineProcess pRespProcessor, IResponseParser<TResponse> parser) //, bool decompress = false)
        {
            NntpResponse response = Command(command, new ResponseParser());
            bool lCanceled;

            if (parser.IsSuccessResponse(response.Code)) {
                string line;

                lCanceled = false;
                pRespProcessor.xStartProcess();
                while ((line = reader.ReadLine()) != null) {
                    if (!pRespProcessor.xProcessLine(line)) {
                        lCanceled = true;
                        break;
                    }
                }
                pRespProcessor.xEndProcess();
                if (lCanceled) {
                    throw new MultiLineCanceledException("MultiLine read canceled");
                }
            }

            return parser.Parse(response.Code, response.Message);
        }

        /// <inheritdoc/>
        public TResponse GetResponse<TResponse>(IResponseParser<TResponse> parser)
        {
            string responseText = reader.ReadLine();
            log.LogInformation("Response received: {Response}", responseText);

            if (responseText == null)
            {
                throw new NntpException("Received no response.");
            }
            if (responseText.Length < 3 || !int.TryParse(responseText.Substring(0, 3), out int code))
            {
                throw new NntpException("Received invalid response.");
            }
            return parser.Parse(code, responseText.Substring(3).Trim());
        }

        /// <inheritdoc/>
        public void WriteLine(string line)
        {
            ThrowIfNotConnected();
            writer.WriteLine(line);
            // JB: Added because autoflush not working anymore (win 11 / .net 6.0).
            writer.Flush();
        }

        private void ThrowIfNotConnected()
        {
            if (!client.Connected)
            {
                throw new NntpException("Client not connected.");
            }
        }

        // JB: Added sync version to use on seperate thread
        private CountingStream GetStream(string hostname, bool useSsl)
        {
            log.LogInformation("Get Stream start");
            NetworkStream stream = client.GetStream();
            log.LogInformation("Get Stream aquired");
            if (!useSsl)
            {
                return new CountingStream(stream);
            }
            log.LogInformation("Get Stream Ssl");
            var sslStream = new SslStream(stream);
            log.LogInformation("Get Stream Ssl aquired");
            // JB: Added certificationdata and protocols. Default values no longer sufficient (win 11 / .net 6.0).
            sslStream.AuthenticateAsClient(hostname, new X509Certificate2Collection(), SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, false);
            log.LogInformation("Get Stream Ssl Authenticated");
            return new CountingStream(sslStream);
        }

        private async Task<CountingStream> GetStreamAsync(string hostname, bool useSsl) {
            log.LogInformation("Get Stream start");
            NetworkStream stream = client.GetStream();
            log.LogInformation("Get Stream aquired");
            if (!useSsl) {
                return new CountingStream(stream);
            }
            log.LogInformation("Get Stream Ssl");
            var sslStream = new SslStream(stream);
            log.LogInformation("Get Stream Ssl aquired");
            // JB: Added certificationdata and protocols. Default values no longer sufficient (win 11 / .net 6.0).
            await sslStream.AuthenticateAsClientAsync(hostname, new X509Certificate2Collection(), SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, false);
            log.LogInformation("Get Stream Ssl Authenticated");
            return new CountingStream(sslStream);
        }

        //private IEnumerable<string> ReadMultiLineDataBlock()
        //{
        //    string line;
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        yield return line;
        //    }
        //}

        /// <inheritdoc/>
        public void Dispose()
        {
            client?.Dispose();
            writer?.Dispose();
            reader?.Dispose();
        }
    }
}
