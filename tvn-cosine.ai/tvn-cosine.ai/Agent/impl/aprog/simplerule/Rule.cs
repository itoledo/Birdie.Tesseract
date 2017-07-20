using System.Text;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * A simple implementation of a "condition-action rule".
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class Rule : IEquatable, IHashable, IToString
    {
        private Condition con;
        private IAction action;

        /**
         * Constructs a condition-action rule.
         * 
         * @param con
         *            a condition
         * @param action
         *            an action
         */
        public Rule(Condition con, IAction action)
        {
            if (null == con ||
                null == action)
            {
                throw new ArgumentNullException("con, action cannot be null");
            }

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
        public IAction getAction()
        {
            return action;
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is Rule))
            {
                return false;
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

            sb.Append("if ")
              .Append(con)
              .Append(" then ")
              .Append(action)
              .Append(".");

            return sb.ToString();
        }
    }
}
