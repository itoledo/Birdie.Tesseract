using System.Text;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * Implementation of an AND condition.
     * 
     * @author Ciaran O'Reilly
     * 
     */
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

            return sb.Append("[")
                     .Append(left)
                     .Append(" && ")
                     .Append(right)
                     .Append("]").ToString();
        }
    }
}
