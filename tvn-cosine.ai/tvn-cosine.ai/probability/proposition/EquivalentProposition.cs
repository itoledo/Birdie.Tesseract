using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.proposition
{
    public class EquivalentProposition : AbstractDerivedProposition
    {
        private string toString = null;

        public EquivalentProposition(string name, params RandomVariable[] equivs)
                : base(name)
        {
            if (null == equivs || 1 >= equivs.Length)
            {
                throw new IllegalArgumentException("Equivalent variables must be specified.");
            }
            foreach (RandomVariable rv in equivs)
            {
                addScope(rv);
            }
        }

        public override bool holds(IMap<RandomVariable, object> possibleWorld)
        {
            bool holds = true;
            bool first = true;

            RandomVariable rvL = null;
            foreach (RandomVariable rvC in getScope())
            {
                if (first)
                {
                    rvL = rvC;
                    first = false;
                    continue;
                }
                if (!possibleWorld.Get(rvL).Equals(possibleWorld.Get(rvC)))
                {
                    holds = false;
                    break;
                }
                rvL = rvC;
            }

            return holds;
        }

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
