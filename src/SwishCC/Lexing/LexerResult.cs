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


        public void PopToken()
        {
            tokens.RemoveFirst();
        }
    }
}
