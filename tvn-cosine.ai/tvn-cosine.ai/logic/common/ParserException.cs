﻿using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.logic.common
{
    /**
     * A runtime exception to be used to describe Parser exceptions. In particular
     * it provides information to help in identifying which tokens proved
     * problematic in the parse.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ParserException : RuntimeException
    {
        private IQueue<Token> problematicTokens = Factory.CreateQueue<Token>();

        public ParserException(string message, params Token[] problematicTokens)
            : base(message)
        {

            if (problematicTokens != null)
            {
                foreach (Token pt in problematicTokens)
                {
                    this.problematicTokens.Add(pt);
                }
            }
        }

        public ParserException(string message, Exception cause, params Token[] problematicTokens)
            : base(message, cause)
        {

            if (problematicTokens != null)
            {
                foreach (Token pt in problematicTokens)
                {
                    this.problematicTokens.Add(pt);
                }
            }
        }

        /**
         * 
         * @return a list of 0 or more tokens from the input stream that are
         *         believed to have contributed to the parse exception.
         */
        public IQueue<Token> getProblematicTokens()
        {
            return problematicTokens;
        }
    }
}
