using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.probability.domain
{
    public class FiniteIntegerDomain : AbstractFiniteDomain
    {
        private ISet<int> possibleValues;

        public FiniteIntegerDomain(params int[] pValues)
        {
            // Keep consistent order
            possibleValues = CollectionFactory.CreateSet<int>();
            foreach (int v in pValues)
            {
                possibleValues.Add(v);
            }
            // Ensure cannot be modified
            possibleValues = CollectionFactory.CreateReadOnlySet<int>(possibleValues);

            indexPossibleValues(possibleValues);
        }
         
        public override int Size()
        {
            return possibleValues.Size();
        }


        public override bool IsOrdered()
        {
            return true;
        } 

        public override ISet<object> GetPossibleValues()
        {
            ISet<object> obj = CollectionFactory.CreateSet<object>();
            foreach (int i in possibleValues)
            {
                obj.Add(i);
            }
            return obj;
        }
         
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
