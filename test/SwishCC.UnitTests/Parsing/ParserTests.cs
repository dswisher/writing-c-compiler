using System.IO;
using FluentAssertions;
using SwishCC.Lexing;
using SwishCC.Models.CTree;
using SwishCC.Parsing;
using Xunit;

namespace SwishCC.UnitTests.Parsing
{
    public class ParserTests
    {
        private readonly Lexer lexer = new();
        private readonly Parser parser = new();


        [Fact]
        public void CanParseChapter1Sample()
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
            ast.Should().BeOfType<CProgramNode>();

            // TODO - check the rest of the tree
        }


        [Theory]
        [InlineData("2")]
        [InlineData("(2)")]
        [InlineData("-2")]
        [InlineData("~2")]
        [InlineData("-(2)")]
        [InlineData("(-2)")]
        [InlineData("~(-2)")]
        public void CanParseExpressions(string exp)
        {
            // Arrange
            var content = "int main(void) {\n" +
                $"    return {exp};\n" +
                "}";

            LexerResult tokens;

            using (var reader = new StringReader(content))
            {
                tokens = lexer.Tokenize(reader);
            }

            // Act
            var ast = parser.Parse(tokens);

            // Assert
            ast.Should().NotBeNull();
            ast.Should().BeOfType<CProgramNode>();

            // TODO - check the rest of the tree
        }
    }
}
