// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace SwishCC.Exceptions
{
    public class DumpException : Exception
    {
        public DumpException(string message)
            : base(message)
        {
        }
    }
}
