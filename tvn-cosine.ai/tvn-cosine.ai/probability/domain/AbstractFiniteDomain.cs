using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.probability.domain
{
    public abstract class AbstractFiniteDomain<T> : FiniteDomain<T>
    {
        private string toString = null;
        private IDictionary<T, int> valueToIdx = new Dictionary<T, int>();
        private IDictionary<int, T> idxToValue = new Dictionary<int, T>();

        public AbstractFiniteDomain()
        { }

        //
        // START-Domain 
        public bool isFinite()
        {
            return true;
        }

        public bool isInfinite()
        {
            return false;
        }

        public abstract int size();

        public abstract bool isOrdered();

        // END-Domain
        //

        //
        // START-FiniteDomain 
        public abstract ISet<T> getPossibleValues();

        public int getOffset(T value)
        {
            if (valueToIdx.ContainsKey(value))
            {
                throw new Exception("Value [" + value + "] is not a possible value of this domain.");
            }
            return valueToIdx[value];
        }

        public T getValueAt(int offset)
        {
            return idxToValue[offset];
        }

        // END-FiniteDomain
        //

        public override string ToString()
        {
            if (null == toString)
            {
                toString = getPossibleValues().ToString();
            }
            return toString;
        }

        //
        // PROTECTED METHODS
        //
        protected void indexPossibleValues(ISet<T> possibleValues)
        {
            int idx = 0;
            foreach (T value in possibleValues)
            {
                valueToIdx.Add(value, idx);
                idxToValue.Add(idx, value);
                idx++;
            }
        }
    }
}
