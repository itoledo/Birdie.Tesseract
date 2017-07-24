using System.Text;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.agentprogram.simplerule
{
    /// <summary>
    /// Implementation of an EQUALity condition.
    /// </summary>
    public class EQUALCondition : Condition
    {
        private object key;
        private object value;

        public EQUALCondition(object key, object value)
        {
            if (null == key ||
                null == value)
            {
                throw new ArgumentNullException("key, value cannot be null");
            }

            this.key = key;
            this.value = value;
        }

        public override bool evaluate(ObjectWithDynamicAttributes p)
        {
            return value.Equals(p.getAttribute(key));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(key)
              .Append("==")
              .Append(value);

            return sb.ToString();
        }
    }
}
