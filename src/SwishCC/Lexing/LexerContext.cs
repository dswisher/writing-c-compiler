// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace SwishCC.Lexing
{
    public class LexerContext
    {
        private readonly StringBuilder characterBuffer = new();
        private readonly Dictionary<int, TokenType> charMap = new();


        public LexerContext(LexerState startState, LexerReader lexerReader, LexerResult result)
        {
            CurrentState = startState;
            LexerReader = lexerReader;
            Result = result;

            charMap.Add('{', TokenType.LeftCurly);
            charMap.Add('}', TokenType.RightCurly);
            charMap.Add('(', TokenType.LeftParen);
            charMap.Add(')', TokenType.RightParen);
            charMap.Add(';', TokenType.Semicolon);
            charMap.Add('~', TokenType.Tilde);
        }


        public LexerState CurrentState { get; set; }
        public LexerReader LexerReader { get; }
        public string CurrentBuffer => characterBuffer.ToString();
        public int LineNumber { get; private set; }
        public int ColumnNumber { get; private set; }
        public LexerResult Result { get; }


        public void EmitToken(TokenType tokenType)
        {
            var token = new LexerToken(tokenType, characterBuffer.ToString(), LineNumber, ColumnNumber);

            Result.AppendToken(token);

            characterBuffer.Clear();
        }


        public void AppendCharacter(int ch)
        {
            if (ch == -1)
            {
                throw new Exception("Attempt to append EOF to character buffer.");
            }

            if (characterBuffer.Length == 0)
            {
                LineNumber = LexerReader.LineNumber;
                ColumnNumber = LexerReader.ColumnNumber;
            }

            characterBuffer.Append((char)ch);
        }


        public void EmitToken(int ch)
        {
            // Figure out what type of token to create
            if (!charMap.TryGetValue(ch, out var tokenType))
            {
                // TODO - custom lexer exception
                throw new Exception($"Do not know how to emit token for character '{(char)ch}'");
            }

            AppendCharacter(ch);

            EmitToken(tokenType);
        }
    }
}
