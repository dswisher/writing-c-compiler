// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace SwishCC.Exceptions
{
    public class LexerException : Exception
    {
        public LexerException(string message)
            : base(message)
        {
        }
    }
}
