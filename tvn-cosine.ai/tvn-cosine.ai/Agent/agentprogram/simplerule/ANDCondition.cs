using System.Text;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.agentprogram.simplerule
{
    /// <summary>
    /// Implementation of an AND condition.
    /// </summary>
    public class ANDCondition : Condition
    {
        private Condition left;
        private Condition right;

        public ANDCondition(Condition leftCon, Condition rightCon)
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
            return (left.evaluate(p) && right.evaluate(p));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[")
              .Append(left.ToString())
              .Append(" && ")
              .Append(right.ToString())
              .Append("]");

            return sb.ToString();
        }
    }
}
