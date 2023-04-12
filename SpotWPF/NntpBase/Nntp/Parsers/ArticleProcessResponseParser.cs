using System;
using System.Collections.Generic;
using System.Linq;
using NntpBase.Nntp.Responses;

namespace NntpBase.Nntp.Parsers {
    internal class ArticleProcessResponseParser : IResponseParser<NntpResponse> {
        private readonly int successCode;

        public ArticleProcessResponseParser(ArticleRequestType pRequestType) {
            switch (pRequestType) {
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
                    throw new ArgumentOutOfRangeException(nameof(pRequestType), pRequestType, null);
            }
        }

        public bool IsSuccessResponse(int code) => code == successCode;

        public NntpResponse Parse(int code, string message) {
            return new NntpResponse(code, message, IsSuccessResponse(code));
        }
    }
}
