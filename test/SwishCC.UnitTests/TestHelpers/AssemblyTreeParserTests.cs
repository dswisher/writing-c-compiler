// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using FluentAssertions;
using SwishCC.Models.AssemblyTree;
using Xunit;

namespace SwishCC.UnitTests.TestHelpers
{
    public class AssemblyTreeParserTests
    {
        private readonly AssemblyTreeParser parser = new();


        [Theory]
        [InlineData("cdq", typeof(AssemblyCdqInstructionNode))]
        public void CanParseZeroOperandInstruction(string code, Type expectedInstructionType)
        {
            // Act
            var instructions = parser.Parse(code);

            // Assert
            instructions.Should().HaveCount(1);
            instructions[0].Should().BeOfType(expectedInstructionType);
        }


        [Theory]
        [InlineData("idivl $3", typeof(AssemblyIDivInstructionNode), typeof(AssemblyImmediateOperandNode))]
        [InlineData("idivl %r10d", typeof(AssemblyIDivInstructionNode), typeof(AssemblyRegisterOperandNode))]
        public void CanParseOneOperandInstruction(string code, Type expectedInstructionType, Type expectedOperandType)
        {
            // Act
            var instructions = parser.Parse(code);

            // Assert
            instructions.Should().HaveCount(1);
            instructions[0].Should().BeOfType(expectedInstructionType);

            switch (instructions[0])
            {
                case AssemblyIDivInstructionNode divNode:
                    divNode.Operand.Should().BeOfType(expectedOperandType);
                    break;
            }
        }


        [Theory]
        [InlineData("movl -4(%rbp), -8(%rbp)", typeof(AssemblyMoveInstructionNode), typeof(AssemblyStackOperandNode), typeof(AssemblyStackOperandNode))]
        [InlineData("movl -4(%rbp), %r10d", typeof(AssemblyMoveInstructionNode), typeof(AssemblyStackOperandNode), typeof(AssemblyRegisterOperandNode))]
        [InlineData("movl %r10d, -4(%rbp)", typeof(AssemblyMoveInstructionNode), typeof(AssemblyRegisterOperandNode), typeof(AssemblyStackOperandNode))]
        public void CanParseTwoOperandInstruction(string code, Type expectedInstructionType, Type expectedOperandType1, Type expectedOperandType2)
        {
            // Act
            var instructions = parser.Parse(code);

            // Assert
            instructions.Should().HaveCount(1);
            instructions[0].Should().BeOfType(expectedInstructionType);

            switch (instructions[0])
            {
                case AssemblyMoveInstructionNode moveNode:
                    moveNode.Source.Should().BeOfType(expectedOperandType1);
                    moveNode.Destination.Should().BeOfType(expectedOperandType2);
                    break;
            }
        }
    }
}
