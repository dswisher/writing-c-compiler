// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using SwishCC.Exceptions;
using SwishCC.Models.CTree;

namespace SwishCC.Parsing
{
    public class ExpressionPostfixFormatter
    {
        private readonly Dictionary<CUnaryOperator, string> unaryMap;
        private readonly Dictionary<CBinaryOperator, string> binaryMap;

        public ExpressionPostfixFormatter()
        {
            unaryMap = new Dictionary<CUnaryOperator, string>
            {
                { CUnaryOperator.Complement, "~" },
                { CUnaryOperator.Negation, "NEG" }
            };

            binaryMap = new Dictionary<CBinaryOperator, string>
            {
                { CBinaryOperator.Add, "+" },
                { CBinaryOperator.Subtract, "-" },
                { CBinaryOperator.Multiply, "*" },
                { CBinaryOperator.Divide, "/" },
                { CBinaryOperator.Remainder, "%" },
            };
        }


        public string Format(CAbstractExpressionNode root)
        {
            var bits = new List<string>();

            Format(root, bits);

            return string.Join(' ', bits);
        }


        private void Format(CAbstractExpressionNode node, List<string> bits)
        {
            if (node is CConstantExpressionNode constant)
            {
                bits.Add(constant.Value.ToString());
            }
            else if (node is CUnaryExpressionNode unary)
            {
                // Add the operand and then the operator
                Format(unary.Operand, bits);
                Format(unary.Operator, bits);
            }
            else if (node is CBinaryExpressionNode binary)
            {
                // Add both operands and then the operator
                Format(binary.Left, bits);
                Format(binary.Right, bits);
                Format(binary.Operator, bits);
            }
            else
            {
                throw new DumpException($"Do not know how to format {node.GetType().Name}");
            }
        }


        private void Format(CUnaryOperator op, List<string> bits)
        {
            if (unaryMap.TryGetValue(op, out var code))
            {
                bits.Add(code);
            }
            else
            {
                throw new DumpException($"Don't know how to format unary operator {op}.");
            }
        }


        private void Format(CBinaryOperator op, List<string> bits)
        {
            if (binaryMap.TryGetValue(op, out var code))
            {
                bits.Add(code);
            }
            else
            {
                throw new DumpException($"Don't know how to format binary operator {op}.");
            }
        }
    }
}
