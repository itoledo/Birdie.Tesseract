using System;
using System.Collections.Generic;
using System.Text;
using tvn.cosine.ai.probability.domain;

namespace tvn.cosine.ai.probability.proposition
{
    public class SubsetProposition<T> : AbstractDerivedProposition<T>
    {
        private FiniteDomain<T> subsetDomain = null;
        private RandomVariable varSubsetOf = null;
        //
        private string toString = null;

        public SubsetProposition(string name, FiniteDomain<T> subsetDomain, RandomVariable ofVar)
            : base(name)
        {
            if (null == subsetDomain)
            {
                throw new ArgumentException("Sum Domain must be specified.");
            }
            this.subsetDomain = subsetDomain;
            this.varSubsetOf = ofVar;
            addScope(this.varSubsetOf);
        }

        //
        // START-Proposition
        public override bool holds(IDictionary<RandomVariable, T> possibleWorld)
        {
            return subsetDomain.getPossibleValues().Contains(possibleWorld[varSubsetOf]);
        }

        // END-Proposition
        //

        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(getDerivedName());
                sb.Append(" = ");
                sb.Append(subsetDomain.ToString());
                toString = sb.ToString();
            }
            return toString;
        }
    } 
}
