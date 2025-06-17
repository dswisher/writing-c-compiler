// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace SwishCC.Tackying
{
    public class TackyException : Exception
    {
        public TackyException(string message)
            : base(message)
        {
        }
    }
}
