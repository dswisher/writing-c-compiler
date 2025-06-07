using System;
using System.IO;

namespace SwishCC.Lexing
{
    public class LexerReader : IDisposable
    {
        private readonly TextReader reader;

        private bool disposed;


        public LexerReader(TextReader reader)
        {
            this.reader = reader;
        }


        public int LineNumber { get; private set; } = 1;
        public int ColumnNumber { get; private set; } = 1;


        public int Peek()
        {
            return reader.Peek();
        }


        public int Advance()
        {
            var ch = reader.Read();

            if (ch == '\n')
            {
                LineNumber += 1;
                ColumnNumber = 1;
            }
            else if (ch != -1)
            {
                ColumnNumber += 1;
            }

            return ch;
        }


        public void Dispose()
        {
            if (!disposed)
            {
                reader.Dispose();
                disposed = true;
            }
        }
    }
}
