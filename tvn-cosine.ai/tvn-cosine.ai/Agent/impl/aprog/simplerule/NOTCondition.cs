using System.Diagnostics;
using System.Text;

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
            Debug.Assert(null != con);

            this.con = con;
        }

        public override bool evaluate(ObjectWithDynamicAttributes<object, object> p)
        {
            return (!con.evaluate(p));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("![").Append(con).Append("]").ToString();
        }
    }
}
