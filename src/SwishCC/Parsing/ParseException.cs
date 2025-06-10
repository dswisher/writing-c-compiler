using System;
using SwishCC.Lexing;

namespace SwishCC.Parsing
{
    public class ParseException : Exception
    {
        public ParseException(LexerToken token, string message)
            : base($"Parse error at {token.Line}:{token.Column} - {message} (Token: {token.TokenType} '{token.Value}')")
        {
        }
    }
}
