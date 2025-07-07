// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using SwishCC.Models.AssemblyTree;

namespace SwishCC.AssemblyGen
{
    public class AssemblyInstructionFixerUpper
    {
        public List<AssemblyAbstractInstructionNode> FixInstructions(List<AssemblyAbstractInstructionNode> inputList)
        {
            // Create the result list
            var outputList = new List<AssemblyAbstractInstructionNode>();

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
