namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// Implementation of a AND condition.
    /// </summary>
    /// <typeparam name="INPUT"></typeparam>
    public class AndCondition<INPUT> : LeftRightConditionBase<INPUT>
    {
        public AndCondition(ICondition<INPUT> leftCondition, ICondition<INPUT> rightCondition)
            : base(leftCondition, rightCondition)
        { }

        public override bool Evaluate(INPUT input)
        {
            return leftCondition.Evaluate(input) && rightCondition.Evaluate(input);
        }

        public override string ToString()
        {
            return string.Format("{{{0} && {1}}}", leftCondition, rightCondition);
        }
    }

    /// <summary>
    /// Implementation of a AND condition.
    /// </summary>
    public class AndCondition : AndCondition<IState>, ICondition
    {
        public AndCondition(ICondition<IState> leftCondition, ICondition<IState> rightCondition)
            : base(leftCondition, rightCondition)
        { }
    }
}
