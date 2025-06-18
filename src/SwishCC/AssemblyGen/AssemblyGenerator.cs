// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using SwishCC.Exceptions;
using SwishCC.Models.AssemblyTree;
using SwishCC.Models.Common;
using SwishCC.Models.TackyTree;

namespace SwishCC.AssemblyGen
{
    public class AssemblyGenerator
    {
        public AssemblyProgramNode ConvertTacky(TackyProgramNode tackyProgram)
        {
            // Convert TACKY to the assembly AST
            var assembly = new AssemblyProgramNode
            {
                FunctionDefinition = ConvertTacky(tackyProgram.FunctionDefinition)
            };

            // Replace pseudo register operands with stack operands
            // TODO - replace pseudo registers
            var stackSize = 0;

            // Add the allocate stack instruction to the start
            assembly.FunctionDefinition.Instructions.Insert(0, new AssemblyAllocateStackInstructionNode(stackSize));

            // Rewrite any invalid Mov instructions
            // TODO - rewrite mov instructions

            // Return the result
            return assembly;
        }


        private AssemblyFunctionNode ConvertTacky(TackyFunctionNode tackyFunction)
        {
            var node = new AssemblyFunctionNode
            {
                Name = new IdentifierNode(tackyFunction.Name.Value),
            };

            foreach (var instruction in tackyFunction.Instructions)
            {
                ConvertTacky(instruction, node.Instructions);
            }

            return node;
        }


        private void ConvertTacky(TackyAbstractInstructionNode tackyInstruction, List<AssemblyAbstractInstructionNode> instructions)
        {
            if (tackyInstruction is TackyReturnInstructionNode ret)
            {
                // TODO - handle return instruction

                // Mov(val, Reg(AX))
                var val = ConvertTacky(ret.Value);
                var reg = new AssemblyRegisterOperandNode(AssemblyRegister.AX);

                instructions.Add(new AssemblyMoveInstructionNode(val, reg));

                // Ret
                instructions.Add(new AssemblyReturnInstructionNode());
            }
            else if (tackyInstruction is TackyUnaryInstructionNode unary)
            {
                // TODO - handle unary instruction
#if false
                Mov(src, dst)
                Unary(unary_operator, dst)
#endif
                throw new CompilerException("TackyUnaryInstructionNode is not yet implemented");
            }
            else
            {
                throw new CompilerException($"Don't know how to convert tacky instruction of type {tackyInstruction.GetType().Name}");
            }
        }


        private AssemblyAbstractOperandNode ConvertTacky(TackyAbstractValueNode tackyValue)
        {
            if (tackyValue is TackyConstantValueNode con)
            {
                return new AssemblyImmediateOperandNode(con.Value);
            }

            throw new CompilerException($"Don't know how to convert tacky value node of type {tackyValue.GetType().Name}");
        }
    }
}
