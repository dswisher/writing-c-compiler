// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SwishCC.Models.Common;

namespace SwishCC.Models.CTree
{
    public class CFunctionNode
    {
        public IdentifierNode Name { get; set; }
        public CReturnNode Body { get; set; }
    }
}
