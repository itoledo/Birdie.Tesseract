﻿using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.proposition
{
    public class ConjunctiveProposition : AbstractProposition, BinarySentenceProposition
    {
        private Proposition left = null;
        private Proposition right = null;
        //
        private string toString = null;

        public ConjunctiveProposition(Proposition left, Proposition right)
        {
            if (null == left)
            {
                throw new IllegalArgumentException("Left side of conjunction must be specified.");
            }
            if (null == right)
            {
                throw new IllegalArgumentException("Right side of conjunction must be specified.");
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
