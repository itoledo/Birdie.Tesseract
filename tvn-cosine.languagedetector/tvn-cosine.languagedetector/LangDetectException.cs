using System;

namespace tvn_cosine.languagedetector
{
    public class LangDetectException : Exception
    { 
        private readonly ErrorCode code;
          
        public LangDetectException(ErrorCode code, string message)
            : base(message)
        { 
            this.code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>the error code</returns>
        public ErrorCode getCode()
        {
            return code;
        }
    }
}
