// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using SwishCC.Exceptions;
using SwishCC.Models.AssemblyTree;

namespace SwishCC.AssemblyEmission
{
    public class AssemblyEmitter
    {
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Emit(AssemblyProgramNode assemblyProgram, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                Emit(writer, assemblyProgram.FunctionDefinition);
            }
        }


        private static void Emit(TextWriter writer, AssemblyFunctionNode assemblyFunction)
        {
            writer.WriteLine($"    .globl {assemblyFunction.Name.Value}");
            writer.WriteLine($"{assemblyFunction.Name.Value}:");
            writer.WriteLine("    pushq %rbp");
            writer.WriteLine("    movq %rsp, %rbp");

            foreach (var instruction in assemblyFunction.Instructions)
            {
                Emit(writer, instruction);
            }
        }


        private static void Emit(TextWriter writer, AssemblyAbstractInstructionNode assemblyInstruction)
        {
            switch (assemblyInstruction)
            {
                case AssemblyReturnInstructionNode:
                    writer.WriteLine("    movq %rbp, %rsp");
                    writer.WriteLine("    popq %rbp");
                    writer.WriteLine("    ret");
                    break;

                case AssemblyAllocateStackInstructionNode stack:
                    writer.WriteLine($"    subq ${stack.StackSize}, %rsp");
                    break;

                case AssemblyMoveInstructionNode moveInstruction:
                    writer.WriteLine($"    mov {Decode(moveInstruction.Source)}, {Decode(moveInstruction.Destination)}");
                    break;

                default:
                    throw new CompilerException($"Do not yet know how to emit assembly: {assemblyInstruction.GetType()}");
            }
        }


        private static string Decode(AssemblyAbstractOperandNode value)
        {
            if (value is AssemblyImmediateOperandNode imm)
            {
                return $"${imm.Value}";
            }

            if (value is AssemblyRegisterOperandNode reg)
            {
                switch (reg.Register)
                {
                    case AssemblyRegister.AX:
                        return "%eax";

                    case AssemblyRegister.R10:
                        return "%r10d";

                    default:
                        throw new CompilerException($"Don't know how to decode register: {reg.Register}");
                }
            }

            throw new CompilerException($"Don't know how to decode value: {value.GetType().Name}");
        }
    }
}
