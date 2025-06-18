// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using SwishCC.Lexing;

namespace SwishCC.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException(LexerToken token, string message)
            : base($"Parse error at {token.Line}:{token.Column} - {message} (Token: {token.TokenType} '{token.Value}')")
        {
        }


        public ParseException(string message)
            : base(message)
        {
        }
    }
}
