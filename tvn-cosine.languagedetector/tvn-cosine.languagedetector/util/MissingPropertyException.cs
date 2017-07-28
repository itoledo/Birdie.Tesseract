using System; 

namespace tvn_cosine.languagedetector.util
{
    public class MissingPropertyException : Exception
    {
        public MissingPropertyException()
            : base()
        { }
       

        public MissingPropertyException(string message)
            : base(message)
        { }


        public MissingPropertyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
