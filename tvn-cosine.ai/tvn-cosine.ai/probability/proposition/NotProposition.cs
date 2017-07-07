using System;
using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.probability.proposition
{
    public class NotProposition<T> : AbstractProposition<T>, UnarySentenceProposition<T>
    {
        private Proposition<T> proposition;
        //
        private string toString = null;

        public NotProposition(Proposition<T> prop)
        {
            if (null == prop)
            {
                throw new ArgumentException("Proposition to be negated must be specified.");
            }
            // Track nested scope
            addScope(prop.getScope());
            addUnboundScope(prop.getUnboundScope());

            proposition = prop;
        }

        public override bool holds(IDictionary<RandomVariable, T> possibleWorld)
        {
            return !proposition.holds(possibleWorld);
        }

        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(NOT ");
                sb.Append(proposition.ToString());
                sb.Append(")");

                toString = sb.ToString();
            }
            return toString;
        }
    } 
}
