// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using SwishCC.Exceptions;
using SwishCC.Models.AssemblyTree;

namespace SwishCC.AssemblyGen
{
    public class AssemblyTreeWriter
    {
        public void Write(AssemblyProgramNode program, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
            {
                Write(writer, program.FunctionDefinition);
            }
        }


        private void Write(TextWriter writer, AssemblyFunctionNode function)
        {
            writer.WriteLine($"{function.Name.Value}:");

            foreach (var instruction in function.Instructions)
            {
                Write(writer, instruction);
            }
        }


        private void Write(TextWriter writer, AssemblyAbstractInstructionNode instruction)
        {
            if (instruction is AssemblyReturnInstructionNode)
            {
                writer.WriteLine("    Ret");
            }
            else if (instruction is AssemblyMoveInstructionNode mov)
            {
                var src = Decode(mov.Source);
                var dst = Decode(mov.Destination);

                writer.WriteLine($"    Mov({src}, {dst})");
            }
            else if (instruction is AssemblyAllocateStackInstructionNode stack)
            {
                writer.WriteLine($"    Stack({stack.StackSize})");
            }
            else
            {
                throw new DumpException($"Unsupported assembly instruction: {instruction.GetType().Name}");
            }
        }


        private string Decode(AssemblyAbstractOperandNode value)
        {
            return value switch
            {
                AssemblyImmediateOperandNode imm => $"Imm({imm.Value})",
                AssemblyRegisterOperandNode reg => $"Reg({reg.Register})",
                _ => throw new DumpException($"Unsupported assembly operand: {value.GetType().Name}")
            };
        }
    }
}
