using System;
using System.IO;

namespace SwishCC.Lexing
{
    public class Lexer
    {
        private int lineNumber = 1;
        private int columnNumber = 1;


        public LexerResult Tokenize(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return Tokenize(reader);
            }
        }


        public LexerResult Tokenize(TextReader reader)
        {
            var result = new LexerResult();

            while (true)
            {
                // Read the next line
                var line = reader.ReadLine();

                // If we reach the end of the file, break the loop
                if (line == null)
                {
                    break;
                }

                // Reset column number for each new line
                columnNumber = 1;

                // Process all the characters in the line
                foreach (var character in line)
                {
                    switch (character)
                    {
                        case ';':
                            result.AppendToken(TokenType.Semicolon, lineNumber, columnNumber);
                            break;

                        case '{':
                            result.AppendToken(TokenType.LeftCurly, lineNumber, columnNumber);
                            break;

                        case '}':
                            result.AppendToken(TokenType.RightCurly, lineNumber, columnNumber);
                            break;

                        case '(':
                            result.AppendToken(TokenType.LeftParen, lineNumber, columnNumber);
                            break;

                        case ')':
                            result.AppendToken(TokenType.RightParen, lineNumber, columnNumber);
                            break;

                        case ' ':
                            break;

                        default:
                            throw new Exception($"Unexpected character '{character}' at line {lineNumber}, column {columnNumber}");
                    }

                    // Bump the column number
                    columnNumber += 1;
                }

                // Bump the line number
                lineNumber += 1;
            }

            // Return the tokens we found
            return result;
        }
    }
}
