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
            var left = ParseFactor(tokens);

            while (tokens.CurrentToken?.TokenType == TokenType.Plus || tokens.CurrentToken?.TokenType == TokenType.Hyphen)
            {
                var binOp = ParseBinaryOperator(tokens.PopToken());
                var right = ParseFactor(tokens);

                left = new CBinaryExpressionNode(binOp, left, right);
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


        private static CBinaryOperator ParseBinaryOperator(LexerToken token)
        {
            if (token.TokenType == TokenType.Hyphen)
            {
                return CBinaryOperator.Subtract;
            }

            if (token.TokenType == TokenType.Plus)
            {
                return CBinaryOperator.Add;
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
