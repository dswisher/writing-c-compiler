// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace SwishCC.Models.AssemblyTree
{
    public class AssemblyRegisterOperandNode : AssemblyAbstractOperandNode
    {
        public AssemblyRegisterOperandNode(AssemblyRegister register)
        {
            Register = register;
        }


        public AssemblyRegister Register { get; set; }
    }
}
