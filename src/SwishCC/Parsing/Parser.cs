using System;
using SwishCC.AST;
using SwishCC.Lexing;

namespace SwishCC.Parsing
{
    public class Parser
    {
        public TreeNode Parse(LexerResult tokens)
        {
            try
            {
                var programNode = new ProgramNode
                {
                    FunctionDefinition = ParseFunction(tokens)
                };

                return programNode;
            }
            catch (ParseException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        private FunctionNode ParseFunction(LexerResult tokens)
        {
            // Parse the return type and function name
            tokens.ExpectAndPopKeyword("int");

            var identifierToken = tokens.ExpectAndPopToken(TokenType.Identifier);

            var functionNode = new FunctionNode
            {
                Name = new Identifier(identifierToken.Value)
            };

            // Parse the function parameters
            tokens.ExpectAndPopToken(TokenType.LeftParen);
            tokens.ExpectAndPopKeyword("void");
            tokens.ExpectAndPopToken(TokenType.RightParen);

            // Parse the function body
            tokens.ExpectAndPopToken(TokenType.LeftCurly);

            // TODO - parse statements - this is a quick hack
            functionNode.Body = ParseReturn(tokens);

            tokens.ExpectAndPopToken(TokenType.Semicolon);

            tokens.ExpectAndPopToken(TokenType.RightCurly);

            tokens.ExpectEnd();

            return functionNode;
        }


        private ReturnNode ParseReturn(LexerResult tokens)
        {
            tokens.ExpectAndPopKeyword("return");
            var constant = tokens.ExpectAndPopToken(TokenType.Constant);

            var returnNode = new ReturnNode
            {
                Expression = new ConstantNode()
                {
                    Value = int.Parse(constant.Value)
                }
            };

            return returnNode;
        }
    }
}
