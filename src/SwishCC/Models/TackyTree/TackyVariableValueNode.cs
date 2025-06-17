// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.TackyTree
{
    public class TackyVariableValueNode : TackyAbstractValueNode
    {
        public TackyVariableValueNode(string name)
        {
            Name = name;
        }


        public string Name { get; private set; }
    }
}
