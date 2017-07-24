using System.Text;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.api;
using tvn.cosine.ai.probability.domain;

namespace tvn.cosine.ai.probability.proposition
{
    public class SubsetProposition : AbstractDerivedProposition
    {
        private FiniteDomain subsetDomain = null;
        private IRandomVariable varSubsetOf = null;
        //
        private string toString = null;

        public SubsetProposition(string name, FiniteDomain subsetDomain, IRandomVariable ofVar)
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
         
        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
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
