using tvn.cosine.ai.common;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.common.text;
using tvn.cosine.ai.common.text.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.agentprogram.simplerule
{
    /// <summary>
    /// Implementation of an OR condition.
    /// </summary>
    public class ORCondition : Condition
    {
        private Condition left;
        private Condition right;

        public ORCondition(Condition leftCon, Condition rightCon)
        {
            if (null == leftCon ||
                null == rightCon)
            {
                throw new ArgumentNullException("leftCon, rightCon cannot be null");
            }

            left = leftCon;
            right = rightCon;
        }

        public override bool evaluate(ObjectWithDynamicAttributes p)
        {
            return (left.evaluate(p) || right.evaluate(p));
        }

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("[")
              .Append(left)
              .Append(" || ")
              .Append(right)
              .Append("]");

            return sb.ToString();
        }
    }
}
