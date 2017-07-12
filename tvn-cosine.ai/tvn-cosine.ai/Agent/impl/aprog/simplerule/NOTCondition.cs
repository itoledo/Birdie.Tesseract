using System; 
using System.Text;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /// <summary>
    /// Implementation of a NOT condition.
    /// </summary>
    public class NOTCondition : Condition
    {
        private readonly Condition condition;

        public NOTCondition(Condition condition)
        {
            if (null == condition)
            {
                throw new ArgumentNullException("condition cannot be null.");
            }

            this.condition = condition;
        }

        public override bool Evaluate(ObjectWithDynamicAttributes<string, object> p)
        {
            return (!condition.Evaluate(p));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("![")
                     .Append(condition)
                     .Append("]")
                     .ToString();
        }
    }
}
