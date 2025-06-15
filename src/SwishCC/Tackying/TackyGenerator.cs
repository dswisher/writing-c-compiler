using SwishCC.Models.CTree;
using SwishCC.Models.TackyTree;

namespace SwishCC.Tackying
{
    public class TackyGenerator
    {
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
            // TODO - need to handle general expressions, for now, just handle constants
            if (cReturn.Expression is CConstantExpressionNode cc)
            {
                var tackyReturn = new TackyReturnInstructionNode
                {
                    Value = new TackyConstantValueNode
                    {
                        Value = cc.Value
                    }
                };

                tackyFunction.Instructions.Add(tackyReturn);
            }
            else
            {
                throw new TackyException($"Emitting tacky for {cReturn.Expression.GetType().Name} is not yet implemented");
            }
        }
    }
}
