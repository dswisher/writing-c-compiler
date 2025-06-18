// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.AssemblyTree
{
    public class AssemblyImmediateOperandNode : AssemblyAbstractOperandNode
    {
        public AssemblyImmediateOperandNode(int value)
        {
            Value = value;
        }


        public int Value { get; }
    }
}
