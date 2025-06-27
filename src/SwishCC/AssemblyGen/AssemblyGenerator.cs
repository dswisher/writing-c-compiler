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
            var instructions = new List<AssemblyAbstractInstructionNode>();

            var assembly = new AssemblyProgramNode
            {
                FunctionDefinition = ConvertTacky(tackyProgram.FunctionDefinition, instructions)
            };

            // Replace pseudo register operands with stack operands
            (instructions, var stackSize) = ReplacePseudoRegisters(instructions);

            // Rewrite any invalid Mov instructions
            instructions = AddStackAndReplaceInvalidMovInstructions(instructions, stackSize);

            // Save the list of instructions we have created
            assembly.FunctionDefinition.Instructions = instructions;

            // Return the result
            return assembly;
        }


        private static AssemblyFunctionNode ConvertTacky(TackyFunctionNode tackyFunction, List<AssemblyAbstractInstructionNode> instructions)
        {
            var node = new AssemblyFunctionNode
            {
                Name = new IdentifierNode(tackyFunction.Name.Value),
            };

            foreach (var instruction in tackyFunction.Instructions)
            {
                ConvertTacky(instruction, instructions);
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
            else if (tackyInstruction is TackyBinaryInstructionNode binary)
            {
                if (binary.Operator == TackyBinaryOperator.Divide || binary.Operator == TackyBinaryOperator.Remainder)
                {
                    // Mov(src1, Reg(AX))
                    var src1 = ConvertTacky(binary.Source1);
                    var regAx = new AssemblyRegisterOperandNode(AssemblyRegister.AX);

                    instructions.Add(new AssemblyMoveInstructionNode(src1, regAx));

                    // Cdq
                    instructions.Add(new AssemblyCdqInstructionNode());

                    // Idiv(src2)
                    var src2 = ConvertTacky(binary.Source2);

                    instructions.Add(new AssemblyIDivInstructionNode(src2));

                    if (binary.Operator == TackyBinaryOperator.Divide)
                    {
                        // Mov(Reg(AX), dst)
                        var dst = ConvertTacky(binary.Destination);

                        instructions.Add(new AssemblyMoveInstructionNode(regAx, dst));
                    }
                    else
                    {
                        // Mov(Reg(DX), dst)
                        var regDx = new AssemblyRegisterOperandNode(AssemblyRegister.DX);
                        var dst = ConvertTacky(binary.Destination);

                        instructions.Add(new AssemblyMoveInstructionNode(regDx, dst));
                    }
                }
                else
                {
                    // Mov(src1, dst)
                    var src1 = ConvertTacky(binary.Source1);
                    var dst = ConvertTacky(binary.Destination);

                    instructions.Add(new AssemblyMoveInstructionNode(src1, dst));

                    // Binary(op, src2, dst)
                    var src2 = ConvertTacky(binary.Source2);

                    instructions.Add(new AssemblyBinaryInstructionNode(Convert(binary.Operator), src2, dst));
                }
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


        private static AssemblyBinaryOperator Convert(TackyBinaryOperator tacky)
        {
            switch (tacky)
            {
                case TackyBinaryOperator.Add:
                    return AssemblyBinaryOperator.Add;

                case TackyBinaryOperator.Subtract:
                    return AssemblyBinaryOperator.Sub;

                case TackyBinaryOperator.Multiply:
                    return AssemblyBinaryOperator.Mult;

                default:
                    throw new CompilerException($"Don't know how to convert tacky binary operator {tacky}");
            }
        }


        private static (List<AssemblyAbstractInstructionNode> Instructions, int StackSize) ReplacePseudoRegisters(List<AssemblyAbstractInstructionNode> inputList)
        {
            // Create the result list and the map of pseudo registers to stack offsets
            var outputList = new List<AssemblyAbstractInstructionNode>();
            var pseudoMap = new Dictionary<string, int>();

            // Go through all the input instructions and replace pseudo registers
            foreach (var inputInstruction in inputList)
            {
                if (inputInstruction is AssemblyMoveInstructionNode mov)
                {
                    var src = ReplaceOperand(pseudoMap, mov.Source);
                    var dst = ReplaceOperand(pseudoMap, mov.Destination);

                    outputList.Add(new AssemblyMoveInstructionNode(src, dst));
                }
                else if (inputInstruction is AssemblyUnaryInstructionNode unary)
                {
                    var operand = ReplaceOperand(pseudoMap, unary.Operand);

                    outputList.Add(new AssemblyUnaryInstructionNode(unary.UnaryOperator, operand));
                }
                else if (inputInstruction is AssemblyBinaryInstructionNode binary)
                {
                    var arg1 = ReplaceOperand(pseudoMap, binary.Operand1);
                    var arg2 = ReplaceOperand(pseudoMap, binary.Operand2);

                    outputList.Add(new AssemblyBinaryInstructionNode(binary.BinaryOperator, arg1, arg2));
                }
                else if (inputInstruction is AssemblyIDivInstructionNode div)
                {
                    var arg = ReplaceOperand(pseudoMap, div.Operand);

                    outputList.Add(new AssemblyIDivInstructionNode(arg));
                }
                else if (inputInstruction is AssemblyReturnInstructionNode)
                {
                    outputList.Add(inputInstruction);
                }
                else if (inputInstruction is AssemblyCdqInstructionNode)
                {
                    outputList.Add(inputInstruction);
                }
                else
                {
                    throw new CompilerException($"Don't know how to fix up pseudo registers for instruction of type {inputInstruction.GetType().Name}");
                }
            }

            // Return the result
            return (outputList, pseudoMap.Count * 4);
        }


        private static AssemblyAbstractOperandNode ReplaceOperand(Dictionary<string, int> pseudoMap, AssemblyAbstractOperandNode input)
        {
            if (input is AssemblyPseudoOperandNode pseudo)
            {
                if (!pseudoMap.TryGetValue(pseudo.Name, out var offset))
                {
                    offset = (pseudoMap.Count + 1) * -4;

                    pseudoMap.Add(pseudo.Name, offset);
                }

                return new AssemblyStackOperandNode(offset);
            }

            // For everything else, just return it
            return input;
        }


        private static List<AssemblyAbstractInstructionNode> AddStackAndReplaceInvalidMovInstructions(List<AssemblyAbstractInstructionNode> inputList, int stackSize)
        {
            // Create the result list
            var outputList = new List<AssemblyAbstractInstructionNode>();

            // Add the allocate stack instruction to start
            outputList.Add(new AssemblyAllocateStackInstructionNode(stackSize));

            // Go through all the input instructions, looking for invalid mov instructions
            foreach (var inputInstruction in inputList)
            {
                if (inputInstruction is AssemblyMoveInstructionNode move)
                {
                    if (move.Source is AssemblyStackOperandNode src && move.Destination is AssemblyStackOperandNode dst)
                    {
                        var temp = new AssemblyRegisterOperandNode(AssemblyRegister.R10);

                        outputList.Add(new AssemblyMoveInstructionNode(src, temp));
                        outputList.Add(new AssemblyMoveInstructionNode(temp, dst));
                    }
                    else
                    {
                        outputList.Add(inputInstruction);
                    }
                }
                else
                {
                    outputList.Add(inputInstruction);
                }
            }

            // Return the result
            return outputList;
        }
    }
}
