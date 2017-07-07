using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tvn.cosine.ai.probability.proposition
{
    public class EquivalentProposition<T> : AbstractDerivedProposition<T>
    {
        //
        private string toString = null;

        public EquivalentProposition(string name, IEnumerable<RandomVariable> equivs)
            : base(name)
        {

            if (null == equivs || 1 >= equivs.Count())
            {
                throw new ArgumentException("Equivalent variables must be specified.");
            }
            foreach (RandomVariable rv in equivs)
            {
                addScope(rv);
            }
        }

        //
        // START-Proposition
        public override bool holds(IDictionary<RandomVariable, T> possibleWorld)
        {
            bool holds = true;

            RandomVariable rvL = getScope().First();

            foreach (var rvC in getScope().Skip(1))
            { 
                if (!possibleWorld[rvL].Equals(possibleWorld[rvC]))
                {
                    holds = false;
                    break;
                }
                rvL = rvC;
            }

            return holds;
        }

        // END-Proposition
        //

        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(getDerivedName());
                foreach (RandomVariable rv in getScope())
                {
                    sb.Append(" = ");
                    sb.Append(rv);
                }
                toString = sb.ToString();
            }
            return toString;
        }
    }
}
