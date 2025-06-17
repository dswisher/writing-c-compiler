// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SwishCC.Lexing;

namespace SwishCC.Parsing
{
    public static class ParseExtensions
    {
        public static void ExpectAndPopKeyword(this LexerResult tokens, string keyword)
        {
            if (tokens.CurrentToken == null)
            {
                throw new ParseException($"Expected keyword {keyword}, but found end of input.");
            }

            if (tokens.CurrentToken.TokenType != TokenType.Keyword || tokens.CurrentToken.Value != keyword)
            {
                throw new ParseException(tokens.CurrentToken, $"Expected keyword '{keyword}' but found {tokens.CurrentToken.TokenType} ({tokens.CurrentToken.Value})");
            }

            tokens.PopToken();
        }


        public static LexerToken ExpectAndPopToken(this LexerResult tokens, TokenType tokenType)
        {
            if (tokens.CurrentToken == null)
            {
                throw new ParseException($"Expected {tokenType}, but found end of input.");
            }

            if (tokens.CurrentToken.TokenType != tokenType)
            {
                throw new ParseException(tokens.CurrentToken, $"Expected {tokenType}, but found {tokens.CurrentToken.TokenType} ({tokens.CurrentToken.Value})");
            }

            return tokens.PopToken();
        }


        public static void ExpectEnd(this LexerResult tokens)
        {
            if (tokens.CurrentToken != null)
            {
                throw new ParseException(tokens.CurrentToken, "Expected end of input but found more tokens.");
            }
        }
    }
}
