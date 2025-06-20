// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using SwishCC.Models.CTree;
using SwishCC.Parsing;
using Xunit;

namespace SwishCC.UnitTests.Parsing
{
    public class ExpressionPostfixFormatterTests
    {
        private readonly ExpressionPostfixFormatter formatter = new();


        [Theory]
        [InlineData(2)]
        [InlineData(17)]
        public void CanFormatConstantExpression(int val)
        {
            // Arrange
            var constant = new CConstantExpressionNode { Value = val };
            var expected = val.ToString();

            // Act
            var result = formatter.Format(constant);

            // Assert
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData(CUnaryOperator.Complement, "16 ~")]
        [InlineData(CUnaryOperator.Negation, "16 NEG")]
        public void CanFormatSimpleUnaryExpression(CUnaryOperator op, string expected)
        {
            // Arrange
            var constant = new CConstantExpressionNode { Value = 16 };
            var unary = new CUnaryExpressionNode
            {
                Operator = op,
                Operand = constant
            };

            // Act
            var result = formatter.Format(unary);

            // Assert
            result.Should().Be(expected);
        }


        [Fact]
        public void CanFormatCompoundUnaryExpression()
        {
            // Arrange
            var constant = new CConstantExpressionNode { Value = 316 };
            var unary1 = new CUnaryExpressionNode
            {
                Operator = CUnaryOperator.Complement,
                Operand = constant
            };
            var unary2 = new CUnaryExpressionNode
            {
                Operator = CUnaryOperator.Negation,
                Operand = unary1
            };

            // Act
            var result = formatter.Format(unary2);

            // Assert
            result.Should().Be("316 ~ NEG");
        }


        [Fact]
        public void CanFormatSimpleBinaryExpression()
        {
            // Arrange
            var constant1 = new CConstantExpressionNode { Value = 2 };
            var constant2 = new CConstantExpressionNode { Value = 3 };
            var binary = new CBinaryExpressionNode
            {
                Operator = CBinaryOperator.Add,
                Left = constant1,
                Right = constant2
            };

            // Act
            var result = formatter.Format(binary);

            // Assert
            result.Should().Be("2 3 +");
        }
    }
}
