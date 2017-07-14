using System; 
using System.Text;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /// <summary>
    /// A simple implementation of a "condition-action rule".
    /// </summary>
    public class Rule
    {
        private Condition con;
        private Action action;
         
        /// <summary>
        /// Constructs a condition-action rule.
        /// </summary>
        /// <param name="condition">a condition</param>
        /// <param name="action">an action</param>
        public Rule(Condition condition, Action action)
        {
            if (null == condition
             || null == action)
            {
                throw new ArgumentNullException("condition, action cannot be null.");
            }

            this.con = condition;
            this.action = action;
        }

        public bool evaluate(ObjectWithDynamicAttributes<string, object> p)
        {
            return (con.Evaluate(p));
        }
         
        /// <summary>
        /// Returns the action of this condition-action rule.
        /// </summary>
        /// <returns>the action of this condition-action rule.</returns>
        public Action getAction()
        {
            return action;
        }

        public override bool Equals(object o)
        {
            if (o == null 
             || !(o is Rule))
            {
                return base.Equals(o);
            }
            return ToString().Equals(((Rule)o).ToString());
        }
         
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
         
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("if ")
                     .Append(con)
                     .Append(" then ")
                     .Append(action)
                     .Append(".")
                     .ToString();
        }
    } 
}
