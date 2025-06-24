// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.CTree
{
    public class CConstantExpressionNode : CAbstractExpressionNode
    {
        public CConstantExpressionNode(int val)
        {
            Value = val;
        }


        public int Value { get; }
    }
}
