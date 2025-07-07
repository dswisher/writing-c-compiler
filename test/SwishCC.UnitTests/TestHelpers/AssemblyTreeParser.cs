// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SwishCC.Models.AssemblyTree;

namespace SwishCC.UnitTests.TestHelpers
{
    public class AssemblyTreeParser
    {
        private readonly Dictionary<string, AssemblyRegister> registerMap = new();

        private readonly Regex immediateRegex = new(@"^\$(?<val>\d+)$");
        private readonly Regex registerRegex = new(@"^%(?<reg>\w+)$");
        private readonly Regex stackRegex = new(@"^(?<offset>-\d+)\(%rbp\)$");


        public AssemblyTreeParser()
        {
            registerMap.Add("r10d", AssemblyRegister.R10);
        }


        public List<AssemblyAbstractInstructionNode> Parse(string code)
        {
            // Create the list to be populated.
            var output = new List<AssemblyAbstractInstructionNode>();

            // Multiple instructions are separated by semicolons. So first split on those,
            // and then handle each instruction separately.
            var lines = code.Split(';');

            foreach (var line in lines)
            {
                ParseLine(output, line.Trim());
            }

            // Return what we've got
            return output;
        }


        private void ParseLine(List<AssemblyAbstractInstructionNode> output, string line)
        {
            // Split the line into instruction and operands, based on whitespace.
            var bits = line.Split(' ');

            var instruction = bits.First().Trim();
            var opString1 = bits.Skip(1).FirstOrDefault()?.Trim().TrimEnd(',');
            var opString2 = bits.Skip(2).FirstOrDefault()?.Trim();

            switch (instruction)
            {
                case "cdq":
                    ParseZeroOperandInstruction(output, instruction);
                    break;

                case "idivl":
                    ParseOneOperandInstruction(output, instruction, opString1);
                    break;

                case "movl":
                    ParseTwoOperandInstruction(output, instruction, opString1, opString2);
                    break;

                default:
                    throw new Exception($"Don't know how to parse instruction '{instruction}'");
            }
        }


        private static void ParseZeroOperandInstruction(List<AssemblyAbstractInstructionNode> output, string instruction)
        {
            switch (instruction)
            {
                case "cdq":
                    output.Add(new AssemblyCdqInstructionNode());
                    break;

                default:
                    throw new Exception($"Don't know how to parse zero-operand instruction '{instruction}'");
            }
        }


        private void ParseOneOperandInstruction(List<AssemblyAbstractInstructionNode> output, string instruction, string opString1)
        {
            var op1 = ParseOperand(opString1);

            switch (instruction)
            {
                case "idivl":
                    output.Add(new AssemblyIDivInstructionNode(op1));
                    break;

                default:
                    throw new Exception($"Don't know how to parse one-operand instruction '{instruction}'");
            }
        }


        private void ParseTwoOperandInstruction(List<AssemblyAbstractInstructionNode> output, string instruction, string opString1, string opString2)
        {
            var op1 = ParseOperand(opString1);
            var op2 = ParseOperand(opString2);

            switch (instruction)
            {
                case "movl":
                    output.Add(new AssemblyMoveInstructionNode(op1, op2));
                    break;

                default:
                    throw new Exception($"Don't know how to parse two-operand instruction '{instruction}'");
            }
        }


        private AssemblyAbstractOperandNode ParseOperand(string operandString)
        {
            // Immediate operands, like $3
            var match = immediateRegex.Match(operandString);
            if (match.Success)
            {
                var value = int.Parse(match.Groups["val"].Value);
                return new AssemblyImmediateOperandNode(value);
            }

            // Register operands, like %r10d
            match = registerRegex.Match(operandString);
            if (match.Success)
            {
                var reg = match.Groups["reg"].Value;
                if (registerMap.TryGetValue(reg, out var register))
                {
                    return new AssemblyRegisterOperandNode(register);
                }

                throw new Exception($"Don't know how to parse register operand '{operandString}'");
            }

            // Stack operands, like -4(%rbp)
            match = stackRegex.Match(operandString);
            if (match.Success)
            {
                var offset = int.Parse(match.Groups["offset"].Value);
                return new AssemblyStackOperandNode(offset);
            }

            // Oops! Don't know what the heck this is!
            throw new Exception($"Don't know how to parse operand '{operandString}'");
        }
    }
}
