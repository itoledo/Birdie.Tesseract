using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.proposition
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Formula 13.4, page
     * 489.<br>
     * <br>
     * 
     * We can also derive the well-known formula for the probability of a
     * disjunction, sometimes called the <b>inclusion-exclusion principle:</b><br>
     * <br>
     * P(a OR b) = P(a) + P(b) - P(a AND b).<br>
     * 
     * @author Ciaran O'Reilly
     */
    public class DisjunctiveProposition : AbstractProposition, BinarySentenceProposition
    {
        private Proposition left = null;
        private Proposition right = null;
        //
        private string toString = null;

        public DisjunctiveProposition(Proposition left, Proposition right)
        {
            if (null == left)
            {
                throw new IllegalArgumentException("Left side of disjunction must be specified.");
            }
            if (null == right)
            {
                throw new IllegalArgumentException("Right side of disjunction must be specified.");
            }
            // Track nested scope
            addScope(left.getScope());
            addScope(right.getScope());
            addUnboundScope(left.getUnboundScope());
            addUnboundScope(right.getUnboundScope());

            this.left = left;
            this.right = right;
        }


        public override bool holds(IMap<RandomVariable, object> possibleWorld)
        {
            return left.holds(possibleWorld) || right.holds(possibleWorld);
        }


        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(left.ToString());
                sb.Append(" OR ");
                sb.Append(right.ToString());
                sb.Append(")");

                toString = sb.ToString();
            }

            return toString;
        }
    }
}
