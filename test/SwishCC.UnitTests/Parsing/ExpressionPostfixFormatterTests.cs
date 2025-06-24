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
            var constant = new CConstantExpressionNode(val);
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
            var constant = new CConstantExpressionNode(16);
            var unary = new CUnaryExpressionNode(op, constant);

            // Act
            var result = formatter.Format(unary);

            // Assert
            result.Should().Be(expected);
        }


        [Fact]
        public void CanFormatCompoundUnaryExpression()
        {
            // Arrange
            var constant = new CConstantExpressionNode(316);
            var unary1 = new CUnaryExpressionNode(CUnaryOperator.Complement, constant);
            var unary2 = new CUnaryExpressionNode(CUnaryOperator.Negation, unary1);

            // Act
            var result = formatter.Format(unary2);

            // Assert
            result.Should().Be("316 ~ NEG");
        }


        [Fact]
        public void CanFormatSimpleBinaryExpression()
        {
            // Arrange
            var constant1 = new CConstantExpressionNode(2);
            var constant2 = new CConstantExpressionNode(3);
            var binary = new CBinaryExpressionNode(CBinaryOperator.Add, constant1, constant2);

            // Act
            var result = formatter.Format(binary);

            // Assert
            result.Should().Be("2 3 +");
        }
    }
}
