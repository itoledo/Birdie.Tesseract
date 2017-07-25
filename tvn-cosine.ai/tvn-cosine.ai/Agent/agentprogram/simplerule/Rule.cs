using tvn.cosine.ai.common;
using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.util;
using tvn.cosine.ai.common.text.api;
using tvn.cosine.ai.common.text;

namespace tvn.cosine.ai.agent.agentprogram.simplerule
{
    /// <summary>
    /// A simple implementation of a "condition-action rule".
    /// </summary>
    public class Rule : IEquatable, IHashable, IStringable
    {
        private Condition con;
        private IAction action;

        /// <summary>
        /// Constructs a condition-action rule.
        /// </summary>
        /// <param name="con">a condition</param>
        /// <param name="action">an action</param>
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

        /// <summary>
        /// Returns the action of this condition-action rule.
        /// </summary>
        /// <returns></returns>
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
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("if ")
              .Append(con)
              .Append(" then ")
              .Append(action)
              .Append(".");

            return sb.ToString();
        }
    }
}
