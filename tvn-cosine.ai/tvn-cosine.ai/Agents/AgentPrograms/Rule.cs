using System;
using System.Text;

namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// A simple implementation of a "condition-action rule".
    /// </summary>
    public class Rule<INPUT, RESULT> : IRule<INPUT, RESULT>, IEquatable<Rule<INPUT, RESULT>>
    {
        public Rule(ICondition<INPUT> condition, RESULT action)
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
            this.Result = action;
        }

        public RESULT Result { get; }
        public ICondition<INPUT> Condition { get; }

        public bool Equals(Rule<INPUT, RESULT> other)
        {
            if (null == other)
            {
                return false;
            }

            return other.Condition.Equals(Condition)
                && other.Result.Equals(Result);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rule<INPUT, RESULT>);
        }

        public bool Evaluate(INPUT state)
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
            stringBuilder.Append(Result);
            stringBuilder.Append(".");

            return stringBuilder.ToString();
        }
    }

    public class Rule : Rule<IState, IAction>
    {
        public Rule(ICondition<IState> condition, IAction action) 
            : base(condition, action)
        { }
    }
}
