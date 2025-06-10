using FluentAssertions;
using SwishCC.AST;
using SwishCC.Lexing;
using SwishCC.Parsing;
using Xunit;

namespace SwishCC.UnitTests.Parsing
{
    public class ParserTests
    {
        private readonly Parser parser = new();


        [Fact]
        public void CanParseSimpleProgram()
        {
            // Arrange
            var tokens = new LexerResult();

            tokens.AppendToken(new LexerToken(TokenType.Keyword, "int", 1, 1));
            tokens.AppendToken(new LexerToken(TokenType.Identifier, "main", 1, 5));
            tokens.AppendToken(new LexerToken(TokenType.LeftParen, "(", 1, 9));
            tokens.AppendToken(new LexerToken(TokenType.Keyword, "void", 1, 10));
            tokens.AppendToken(new LexerToken(TokenType.RightParen, ")", 1, 14));
            tokens.AppendToken(new LexerToken(TokenType.LeftCurly, "{", 1, 16));
            tokens.AppendToken(new LexerToken(TokenType.Keyword, "return", 2, 5));
            tokens.AppendToken(new LexerToken(TokenType.Constant, "2", 2, 12));
            tokens.AppendToken(new LexerToken(TokenType.Semicolon, ";", 2, 13));
            tokens.AppendToken(new LexerToken(TokenType.RightCurly, "}", 3, 1));

            // Act
            var ast = parser.Parse(tokens);

            // Assert
            ast.Should().NotBeNull();
            ast.Should().BeOfType<ProgramNode>();

            // TODO - check the rest of the tree
        }
    }
}
