namespace tvn.cosine.ai.common.exceptions
{
    public class IOException : Exception
    {
        public IOException()
            : this("")
        { }

        public IOException(string message)
            : base(message)
        { }

        public IOException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
