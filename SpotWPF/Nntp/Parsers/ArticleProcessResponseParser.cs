using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers {
    internal class ArticleProcessResponseParser : IResponseParser<NntpResponse> {
        private readonly ILogger log = Logger.Create<ArticleProcessResponseParser>();
        private readonly ArticleRequestType requestType;
        private readonly int successCode;

        public ArticleProcessResponseParser(ArticleRequestType requestType) {
            switch (this.requestType = requestType) {
                case ArticleRequestType.Head:
                    successCode = 221;
                    break;

                case ArticleRequestType.Body:
                    successCode = 222;
                    break;

                case ArticleRequestType.Article:
                    successCode = 220;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
            }
        }

        public bool IsSuccessResponse(int code) => code == successCode;

        public NntpResponse Parse(int code, string message) {
            return new NntpResponse(code, message, IsSuccessResponse(code));
        }
    }
}
