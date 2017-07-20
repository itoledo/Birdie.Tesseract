using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.probability.domain
{
    public class FiniteIntegerDomain : AbstractFiniteDomain
    {
        private ISet<int> possibleValues;

        public FiniteIntegerDomain(params int[] pValues)
        {
            // Keep consistent order
            possibleValues = Factory.CreateSet<int>();
            foreach (int v in pValues)
            {
                possibleValues.Add(v);
            }
            // Ensure cannot be modified
            possibleValues = Factory.CreateReadOnlySet<int>(possibleValues);

            indexPossibleValues(possibleValues);
        }

        //
        // START-Domain

        public override int size()
        {
            return possibleValues.Size();
        }


        public override bool isOrdered()
        {
            return true;
        }

        // END-Domain
        //

        //
        // START-DiscreteDomain

        public override ISet<object> getPossibleValues()
        {
            ISet<object> obj = Factory.CreateSet<object>();
            foreach (int i in possibleValues)
            {
                obj.Add(i);
            }
            return obj;
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
