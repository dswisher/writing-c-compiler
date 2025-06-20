// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SwishCC.Lexing;
using SwishCC.Models.Common;
using SwishCC.Models.CTree;

namespace SwishCC.Parsing
{
    public class Parser
    {
        private readonly ExpressionParser expressionParser;

        public Parser()
        {
            expressionParser = new ExpressionParser();
        }


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
                Expression = expressionParser.ParseExpression(tokens)
            };

            return returnNode;
        }
    }
}
