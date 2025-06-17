// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using SwishCC.Models.CTree;
using SwishCC.Models.TackyTree;

namespace SwishCC.Tackying
{
    public class TackyGenerator
    {
        private int nextTemporaryIndex;


        public TackyProgramNode EmitTacky(CProgramNode cProgram)
        {
            return new TackyProgramNode
            {
                FunctionDefinition = EmitTacky(cProgram.FunctionDefinition)
            };
        }


        private TackyFunctionNode EmitTacky(CFunctionNode cFunction)
        {
            var tackyFunction = new TackyFunctionNode
            {
                Name = new TackyIdentifierNode(cFunction.Name.Value)
            };

            EmitTacky(tackyFunction, cFunction.Body);

            return tackyFunction;
        }


        private void EmitTacky(TackyFunctionNode tackyFunction, CReturnNode cReturn)
        {
            var op = EmitTacky(cReturn.Expression, tackyFunction.Instructions);

            var tackyReturn = new TackyReturnInstructionNode
            {
                Value = op
            };

            tackyFunction.Instructions.Add(tackyReturn);
        }


        private TackyAbstractValueNode EmitTacky(CAbstractExpressionNode exp, List<TackyAbstractInstructionNode> instructions)
        {
            // TODO - need to handle general expressions, for now, just handle constants
            if (exp is CConstantExpressionNode cce)
            {
                return new TackyConstantValueNode
                {
                    Value = cce.Value
                };
            }

            if (exp is CUnaryExpressionNode cue)
            {
                var src = EmitTacky(cue.Operand, instructions);
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

            throw new TackyException($"Emitting tacky for {exp.GetType().Name} is not yet implemented");
        }


        private string MakeTemporary()
        {
            return $"tmp.{++nextTemporaryIndex}";
        }
    }
}
