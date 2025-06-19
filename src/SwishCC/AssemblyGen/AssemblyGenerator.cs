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
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public AssemblyProgramNode ConvertTacky(TackyProgramNode tackyProgram)
        {
            // Convert TACKY to the assembly AST
            var assembly = new AssemblyProgramNode
            {
                FunctionDefinition = ConvertTacky(tackyProgram.FunctionDefinition)
            };

            // Replace pseudo register operands with stack operands
            // TODO - xyzzy - replace pseudo registers
            var stackSize = 0;

            // Add the allocate stack instruction to the start
            assembly.FunctionDefinition.Instructions.Insert(0, new AssemblyAllocateStackInstructionNode(stackSize));

            // Rewrite any invalid Mov instructions
            // TODO - xyzzy - rewrite mov instructions

            // Return the result
            return assembly;
        }


        private static AssemblyFunctionNode ConvertTacky(TackyFunctionNode tackyFunction)
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


        private static void ConvertTacky(TackyAbstractInstructionNode tackyInstruction, List<AssemblyAbstractInstructionNode> instructions)
        {
            if (tackyInstruction is TackyReturnInstructionNode ret)
            {
                // Mov(val, Reg(AX))
                var src = ConvertTacky(ret.Value);
                var dst = new AssemblyRegisterOperandNode(AssemblyRegister.AX);

                instructions.Add(new AssemblyMoveInstructionNode(src, dst));

                // Ret
                instructions.Add(new AssemblyReturnInstructionNode());
            }
            else if (tackyInstruction is TackyUnaryInstructionNode unary)
            {
                // Mov(src, dst)
                var src = ConvertTacky(unary.Source);
                var dst = ConvertTacky(unary.Destination);

                instructions.Add(new AssemblyMoveInstructionNode(src, dst));

                // Unary(unary_operator, dst)
                instructions.Add(new AssemblyUnaryInstructionNode(Convert(unary.Operator), dst));
            }
            else
            {
                throw new CompilerException($"Don't know how to convert tacky instruction of type {tackyInstruction.GetType().Name}");
            }
        }


        private static AssemblyAbstractOperandNode ConvertTacky(TackyAbstractValueNode tackyValue)
        {
            if (tackyValue is TackyConstantValueNode con)
            {
                return new AssemblyImmediateOperandNode(con.Value);
            }

            if (tackyValue is TackyVariableValueNode val)
            {
                return new AssemblyPseudoOperandNode(val.Name);
            }

            throw new CompilerException($"Don't know how to convert tacky value node of type {tackyValue.GetType().Name}");
        }


        private static AssemblyUnaryOperator Convert(TackyUnaryOperator tacky)
        {
            switch (tacky)
            {
                case TackyUnaryOperator.Complement:
                    return AssemblyUnaryOperator.Not;

                case TackyUnaryOperator.Negation:
                    return AssemblyUnaryOperator.Neg;

                default:
                    throw new CompilerException($"Don't know how to convert tacky unary operator {tacky}");
            }
        }
    }
}
