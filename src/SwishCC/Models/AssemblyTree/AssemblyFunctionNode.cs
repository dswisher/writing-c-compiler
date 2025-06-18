// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using SwishCC.Models.Common;

namespace SwishCC.Models.AssemblyTree
{
    public class AssemblyFunctionNode
    {
        public IdentifierNode Name { get; set; }
        public List<AssemblyAbstractInstructionNode> Instructions { get; } = [];
    }
}
