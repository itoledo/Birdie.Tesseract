using System;
using System.Text;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /// <summary>
    /// Implementation of an AND condition.
    /// </summary>
    public class ANDCondition : Condition
    {
        private readonly Condition left;
        private readonly Condition right;

        public ANDCondition(Condition leftCondition, Condition rightCondition)
        {
            if (null == leftCondition
             || null == rightCondition)
            {
                throw new ArgumentNullException("leftCondition, rightCondition cannot be null.");
            }

            left = leftCondition;
            right = rightCondition;
        }

        public override bool Evaluate(ObjectWithDynamicAttributes<string, object> p)
        {
            return (left.Evaluate(p)
                 && right.Evaluate(p));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("[")
                     .Append(left)
                     .Append(" && ")
                     .Append(right)
                     .Append("]")
                     .ToString();
        }
    } 
}
