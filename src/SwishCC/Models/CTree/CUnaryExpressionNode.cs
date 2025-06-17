// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.CTree
{
    public class CUnaryExpressionNode : CAbstractExpressionNode
    {
        public CUnaryOperator Operator { get; set; }
        public CAbstractExpressionNode Operand { get; set; }
    }
}
