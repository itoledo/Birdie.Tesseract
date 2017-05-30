using System;
using System.Text;

namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// A simple implementation of a "condition-action rule".
    /// </summary>
    public class Rule : IRule, IEquatable<Rule>
    {
        public Rule(ICondition condition, IAction action)
        {
            if (null == condition)
            {
                throw new ArgumentNullException("The condition cannot be null.");
            }
            if (null == action)
            {
                throw new ArgumentNullException("The action cannot be null.");
            }

            this.Condition = condition;
            this.Action = action;
        }

        public IAction Action { get; }
        public ICondition Condition { get; }

        public bool Equals(Rule other)
        {
            if (null == other)
            {
                return false;
            }

            return other.Condition.Equals(Condition)
                && other.Action.Equals(Action);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rule);
        }

        public bool Evaluate(IState state)
        {
            return Condition.Evaluate(state);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("if ");
            stringBuilder.Append(Condition);
            stringBuilder.Append(" then ");
            stringBuilder.Append(Action);
            stringBuilder.Append(".");

            return stringBuilder.ToString();
        }
    }
}
