using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.domain.api;

namespace tvn.cosine.ai.probability.domain
{
    public abstract class AbstractFiniteDomain : IFiniteDomain
    {
        private string toString = null;
        private IMap<object, int> valueToIdx = CollectionFactory.CreateInsertionOrderedMap<object, int>();
        private IMap<int, object> idxToValue = CollectionFactory.CreateInsertionOrderedMap<int, object>();

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
            if (!valueToIdx.ContainsKey(value))
            {
                throw new IllegalArgumentException("Value [" + value + "] is not a possible value of this domain.");
            }
            return valueToIdx.Get(value);
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

        protected virtual void indexPossibleValues<T>(ISet<T> possibleValues)
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
