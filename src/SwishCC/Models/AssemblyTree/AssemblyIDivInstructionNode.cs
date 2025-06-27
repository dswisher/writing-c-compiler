// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.AssemblyTree
{
    public class AssemblyIDivInstructionNode : AssemblyAbstractInstructionNode
    {
        public AssemblyIDivInstructionNode(AssemblyAbstractOperandNode operand)
        {
            Operand = operand;
        }

        public AssemblyAbstractOperandNode Operand { get; }
    }
}
