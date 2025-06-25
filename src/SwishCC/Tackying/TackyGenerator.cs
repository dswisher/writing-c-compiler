// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using SwishCC.Exceptions;
using SwishCC.Models.Common;
using SwishCC.Models.CTree;
using SwishCC.Models.TackyTree;

namespace SwishCC.Tackying
{
    public class TackyGenerator
    {
        private int nextTemporaryIndex;


        public TackyProgramNode ConvertCTree(CProgramNode cProgram)
        {
            return new TackyProgramNode
            {
                FunctionDefinition = ConvertCTree(cProgram.FunctionDefinition)
            };
        }


        private TackyFunctionNode ConvertCTree(CFunctionNode cFunction)
        {
            var tackyFunction = new TackyFunctionNode
            {
                // TODO - only prepend an underscore if this is a Mac
                Name = new IdentifierNode($"_{cFunction.Name.Value}")
            };

            ConvertCTree(tackyFunction, cFunction.Body);

            return tackyFunction;
        }


        private void ConvertCTree(TackyFunctionNode tackyFunction, CReturnNode cReturn)
        {
            var op = ConvertCTree(cReturn.Expression, tackyFunction.Instructions);

            var tackyReturn = new TackyReturnInstructionNode
            {
                Value = op
            };

            tackyFunction.Instructions.Add(tackyReturn);
        }


        private TackyAbstractValueNode ConvertCTree(CAbstractExpressionNode exp, List<TackyAbstractInstructionNode> instructions)
        {
            if (exp is CConstantExpressionNode cce)
            {
                return new TackyConstantValueNode
                {
                    Value = cce.Value
                };
            }

            if (exp is CUnaryExpressionNode cue)
            {
                var src = ConvertCTree(cue.Operand, instructions);
                var dst = new TackyVariableValueNode(MakeTemporary());
                var op = cue.Operator.ToTacky();

                var unary = new TackyUnaryInstructionNode
                {
                    Operator = op,
                    Source = src,
                    Destination = dst
                };

                instructions.Add(unary);

                return dst;
            }

            if (exp is CBinaryExpressionNode cbe)
            {
                var src1 = ConvertCTree(cbe.Left, instructions);
                var src2 = ConvertCTree(cbe.Right, instructions);
                var dst = new TackyVariableValueNode(MakeTemporary());
                var op = cbe.Operator.ToTacky();

                var binary = new TackyBinaryInstructionNode
                {
                    Operator = op,
                    Source1 = src1,
                    Source2 = src2,
                    Destination = dst
                };

                instructions.Add(binary);

                return dst;
            }

            throw new CompilerException($"Emitting tacky for {exp.GetType().Name} is not yet implemented");
        }


        private string MakeTemporary()
        {
            return $"tmp.{++nextTemporaryIndex}";
        }
    }
}
