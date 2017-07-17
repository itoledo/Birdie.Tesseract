namespace tvn.cosine.ai.common.exceptions
{
    public class NotImplementedException : Exception
    {
        public NotImplementedException(string message)
            : base(message)
        { }

        public NotImplementedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
