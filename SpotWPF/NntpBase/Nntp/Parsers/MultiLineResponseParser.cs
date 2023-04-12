using System;
using System.Collections.Generic;
using System.Linq;
using NntpBase.Nntp.Responses;

namespace NntpBase.Nntp.Parsers
{
    internal class MultiLineResponseParser : IMultiLineResponseParser<NntpMultiLineResponse>
    {
        private readonly int[] successCodes;

        public MultiLineResponseParser(params int[] successCodes)
        {
            this.successCodes = successCodes ?? new int[0];
        }

        public bool IsSuccessResponse(int code) {
            if (successCodes.Contains(code)) {
                return true;
            } else {
                return false;
            }
        }
//        public bool IsSuccessResponse(int code) => successCodes.Contains(code);

        public NntpMultiLineResponse Parse(int code, string message, IEnumerable<string> dataBlock) => 
            new NntpMultiLineResponse(code, message, IsSuccessResponse(code), dataBlock);
    }
}
