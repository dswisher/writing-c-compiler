// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace SwishCC.Lexing
{
    public class LexException : Exception
    {
        public LexException(string message)
            : base(message)
        {
        }
    }
}
