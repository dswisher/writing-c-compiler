using System;

namespace SwishCC.Lexing
{
    public class LexException : Exception
    {
        public LexException(string message)
            : base(message)
        {
        }
    }
}
