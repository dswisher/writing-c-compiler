// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using FluentAssertions;
using SwishCC.Lexing;
using SwishCC.Parsing;
using Xunit;

namespace SwishCC.UnitTests.Parsing
{
    public class ExpressionParserTests
    {
        private readonly Lexer lexer = new();
        private readonly ExpressionParser parser = new();
        private readonly ExpressionPostfixFormatter formatter = new();


        [Theory]
        [InlineData("2", "2")]
        [InlineData("(2)", "2")]
        [InlineData("-2", "2 NEG")]
        [InlineData("~2", "2 ~")]
        [InlineData("-(2)", "2 NEG")]
        [InlineData("(-2)", "2 NEG")]
        [InlineData("~(-2)", "2 NEG ~")]
        [InlineData("-(~2)", "2 ~ NEG")]
        [InlineData("1 + 2", "1 2 +")]
        [InlineData("2 - 1", "2 1 -")]
        [InlineData("1 + 2 + 3", "1 2 + 3 +")]
        public void CanParseExpressions(string expr, string expected)
        {
            // Arrange
            LexerResult tokens;

            using (var reader = new StringReader(expr))
            {
                tokens = lexer.Tokenize(reader);
            }

            // Act
            var ast = parser.ParseExpression(tokens);

            // Assert
            ast.Should().NotBeNull();

            var result = formatter.Format(ast);

            result.Should().Be(expected);
        }
    }
}
