using System;
using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.probability.proposition
{
    public class ConjunctiveProposition<T> : AbstractProposition<T>, BinarySentenceProposition<T>
    {
        private Proposition<T> left = null;
        private Proposition<T> right = null;
        //
        private string toString = null;

        public ConjunctiveProposition(Proposition<T> left, Proposition<T> right)
        {
            if (null == left)
            {
                throw new ArgumentException("Left side of conjunction must be specified.");
            }
            if (null == right)
            {
                throw new ArgumentException("Right side of conjunction must be specified.");
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
            return left.holds(possibleWorld) && right.holds(possibleWorld);
        }

        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(left.ToString());
                sb.Append(" AND ");
                sb.Append(right.ToString());
                sb.Append(")");

                toString = sb.ToString();
            }

            return toString;
        }
    } 
}
