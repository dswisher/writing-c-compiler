using FluentAssertions;
using SwishCC.Lexing;
using Xunit;

namespace SwishCC.UnitTests.Lexing
{
    public class LexerResultTests
    {
        private readonly LexerResult result = new();


        [Fact]
        public void PopTokenRemovesCurrent()
        {
            // Arrange
            result.AppendToken(TokenType.Semicolon, 1, 1);

            // Act
            result.PopToken();

            // Assert
            result.CurrentToken.Should().BeNull();
        }
    }
}
