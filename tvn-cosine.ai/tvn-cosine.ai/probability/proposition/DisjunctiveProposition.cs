using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.common.text;
using tvn.cosine.ai.common.text.api;
using tvn.cosine.ai.probability.api;
using tvn.cosine.ai.probability.proposition.api;

namespace tvn.cosine.ai.probability.proposition
{
    /// <summary>   
    /// Artificial Intelligence A Modern Approach (3rd Edition): Formula 13.4, page 489. 
    /// <para />
    /// We can also derive the well-known formula for the probability of a
    /// disjunction, sometimes called the inclusion-exclusion principle: 
    /// <para />
    /// P(a OR b) = P(a) + P(b) - P(a AND b). 
    /// </summary>
    public class DisjunctiveProposition : AbstractProposition, IBinarySentenceProposition
    {
        private IProposition left = null;
        private IProposition right = null;
        //
        private string toString = null;

        public DisjunctiveProposition(IProposition left, IProposition right)
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
         
        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
        {
            return left.holds(possibleWorld) || right.holds(possibleWorld);
        }
         
        public override string ToString()
        {
            if (null == toString)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
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
