// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.CTree
{
    public class CUnaryExpressionNode : CAbstractExpressionNode
    {
        public CUnaryExpressionNode(CUnaryOperator unaryOp, CAbstractExpressionNode operand)
        {
            Operator = unaryOp;
            Operand = operand;
        }

        public CUnaryOperator Operator { get; }
        public CAbstractExpressionNode Operand { get; }
    }
}
