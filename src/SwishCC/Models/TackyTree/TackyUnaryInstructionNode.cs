// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.TackyTree
{
    public class TackyUnaryInstructionNode : TackyAbstractInstructionNode
    {
        public TackyUnaryOperator Operator { get; set; }
        public TackyAbstractValueNode Source { get; set; }
        public TackyAbstractValueNode Destination { get; set; }
    }
}
