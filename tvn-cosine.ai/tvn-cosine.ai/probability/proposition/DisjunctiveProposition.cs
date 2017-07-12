using System;
using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.probability.proposition
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Formula 13.4, page
     * 489. 
     *  
     * 
     * We can also derive the well-known formula for the probability of a
     * disjunction, sometimes called the <b>inclusion-exclusion principle:</b> 
     *  
     * P(a OR b) = P(a) + P(b) - P(a AND b). 
     * 
     * @author Ciaran O'Reilly
     */
    public class DisjunctiveProposition<T> : AbstractProposition<T>, BinarySentenceProposition<T>
    {
        private Proposition<T> left = null;
        private Proposition<T> right = null;
        //
        private string toString = null;

        public DisjunctiveProposition(Proposition<T> left, Proposition<T> right)
        {
            if (null == left)
            {
                throw new ArgumentException("Left side of disjunction must be specified.");
            }
            if (null == right)
            {
                throw new ArgumentException("Right side of disjunction must be specified.");
            }
            // Track nested scope
            addScope(left.getScope());
            addScope(right.getScope());
            addUnboundScope(left.getUnboundScope());
            addUnboundScope(right.getUnboundScope());

            this.left = left;
            this.right = right;
        }

        public override bool holds(IDictionary<RandomVariable, T> possibleWorld)
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
