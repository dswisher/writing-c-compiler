// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using FluentAssertions;
using SwishCC.Lexing;
using Xunit;

namespace SwishCC.UnitTests.Lexing
{
    public class LexerHelpersTests
    {
        [Theory]
        [InlineData("a-c", 'a', 'b', 'c')]
        [InlineData(" \n", ' ', '\n')]
        [InlineData("Aa2", 'A', 'a', '2')]
        [InlineData("{}[],:", '{', '}', '[', ']', ',', ':')]
        [InlineData("1-3-", '1', '2', '3', '-')]
        [InlineData("<+d-fA", '<', '+', 'd', 'e', 'f', 'A')]
        [InlineData("x-zX-Z1-3", 'x', 'y', 'z', 'X', 'Y', 'Z', '1', '2', '3')]
        public void CanExpandRange(string range, params char[] expected)
        {
            // Act
            var actual = range.ExpandRange().ToList();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
