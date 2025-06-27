// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.AssemblyTree
{
    public class AssemblyBinaryInstructionNode : AssemblyAbstractInstructionNode
    {
        public AssemblyBinaryInstructionNode(AssemblyBinaryOperator binaryOperator, AssemblyAbstractOperandNode operand1, AssemblyAbstractOperandNode operand2)
        {
            BinaryOperator = binaryOperator;
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public AssemblyBinaryOperator BinaryOperator { get; }
        public AssemblyAbstractOperandNode Operand1 { get; }
        public AssemblyAbstractOperandNode Operand2 { get; }
    }
}
