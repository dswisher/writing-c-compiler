using System;
using System.Collections.Generic;

namespace SwishCC.Lexing
{
    public class LexerResult
    {
        private readonly LinkedList<LexerToken> tokens = new();

        public LexerToken CurrentToken => tokens.First?.Value;


        public void AppendToken(LexerToken token)
        {
            tokens.AddLast(token);
        }


        public LexerToken PopToken()
        {
            var token = tokens.First;

            if (token == null)
            {
                throw new Exception("Attempt to pop token from empty token list.");
            }

            tokens.RemoveFirst();

            return token.Value;
        }
    }
}
