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
