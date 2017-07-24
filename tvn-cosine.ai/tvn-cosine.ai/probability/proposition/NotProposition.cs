using System.Text;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.api;

namespace tvn.cosine.ai.probability.proposition
{
    public class NotProposition : AbstractProposition, UnarySentenceProposition
    {
        private Proposition proposition;
        //
        private string toString = null;

        public NotProposition(Proposition prop)
        {
            if (null == prop)
            {
                throw new IllegalArgumentException("Proposition to be negated must be specified.");
            }
            // Track nested scope
            addScope(prop.getScope());
            addUnboundScope(prop.getUnboundScope());

            proposition = prop;
        }


        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
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
