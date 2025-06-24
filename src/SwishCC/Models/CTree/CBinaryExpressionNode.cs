// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.CTree
{
    public class CBinaryExpressionNode : CAbstractExpressionNode
    {
        public CBinaryExpressionNode(CBinaryOperator operatorType, CAbstractExpressionNode left, CAbstractExpressionNode right)
        {
            Operator = operatorType;
            Left = left;
            Right = right;
        }


        public CBinaryOperator Operator { get; }
        public CAbstractExpressionNode Left { get; }
        public CAbstractExpressionNode Right { get; }
    }
}
