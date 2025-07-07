// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using SwishCC.AssemblyGen;
using SwishCC.UnitTests.TestHelpers;
using Xunit;

namespace SwishCC.UnitTests.AssemblyGen
{
    public class AssemblyInstructionFixerUpperTests
    {
        private readonly AssemblyInstructionFixerUpper fixerUpper = new();

        private readonly AssemblyTreeParser parser = new();


        [Theory]
        [InlineData("cdq")]
        [InlineData("movl -4(%rbp), %r10d")]
        [InlineData("movl %r10d, -4(%rbp)")]
        [InlineData("idivl %r10d")]
        public void ProperInstructionsAreLeftAlong(string code)
        {
            // Arrange
            var inputs = parser.Parse(code);

            // Act
            var outputs = fixerUpper.FixInstructions(inputs);

            // Assert
            outputs.Should().HaveCount(1);

            outputs[0].Should().BeSameAs(inputs[0]);
        }


        [Fact]
        public void CanFixUpMovInstruction()
        {
            // Arrange
            var inputs = parser.Parse("movl -4(%rbp), -8(%rbp)");
            var expected = parser.Parse("movl -4(%rbp), %r10d; movl %r10d, -8(%rbp)");

            // Act
            var outputs = fixerUpper.FixInstructions(inputs);

            // Assert
            outputs.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }


        [Fact]
        public void CanFixUpIDivInstruction()
        {
            // Arrange
            var inputs = parser.Parse("idivl $3");
            var expected = parser.Parse("movl $3, %r10d; idivl %r10d");

            // Act
            var outputs = fixerUpper.FixInstructions(inputs);

            // Assert
            outputs.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }
    }
}
