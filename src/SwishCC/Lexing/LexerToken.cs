// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Lexing
{
    public class LexerToken
    {
        public LexerToken(TokenType type, string value, int line, int column)
        {
            TokenType = type;
            Value = value;
            Line = line;
            Column = column;
        }

        public TokenType TokenType { get; }
        public string Value { get; }
        public int Line { get; }
        public int Column { get; }

        public override string ToString()
        {
            return $"{TokenType}({Value}) at {Line}:{Column}";
        }
    }
}
