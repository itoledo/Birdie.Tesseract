using System;
using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.probability.proposition
{
    public class AssignmentProposition<T> : AbstractTermProposition<T>
    {
        private T value;
        //
        private string toString = null;

        public AssignmentProposition(RandomVariable forVariable, T value)
            : base(forVariable)
        {
            setValue(value);
        }

        public T getValue()
        {
            return value;
        }

        public void setValue(T value)
        {
            if (null == value)
            {
                throw new ArgumentException("The value for the Random Variable must be specified.");
            }
            this.value = value;
        }

        public override bool holds(IDictionary<RandomVariable, T> possibleWorld)
        {
            return value.Equals(possibleWorld[getTermVariable()]);
        }

        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(getTermVariable().getName());
                sb.Append(" = ");
                sb.Append(value);

                toString = sb.ToString();
            }
            return toString;
        }
    }

}
