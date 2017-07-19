using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.proposition
{
    public class AssignmentProposition : AbstractTermProposition
    {
        private object value = null;
        //
        private string toString = null;

        public AssignmentProposition(RandomVariable forVariable, object value)
            : base(forVariable)
        {
            setValue(value);
        }

        public object getValue()
        {
            return value;
        }

        public void setValue(object value)
        {
            if (null == value)
            {
                throw new IllegalArgumentException("The value for the Random Variable must be specified.");
            }
            this.value = value;
        }
         
        public override bool holds(IMap<RandomVariable, object> possibleWorld)
        {
            return value.Equals(possibleWorld.Get(getTermVariable()));
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
