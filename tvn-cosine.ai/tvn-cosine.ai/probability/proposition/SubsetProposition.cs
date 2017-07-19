using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.domain;

namespace tvn.cosine.ai.probability.proposition
{
    public class SubsetProposition : AbstractDerivedProposition
    {
        private FiniteDomain subsetDomain = null;
        private RandomVariable varSubsetOf = null;
        //
        private string toString = null;

        public SubsetProposition(string name, FiniteDomain subsetDomain, RandomVariable ofVar)
            : base(name)
        {


            if (null == subsetDomain)
            {
                throw new IllegalArgumentException("Sum Domain must be specified.");
            }
            this.subsetDomain = subsetDomain;
            this.varSubsetOf = ofVar;
            addScope(this.varSubsetOf);
        }
         
        public override bool holds(IMap<RandomVariable, object> possibleWorld)
        {
            return subsetDomain.getPossibleValues().Contains(possibleWorld.Get(varSubsetOf));
        }

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
