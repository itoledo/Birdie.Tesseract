using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.probability.domain
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 486.
     * 
     * As in CSPs, domains can be sets of arbitrary tokens; we might choose the
     * domain of <i>Age</i> to be {<i>juvenile,teen,adult</i>} and the domain of
     * <i>Weather</i> might be {<i>sunny,rain,cloudy,snow</i>}.
     * 
     * @author Ciaran O'Reilly
     */
    public class ArbitraryTokenDomain : AbstractFiniteDomain
    {
        private ISet<object> possibleValues = null;
        private bool ordered = false;

        public ArbitraryTokenDomain(params object[] pValues)
                : this(false, pValues)
        { }

        public ArbitraryTokenDomain(bool ordered, params object[] pValues)
        {
            this.ordered = ordered;
            // Keep consistent order
            possibleValues = CollectionFactory.CreateSet<object>();
            foreach (object v in pValues)
            {
                possibleValues.Add(v);
            }
            // Ensure cannot be modified
            possibleValues = CollectionFactory.CreateReadOnlySet<object>(possibleValues);

            indexPossibleValues(possibleValues);
        }
        
        public override int size()
        {
            return possibleValues.Size();
        }


        public override bool isOrdered()
        {
            return ordered;
        }
         
        public override ISet<object> getPossibleValues()
        {
            return possibleValues;
        }
        
        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if (!(o is ArbitraryTokenDomain))
            {
                return false;
            }

            ArbitraryTokenDomain other = (ArbitraryTokenDomain)o;

            return this.possibleValues.Equals(other.possibleValues);
        } 

        public override int GetHashCode()
        {
            return possibleValues.GetHashCode();
        }
    } 
}
