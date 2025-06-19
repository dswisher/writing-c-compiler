// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.AssemblyTree
{
    public class AssemblyUnaryInstructionNode : AssemblyAbstractInstructionNode
    {
        public AssemblyUnaryInstructionNode(AssemblyUnaryOperator unaryOperator, AssemblyAbstractOperandNode operand)
        {
            UnaryOperator = unaryOperator;
            Operand = operand;
        }


        public AssemblyUnaryOperator UnaryOperator { get; set; }
        public AssemblyAbstractOperandNode Operand { get; set; }
    }
}
