using System.IO;
using FluentAssertions;
using SwishCC.Lexing;
using Xunit;

namespace SwishCC.UnitTests.Lexing
{
    public class LexerReaderTests
    {
        [Fact]
        public void PeekDoesNotConsume()
        {
            // Arrange
            var reader = new LexerReader(new StringReader("return 3;"));

            // Act
            var peeked = reader.Peek();

            // Assert
            peeked.Should().Be('r');
            reader.Advance().Should().Be('r');
        }


        [Fact]
        public void CanReadSimpleString()
        {
            // Arrange
            var reader = new LexerReader(new StringReader("return 3;"));

            // Act and assert
            reader.Advance().Should().Be('r');
            reader.Advance().Should().Be('e');
            reader.Advance().Should().Be('t');
            reader.Advance().Should().Be('u');
            reader.Advance().Should().Be('r');
            reader.Advance().Should().Be('n');
            reader.Advance().Should().Be(' ');
            reader.Advance().Should().Be('3');
            reader.Advance().Should().Be(';');
            reader.Advance().Should().Be(-1);
            reader.Advance().Should().Be(-1);
        }
    }
}
