﻿namespace tvn.cosine.ai.common.exceptions
{
    public class ArrayIndexOutOfBoundsException : Exception
    {
        public ArrayIndexOutOfBoundsException()
            : this("")
        { }

        public ArrayIndexOutOfBoundsException(string message)
            : base(message)
        { }

        public ArrayIndexOutOfBoundsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}