// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SwishCC.Exceptions;
using SwishCC.Lexing;
using SwishCC.Models.CTree;

namespace SwishCC.Parsing
{
    public class ExpressionParser
    {
        public CAbstractExpressionNode ParseExpression(LexerResult tokens)
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

                return new CConstantExpressionNode
                {
                    Value = int.Parse(constant.Value)
                };
            }

            // ...or an expression in parentheses...
            if (tokens.CurrentToken.TokenType == TokenType.LeftParen)
            {
                tokens.ExpectAndPopToken(TokenType.LeftParen);

                var innerExpression = ParseExpression(tokens);

                tokens.ExpectAndPopToken(TokenType.RightParen);

                return innerExpression;
            }

            // ...or a unary operator followed by an expression...
            if (tokens.CurrentToken.TokenType == TokenType.Hyphen || tokens.CurrentToken.TokenType == TokenType.Tilde)
            {
                var unaryOp = ParseUnaryOperator(tokens.PopToken());
                var innerExpression = ParseExpression(tokens);

                return new CUnaryExpressionNode
                {
                    Operator = unaryOp,
                    Operand = innerExpression
                };
            }

            throw new ParseException(tokens.CurrentToken, "Malformed expression.");
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
