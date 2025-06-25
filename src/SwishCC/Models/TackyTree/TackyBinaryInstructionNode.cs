// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.TackyTree
{
    public class TackyBinaryInstructionNode : TackyAbstractInstructionNode
    {
        public TackyBinaryOperator Operator { get; set; }
        public TackyAbstractValueNode Source1 { get; set; }
        public TackyAbstractValueNode Source2 { get; set; }
        public TackyAbstractValueNode Destination { get; set; }
    }
}
