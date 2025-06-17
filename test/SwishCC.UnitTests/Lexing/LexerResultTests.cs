// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
            var token = new LexerToken(TokenType.Semicolon, ";", 1, 1);

            result.AppendToken(token);

            // Act
            result.PopToken();

            // Assert
            result.CurrentToken.Should().BeNull();
        }
    }
}
