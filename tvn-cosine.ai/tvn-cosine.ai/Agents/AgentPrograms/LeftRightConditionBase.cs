using System;

namespace tvn_cosine.ai.Agents.AgentPrograms
{
    public abstract class LeftRightConditionBase<INPUT> : ICondition<INPUT>, IEquatable<LeftRightConditionBase<INPUT>>
    {
        protected readonly ICondition<INPUT> leftCondition;
        protected readonly ICondition<INPUT> rightCondition;

        public LeftRightConditionBase(ICondition<INPUT> leftCondition, ICondition<INPUT> rightCondition)
        {
            if (null == leftCondition) throw new ArgumentNullException("Left condition cannot be null.");
            if (null == rightCondition) throw new ArgumentNullException("Right condition cannot be null.");

            this.leftCondition = leftCondition;
            this.rightCondition = rightCondition;
        }

        public abstract bool Evaluate(INPUT input);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LeftRightConditionBase<INPUT>);
        }

        public bool Equals(LeftRightConditionBase<INPUT> other)
        {
            if (null == other) return false;

            return leftCondition.Equals(other.leftCondition)
                && rightCondition.Equals(other.rightCondition);
        }
    }

    public abstract class LeftRightConditionBase : LeftRightConditionBase<IState>, ICondition
    {
        public LeftRightConditionBase(ICondition<IState> leftCondition, ICondition<IState> rightCondition) 
            : base(leftCondition, rightCondition)
        { }
    }
}
