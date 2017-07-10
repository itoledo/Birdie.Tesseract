using System.Collections.Generic;

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
    public class ArbitraryTokenDomain<T> : AbstractFiniteDomain<T>
    {
        private ISet<T> possibleValues = null;
        private bool ordered = false;

        public ArbitraryTokenDomain(params T[] pValues)
            : this(false, pValues)
        { }

        public ArbitraryTokenDomain(bool ordered, params T[] pValues)
        {
            this.ordered = ordered;
            // Keep consistent order
            possibleValues = new HashSet<T>();
            foreach (T v in pValues)
            {
                possibleValues.Add(v);
            }
            // TODO: Ensure cannot be modified
            //   possibleValues = ReadonlyCollection( possibleValues);

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
            return ordered;
        }

        // END-Domain
        //

        //
        // START-FiniteDomain 
        public override ISet<T> getPossibleValues()
        {
            return possibleValues;
        }

        // END-finiteDomain
        //

        public override bool Equals(object obj)
        {

            if (this == obj)
            {
                return true;
            }
            if (!(obj is ArbitraryTokenDomain<T>))
            {
                return false;
            }

            ArbitraryTokenDomain<T> other = (ArbitraryTokenDomain<T>)obj;

            return this.possibleValues.Equals(other.possibleValues);
        }

        public override int GetHashCode()
        {
            return possibleValues.GetHashCode();
        }
    }
}
