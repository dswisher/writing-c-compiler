// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
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


        [Fact]
        public void CanLexOneNumber()
        {
            // Arrange
            var stream = new StringReader("564");

            // Act
            var result = lexer.Tokenize(stream);

            // Assert
            result.Should().NotBeNull();
            result.CurrentToken.Should().NotBeNull();
            result.CurrentToken.TokenType.Should().Be(TokenType.Constant);
            result.CurrentToken.Value.Should().Be("564");
        }


        [Fact]
        public void CanLexOneIdentifier()
        {
            // Arrange
            var stream = new StringReader("main");

            // Act
            var result = lexer.Tokenize(stream);

            // Assert
            result.Should().NotBeNull();
            result.CurrentToken.Should().NotBeNull();
            result.CurrentToken.TokenType.Should().Be(TokenType.Identifier);
            result.CurrentToken.Value.Should().Be("main");
        }


        [Fact]
        public void CanLexOneKeyword()
        {
            // Arrange
            var stream = new StringReader("int");

            // Act
            var result = lexer.Tokenize(stream);

            // Assert
            result.Should().NotBeNull();
            result.CurrentToken.Should().NotBeNull();
            result.CurrentToken.TokenType.Should().Be(TokenType.Keyword);
            result.CurrentToken.Value.Should().Be("int");
        }


        [Theory]
        [InlineData(";", TokenType.Semicolon)]
        [InlineData("(", TokenType.LeftParen)]
        [InlineData(")", TokenType.RightParen)]
        [InlineData("~", TokenType.Tilde)]
        [InlineData("-", TokenType.Hyphen)]
        [InlineData("--", TokenType.TwoHyphens)]
        [InlineData("()", TokenType.LeftParen, TokenType.RightParen)]
        [InlineData("( ) { }", TokenType.LeftParen, TokenType.RightParen, TokenType.LeftCurly, TokenType.RightCurly)]
        [InlineData("123", TokenType.Constant)]
        [InlineData("main", TokenType.Identifier)]
        [InlineData("int", TokenType.Keyword)]
        [InlineData("int\tmain", TokenType.Keyword, TokenType.Identifier)]
        public void CanLexSingleLines(string input, params TokenType[] expectedTypes)
        {
            // Arrange
            var stream = new StringReader(input);

            // Act
            var result = lexer.Tokenize(stream);

            // Assert
            VerifyTokens(result, expectedTypes);
        }


        [Theory]
        [InlineData("int main\n{\n}\n", TokenType.Keyword, TokenType.Identifier, TokenType.LeftCurly, TokenType.RightCurly)]
        public void CanLexMultipleLines(string input, params TokenType[] expectedTypes)
        {
            // Arrange
            var stream = new StringReader(input);

            // Act
            var result = lexer.Tokenize(stream);

            // Assert
            VerifyTokens(result, expectedTypes);
        }


        [Theory]
        [InlineData("return 1foo;")]
        public void CanDetectErrors(string input)
        {
            // Arrange
            var stream = new StringReader(input);

            Action act = () => lexer.Tokenize(stream);

            // Act and assert
            act.Should().Throw<Exception>();
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
