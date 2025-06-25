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


        public static TackyBinaryOperator ToTacky(this CBinaryOperator binaryOperator)
        {
            return binaryOperator switch
            {
                CBinaryOperator.Add => TackyBinaryOperator.Add,
                CBinaryOperator.Subtract => TackyBinaryOperator.Subtract,
                CBinaryOperator.Multiply => TackyBinaryOperator.Multiply,
                CBinaryOperator.Divide => TackyBinaryOperator.Divide,
                CBinaryOperator.Remainder => TackyBinaryOperator.Remainder,
                _ => throw new ArgumentOutOfRangeException(nameof(binaryOperator), binaryOperator, null)
            };
        }
    }
}
