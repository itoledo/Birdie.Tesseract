namespace tvn.cosine.ai.common.exceptions
{
    public class RuntimeException : Exception
    {
        public RuntimeException()
            : this(string.Empty)
        { }

        public RuntimeException(string message)
            : base(message)
        { }

        public RuntimeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
