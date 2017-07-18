﻿namespace tvn.cosine.ai.common.exceptions
{
    public class NotSupportedException : Exception
    {
        public NotSupportedException()
            : this("")
        { }

        public NotSupportedException(string message)
            : base(message)
        { }

        public NotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}