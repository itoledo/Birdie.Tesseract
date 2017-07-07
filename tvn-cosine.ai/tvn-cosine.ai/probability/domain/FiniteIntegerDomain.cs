using System.Collections.Generic;

namespace tvn.cosine.ai.probability.domain
{
    public class FiniteIntegerDomain : AbstractFiniteDomain<int>
    {
        private ISet<int> possibleValues = null;

        public FiniteIntegerDomain(IEnumerable<int> pValues)
        {
            // Keep consistent order
            possibleValues = new HashSet<int>();
            foreach (int v in pValues)
            {
                possibleValues.Add(v);
            }
            //TODO: Ensure cannot be modified
            // new IReadOnlyCollection(possibleValues);

            indexPossibleValues(possibleValues);
        }

        //
        // START-Domain 
        public override int size()
        {
            return possibleValues.Count;
        }

        public override bool isOrdered()
        {
            return true;
        }

        // END-Domain
        //

        //
        // START-DiscreteDomain 
        public override ISet<int> getPossibleValues()
        {
            return possibleValues;
        }

        // END-DiscreteDomain
        //

        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if (!(o is FiniteIntegerDomain))
            {
                return false;
            }

            FiniteIntegerDomain other = (FiniteIntegerDomain)o;

            return this.possibleValues.Equals(other.possibleValues);
        }

        public override int GetHashCode()
        {
            return possibleValues.GetHashCode();
        }
    } 
}
