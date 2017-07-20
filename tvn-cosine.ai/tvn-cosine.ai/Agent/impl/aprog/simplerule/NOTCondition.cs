using System.Text;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * Implementation of a NOT condition.
     * 
     * @author Ciaran O'Reilly
     * 
     */
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
            StringBuilder sb = new StringBuilder();

            sb.Append("![")
              .Append(con)
              .Append("]");

            return sb.ToString();
        }
    }
}
