using NntpBase.Nntp.Parsers;
using NntpBase.Nntp.Responses;
using NntpBase.Util;
using Microsoft.Extensions.Logging;

namespace NntpBase.Nntp
{
    /// <summary>
    /// Based on Kristian Hellang's NntpLib.Net project https://github.com/khellang/NntpLib.Net.
    /// </summary>
    public partial class NntpClient
    {
        private readonly ILogger log = Logger.Create<NntpClient>();

        /// <summary>
        /// The <a href="https://tools.ietf.org/html/rfc4643#section-2.3">AUTHINFO USER and AUTHINFO PASS</a> 
        /// (<a href="https://tools.ietf.org/html/rfc2980#section-3.1.1">ad 1</a>)
        /// commands are used to present clear text credentials to the server.
        /// </summary>
        /// <param name="username">The username to use.</param>
        /// <param name="password">The password to use.</param>
        /// <returns>true if the user was authenticated successfully; otherwise false.</returns>
        public bool Authenticate(string username, string password = null)
        {
            log.LogInformation("Authenticate user start");
            Guard.ThrowIfNullOrWhiteSpace(username, nameof(username));
            log.LogInformation("Authenticate user {user}, password {password}", username, password);
            NntpResponse userResponse = connection.Command($"AUTHINFO USER {username}", new ResponseParser(281));
            if (userResponse.Success)
            {
                log.LogInformation("Authenticate user response success");
                return true;
            }
            if (userResponse.Code != 381 || string.IsNullOrWhiteSpace(password))
            {
                log.LogInformation("Authenticate user response error");
                return false;
            }
            log.LogInformation("Authenticate password");
            NntpResponse passResponse = connection.Command($"AUTHINFO PASS {password}", new ResponseParser(281));
            log.LogInformation("Authenticate password response");
            return passResponse.Success;
        }
    }
}
