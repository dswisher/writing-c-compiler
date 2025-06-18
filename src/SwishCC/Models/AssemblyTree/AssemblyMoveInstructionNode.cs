// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.AssemblyTree
{
    public class AssemblyMoveInstructionNode : AssemblyAbstractInstructionNode
    {
        public AssemblyMoveInstructionNode(AssemblyAbstractOperandNode source, AssemblyAbstractOperandNode destination)
        {
            Source = source;
            Destination = destination;
        }


        public AssemblyAbstractOperandNode Source { get; set; }
        public AssemblyAbstractOperandNode Destination { get; set; }
    }
}
