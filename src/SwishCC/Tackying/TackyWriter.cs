// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using SwishCC.Misc;
using SwishCC.Models.TackyTree;

namespace SwishCC.Tackying
{
    public class TackyWriter
    {
        public void Write(TackyProgramNode program, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
            {
                Write(writer, program.FunctionDefinition);
            }
        }


        private void Write(TextWriter writer, TackyFunctionNode function)
        {
            writer.WriteLine($"{function.Name.Value}:");

            foreach (var instruction in function.Instructions)
            {
                Write(writer, instruction);
            }
        }


        private void Write(TextWriter writer, TackyAbstractInstructionNode instruction)
        {
            if (instruction is TackyReturnInstructionNode ret)
            {
                var val = Decode(ret.Value);
                writer.WriteLine($"    return {val};");
            }
            else if (instruction is TackyUnaryInstructionNode unary)
            {
                var src = Decode(unary.Source);
                var dst = Decode(unary.Destination);
                var op = unary.Operator switch
                {
                    TackyUnaryOperator.Negation => "-",
                    TackyUnaryOperator.Complement => "~",
                    _ => throw new DumpException($"Unsupported unary operator: {unary.Operator}")
                };

                writer.WriteLine($"    {dst} = {op}{src};");
            }
            else
            {
                throw new DumpException($"Unsupported tacky instruction: {instruction.GetType().Name}");
            }
        }


        private string Decode(TackyAbstractValueNode value)
        {
            if (value is TackyConstantValueNode constant)
            {
                return constant.Value.ToString();
            }

            if (value is TackyVariableValueNode variable)
            {
                return variable.Name;
            }

            throw new DumpException($"Unsupported value type: {value.GetType().Name}");
        }
    }
}
