using tvn.cosine.ai.common;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.common.text;
using tvn.cosine.ai.common.text.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.agentprogram.simplerule
{
    /// <summary>
    /// Implementation of a NOT condition.
    /// </summary>
    public class NOTCondition : Condition
    {
        private Condition con;

        public NOTCondition(Condition con)
        {
            if (null == con)
            {
                throw new ArgumentNullException("con cannot be null");
            }

            this.con = con;
        }

        public override bool evaluate(ObjectWithDynamicAttributes p)
        {
            return (!con.evaluate(p));
        }

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("![")
              .Append(con)
              .Append("]");

            return sb.ToString();
        }
    }
}
