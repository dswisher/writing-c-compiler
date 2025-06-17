// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using SwishCC.Models.CTree;
using SwishCC.Models.TackyTree;

namespace SwishCC.Tackying
{
    public static class TackyHelpers
    {
        public static TackyUnaryOperator ToTacky(this CUnaryOperator unaryOperator)
        {
            return unaryOperator switch
            {
                CUnaryOperator.Complement => TackyUnaryOperator.Complement,
                CUnaryOperator.Negation => TackyUnaryOperator.Negation,
                _ => throw new ArgumentOutOfRangeException(nameof(unaryOperator), unaryOperator, null)
            };
        }
    }
}
