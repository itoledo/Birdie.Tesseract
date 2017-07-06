using System.Diagnostics;
using System.Text;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * A simple implementation of a "condition-action rule".
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class Rule
    {
        private Condition con;
        private Action action;

        /**
         * Constructs a condition-action rule.
         * 
         * @param con
         *            a condition
         * @param action
         *            an action
         */
        public Rule(Condition con, Action action)
        {
            Debug.Assert(null != con);
            Debug.Assert(null != action);

            this.con = con;
            this.action = action;
        }

        public bool evaluate(ObjectWithDynamicAttributes p)
        {
            return (con.evaluate(p));
        }

        /**
         * Returns the action of this condition-action rule.
         * 
         * @return the action of this condition-action rule.
         */
        public Action getAction()
        {
            return action;
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is Rule))
            {
                return base.Equals(o);
            }
            return (ToString().Equals(((Rule)o).ToString()));
        }
         
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
         
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("if ").Append(con).Append(" then ").Append(action)
                     .Append(".").ToString();
        }
    }

}
