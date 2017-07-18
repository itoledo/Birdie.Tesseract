using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.domain
{
    public abstract class AbstractFiniteDomain : FiniteDomain
    {
        private string toString = null;
        private IMap<object, int> valueToIdx = Factory.CreateMap<object, int>();
        private IMap<int, object> idxToValue = Factory.CreateMap<int, object>();

        public AbstractFiniteDomain()
        {

        }

        //
        // START-Domain

        public virtual bool isFinite()
        {
            return true;
        }


        public virtual bool isInfinite()
        {
            return false;
        }


        public abstract int size();


        public abstract bool isOrdered();

        // END-Domain
        //

        //
        // START-FiniteDomain

        public abstract ISet<T> getPossibleValues<T>();


        public virtual int getOffset(object value)
        {
            if (!valueToIdx.ContainsKey(value))
            {
                throw new IllegalArgumentException("Value [" + value
                        + "] is not a possible value of this domain.");
            }
            return valueToIdx.Get(value);
        }


        public object getValueAt(int offset)
        {
            return idxToValue.Get(offset);
        }

        // END-FiniteDomain
        //


        public override string ToString()
        {
            if (null == toString)
            {
                toString = getPossibleValues<int>().ToString();
            }
            return toString;
        }

        //
        // PROTECTED METHODS
        //
        protected void indexPossibleValues<T>(ISet<T> possibleValues)
        {
            int idx = 0;
            foreach (object value in possibleValues)
            {
                valueToIdx.Put(value, idx);
                idxToValue.Put(idx, value);
                idx++;
            }
        }
    }

}
