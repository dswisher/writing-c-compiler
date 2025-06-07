using System.IO;
using FluentAssertions;
using SwishCC.Lexing;
using Xunit;

namespace SwishCC.UnitTests.Lexing
{
    public class LexerTests
    {
        private readonly Lexer lexer;

        public LexerTests()
        {
            lexer = new Lexer();
        }


        [Fact]
        public void CanLexOneToken()
        {
            // Arrange
            var stream = new StringReader(";");     // arbitrary choice of single-character token

            // Act
            var result = lexer.Tokenize(stream);

            // Assert
            result.Should().NotBeNull();
            result.CurrentToken.Should().NotBeNull();
            result.CurrentToken.TokenType.Should().Be(TokenType.Semicolon);
            result.CurrentToken.Value.Should().Be(";");
            result.CurrentToken.Line.Should().Be(1);
            result.CurrentToken.Column.Should().Be(1);
        }


        [Theory]
        [InlineData(";", TokenType.Semicolon)]
        [InlineData("(", TokenType.LeftParen)]
        [InlineData(")", TokenType.RightParen)]
        [InlineData("()", TokenType.LeftParen, TokenType.RightParen)]
        [InlineData("( ) { }", TokenType.LeftParen, TokenType.RightParen, TokenType.LeftCurly, TokenType.RightCurly)]
        [InlineData("123", TokenType.Constant)]
        public void CanLexSingleLines(string input, params TokenType[] expectedTypes)
        {
            // Arrange
            var stream = new StringReader(input);

            // Act
            var result = lexer.Tokenize(stream);

            // Assert
            VerifyTokens(result, expectedTypes);
        }


        private static void VerifyTokens(LexerResult result, params TokenType[] expectedTypes)
        {
            var index = 0;

            while (result.CurrentToken != null)
            {
                result.CurrentToken.TokenType.Should().Be(expectedTypes[index]);

                index += 1;

                result.PopToken();
            }

            // Ensure we processed all expected tokens
            index.Should().Be(expectedTypes.Length);
        }
    }
}
