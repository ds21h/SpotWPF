﻿using System;
using System.Collections.Generic;
using System.Linq;
using NntpBase.Nntp.Responses;

namespace NntpBase.Nntp.Parsers
{
    internal class ResponseParser : IResponseParser<NntpResponse>
    {
        private readonly int[] successCodes;

        public ResponseParser(params int[] successCodes)
        {
            this.successCodes = successCodes ?? new int[0];
        }

        public bool IsSuccessResponse(int code) => successCodes.Contains(code);

        public NntpResponse Parse(int code, string message) => 
            new NntpResponse(code, message, IsSuccessResponse(code));
    }
}
