// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.CTree
{
    public class CBinaryExpressionNode : CAbstractExpressionNode
    {
        public CBinaryOperator Operator { get; set; }
        public CAbstractExpressionNode Left { get; set; }
        public CAbstractExpressionNode Right { get; set; }
    }
}
