using System;

namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// Implementation of a NOT condition.
    /// </summary>
    /// <typeparam name="INPUT"></typeparam>
    public class NotCondition<INPUT> : IEquatable<NotCondition<INPUT>>
    {
        private readonly ICondition<INPUT> condition;

        public NotCondition(ICondition<INPUT> condition)
        {
            if (null == condition) throw new ArgumentNullException("Condition cannot be null.");

            this.condition = condition;
        }

        public bool Evaluate(INPUT input)
        {
            return !condition.Evaluate(input);
        }

        #region ICondition<INPUT> Support
        public override string ToString()
        {
            return string.Format("!{{{0}}}", condition);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NotCondition<INPUT>);
        }

        public bool Equals(NotCondition<INPUT> other)
        {
            if (null == other) return false;

            return condition.Equals(other.condition);
        }
        #endregion
    }

    /// <summary>
    /// Implementation of a NOT condition.
    /// </summary>
    public class NotCondition : NotCondition<IState>, ICondition
    {
        public NotCondition(ICondition<IState> condition)
            : base(condition)
        { }
    }
}
