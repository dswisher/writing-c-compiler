// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using SwishCC.Exceptions;
using SwishCC.Lexing;
using SwishCC.Models.CTree;

namespace SwishCC.Parsing
{
    public class ExpressionParser
    {
        private readonly Dictionary<TokenType, int> binaryOperatorPrecedence = new()
        {
            { TokenType.Star, 50 },
            { TokenType.Slash, 50 },
            { TokenType.Percent, 50 },
            { TokenType.Plus, 45 },
            { TokenType.Hyphen, 45 },
        };


        public CAbstractExpressionNode ParseExpression(LexerResult tokens, int minPrecedence = 0)
        {
            var left = ParseFactor(tokens);
            var nextToken = tokens.CurrentToken;

            while (nextToken != null && IsBinaryOperator(nextToken.TokenType) && binaryOperatorPrecedence[nextToken.TokenType] >= minPrecedence)
            {
                var binOp = ParseBinaryOperator(tokens.PopToken());
                var right = ParseExpression(tokens, binaryOperatorPrecedence[nextToken.TokenType] + 1);

                left = new CBinaryExpressionNode(binOp, left, right);

                nextToken = tokens.CurrentToken;
            }

            return left;
        }


        private CAbstractExpressionNode ParseFactor(LexerResult tokens)
        {
            // We should not be at the end
            if (tokens.CurrentToken == null)
            {
                throw new ParseException("Unexpected end of input while parsing expression.");
            }

            // An expression might be a constant...
            if (tokens.CurrentToken.TokenType == TokenType.Constant)
            {
                var constant = tokens.ExpectAndPopToken(TokenType.Constant);

                return new CConstantExpressionNode(int.Parse(constant.Value));
            }

            // ...or a unary operator followed by an expression...
            if (tokens.CurrentToken.TokenType == TokenType.Hyphen || tokens.CurrentToken.TokenType == TokenType.Tilde)
            {
                var unaryOp = ParseUnaryOperator(tokens.PopToken());
                var innerExpression = ParseFactor(tokens);

                return new CUnaryExpressionNode(unaryOp, innerExpression);
            }

            // ...or an expression in parentheses...
            if (tokens.CurrentToken.TokenType == TokenType.LeftParen)
            {
                tokens.ExpectAndPopToken(TokenType.LeftParen);

                var innerExpression = ParseExpression(tokens);

                tokens.ExpectAndPopToken(TokenType.RightParen);

                return innerExpression;
            }

            throw new ParseException(tokens.CurrentToken, "Unexpected token in factor.");
        }


        private static bool IsBinaryOperator(TokenType tokenType)
        {
            return tokenType == TokenType.Plus
                   || tokenType == TokenType.Hyphen
                   || tokenType == TokenType.Star
                   || tokenType == TokenType.Slash
                   || tokenType == TokenType.Percent;
        }


        private static CBinaryOperator ParseBinaryOperator(LexerToken token)
        {
            // TODO - replace this code, and IsBinaryOperator, with a dictionary lookup

            if (token.TokenType == TokenType.Hyphen)
            {
                return CBinaryOperator.Subtract;
            }

            if (token.TokenType == TokenType.Plus)
            {
                return CBinaryOperator.Add;
            }

            if (token.TokenType == TokenType.Star)
            {
                return CBinaryOperator.Multiply;
            }

            if (token.TokenType == TokenType.Slash)
            {
                return CBinaryOperator.Divide;
            }

            if (token.TokenType == TokenType.Percent)
            {
                return CBinaryOperator.Remainder;
            }

            throw new ParseException(token, $"Unexpected token {token.TokenType} ({token.Value}) when parsing binary operator.");
        }


        private static CUnaryOperator ParseUnaryOperator(LexerToken token)
        {
            if (token.TokenType == TokenType.Hyphen)
            {
                return CUnaryOperator.Negation;
            }

            if (token.TokenType == TokenType.Tilde)
            {
                return CUnaryOperator.Complement;
            }

            throw new ParseException(token, $"Unexpected token {token.TokenType} ({token.Value}) when parsing unary operator.");
        }
    }
}
