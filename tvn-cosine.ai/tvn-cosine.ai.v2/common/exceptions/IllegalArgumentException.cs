using System;

namespace tvn_cosine.ai.v2.common.exceptions
{
    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException() : base() { }
        public IllegalArgumentException(string message) : base(message) { }
        public IllegalArgumentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
