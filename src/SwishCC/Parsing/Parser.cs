// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SwishCC.Exceptions;
using SwishCC.Lexing;
using SwishCC.Models.Common;
using SwishCC.Models.CTree;

namespace SwishCC.Parsing
{
    public class Parser
    {
        public CProgramNode Parse(LexerResult tokens)
        {
            var programNode = new CProgramNode
            {
                FunctionDefinition = ParseFunction(tokens)
            };

            return programNode;
        }


        private CFunctionNode ParseFunction(LexerResult tokens)
        {
            // Parse the return type and function name
            tokens.ExpectAndPopKeyword("int");

            var identifierToken = tokens.ExpectAndPopToken(TokenType.Identifier);

            var functionNode = new CFunctionNode
            {
                Name = new IdentifierNode(identifierToken.Value)
            };

            // Parse the function parameters
            tokens.ExpectAndPopToken(TokenType.LeftParen);
            tokens.ExpectAndPopKeyword("void");
            tokens.ExpectAndPopToken(TokenType.RightParen);

            // Parse the function body
            tokens.ExpectAndPopToken(TokenType.LeftCurly);

            // TODO - parse statements - right now, all that is allowed is a single return statement
            functionNode.Body = ParseReturn(tokens);

            tokens.ExpectAndPopToken(TokenType.Semicolon);

            tokens.ExpectAndPopToken(TokenType.RightCurly);

            tokens.ExpectEnd();

            return functionNode;
        }


        private CReturnNode ParseReturn(LexerResult tokens)
        {
            tokens.ExpectAndPopKeyword("return");

            var returnNode = new CReturnNode
            {
                Expression = ParseExpression(tokens)
            };

            return returnNode;
        }


        private CAbstractExpressionNode ParseExpression(LexerResult tokens)
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


        private CUnaryOperator ParseUnaryOperator(LexerToken token)
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
