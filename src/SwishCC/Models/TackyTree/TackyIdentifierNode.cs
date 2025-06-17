// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.TackyTree
{
    public class TackyIdentifierNode
    {
        public TackyIdentifierNode(string value)
        {
            Value = value;
        }


        public string Value { get; private set; }
    }
}
