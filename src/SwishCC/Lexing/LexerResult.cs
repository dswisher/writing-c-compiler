using System.Collections.Generic;

namespace SwishCC.Lexing
{
    public class LexerResult
    {
        private readonly LinkedList<LexerToken> tokens = new();

        public LexerToken CurrentToken => tokens.First?.Value;


        public void AppendToken(TokenType type, int line, int column)
        {
            var token = new LexerToken(type, null, line, column);
            tokens.AddLast(token);
        }


        public void PopToken()
        {
            tokens.RemoveFirst();
        }
    }
}
