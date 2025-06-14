using SwishCC.AST;
using SwishCC.Lexing;

namespace SwishCC.Parsing
{
    public class Parser
    {
        public TreeNode Parse(LexerResult tokens)
        {
            var programNode = new ProgramNode
            {
                FunctionDefinition = ParseFunction(tokens)
            };

            return programNode;
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

            var returnNode = new ReturnNode
            {
                Expression = ParseExpression(tokens)
            };

            return returnNode;
        }


        private ExpressionNode ParseExpression(LexerResult tokens)
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

                return new ConstantExpressionNode
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

                return new UnaryExpressionNode
                {
                    Operator = unaryOp,
                    Operand = innerExpression
                };
            }

            throw new ParseException(tokens.CurrentToken, "Malformed expression.");
        }


        private UnaryOperator ParseUnaryOperator(LexerToken token)
        {
            if (token.TokenType == TokenType.Hyphen)
            {
                return UnaryOperator.Negation;
            }

            if (token.TokenType == TokenType.Tilde)
            {
                return UnaryOperator.Complement;
            }

            throw new ParseException(token, $"Unexpected token {token.TokenType} ({token.Value}) when parsing unary operator.");
        }
    }
}
