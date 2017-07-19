namespace tvn.cosine.ai.common.exceptions
{
    public class NumberFormatException : Exception
    {
        public NumberFormatException()
            : this("")
        { }

        public NumberFormatException(string message)
            : base(message)
        { }

        public NumberFormatException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
