﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NntpBase.Nntp.Parsers;
using NntpBase.Util;

namespace NntpBase.Nntp
{
    /// <summary>
    /// Represents an NNTP connection.
    /// Based on Kristian Hellang's NntpLib.Net project https://github.com/khellang/NntpLib.Net.
    /// </summary>
    public interface INntpConnection : IDisposable
    {
        /// <summary>
        /// Attempts to establish a connection with a usenet server.
        /// </summary>
        /// <typeparam name="TResponse">The type of the parsed response.</typeparam>
        /// <param name="hostname">The hostname of the usenet server.</param>
        /// <param name="port">The port to use.</param>
        /// <param name="useSsl">A value to indicate whether or not to use SSL encryption.</param>
        /// <param name="parser">The response parser to use.</param>
        /// <returns>A response object of type <typeparamref name="TResponse"/>.</returns>
        // JB: Added sync version to use on seperate thread
        TResponse Connect<TResponse>(string hostname, int port, bool useSsl, IResponseParser<TResponse> parser);

        /// <summary>
        /// Attempts to establish a connection with a usenet server.
        /// </summary>
        /// <typeparam name="TResponse">The type of the parsed response.</typeparam>
        /// <param name="hostname">The hostname of the usenet server.</param>
        /// <param name="port">The port to use.</param>
        /// <param name="useSsl">A value to indicate whether or not to use SSL encryption.</param>
        /// <param name="parser">The response parser to use.</param>
        /// <returns>A response object of type <typeparamref name="TResponse"/>.</returns>
        Task<TResponse> ConnectAsync<TResponse>(string hostname, int port, bool useSsl, IResponseParser<TResponse> parser);

        /// <summary>
        /// Sends a command to the usenet server. The response is expected to be a single line.
        /// </summary>
        /// <typeparam name="TResponse">The type of the parsed response.</typeparam>
        /// <param name="command">The command to send to the server.</param>
        /// <param name="parser">The response parser to use.</param>
        /// <returns>A response object of type <typeparamref name="TResponse"/>.</returns>
        TResponse Command<TResponse>(string command, IResponseParser<TResponse> parser);

        /// <summary>
        /// Sends a command to the usenet server. The response is expected to be multiple lines.
        /// </summary>
        /// <typeparam name="TResponse">The type of the parsed response.</typeparam>
        /// <param name="command">The command to send to the server.</param>
        /// <param name="parser">The multi-line response parser to use.</param>
        /// <returns>A response object of type <typeparamref name="TResponse"/>.</returns>
        TResponse MultiLineCommand<TResponse>(string command, IMultiLineResponseParser<TResponse> parser);
        TResponse MultiLineRawCommand<TResponse>(string command, IMultiLineProcess pRespProcessor, IResponseParser<TResponse> parser);

        /// <summary>
        /// Gets a single-line response from the usenet server.
        /// </summary>
        /// <typeparam name="TResponse">The type of the parsed response.</typeparam>
        /// <param name="parser">The multi-line response parser to use.</param>
        /// <returns>A response object of type <typeparamref name="TResponse"/>.</returns>
        TResponse GetResponse<TResponse>(IResponseParser<TResponse> parser);

        /// <summary>
        /// Sends a line to the usenet server.
        /// </summary>
        /// <param name="line">The line to send to the usenet server.</param>
        void WriteLine(string line);

        /// <summary>
        /// The stream used by the connection.
        /// </summary>
        CountingStream Stream { get; }
    }
}