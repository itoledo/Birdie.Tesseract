using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.domain.api;

namespace tvn.cosine.ai.probability.domain
{
    public abstract class AbstractFiniteDomain<T> : IFiniteDomain
    {
        private string toString = null;
        private IMap<T, int> valueToIdx = CollectionFactory.CreateInsertionOrderedMap<T, int>();
        private IMap<int, T> idxToValue = CollectionFactory.CreateInsertionOrderedMap<int, T>();

        public AbstractFiniteDomain()
        { }

        public virtual bool IsFinite()
        {
            return true;
        }
         
        public virtual bool IsInfinite()
        {
            return false;
        }
         
        public abstract int Size(); 
        public abstract bool IsOrdered(); 
        public abstract ISet<object> GetPossibleValues();
         
        public virtual int GetOffset(object value)
        {
            if (!valueToIdx.ContainsKey((T)value))
            {
                throw new IllegalArgumentException("Value [" + value + "] is not a possible value of this domain.");
            }
            return valueToIdx.Get((T)value);
        }
         
        public virtual object GetValueAt(int offset)
        {
            return idxToValue.Get(offset);
        }

        public override string ToString()
        {
            if (null == toString)
            {
                toString = GetPossibleValues().ToString();
            }
            return toString;
        }

        protected virtual void indexPossibleValues(ISet<T> possibleValues)
        {
            int idx = 0;
            foreach (T value in possibleValues)
            {
                valueToIdx.Put(value, idx);
                idxToValue.Put(idx, value);
                idx++;
            }
        }
    }
}
