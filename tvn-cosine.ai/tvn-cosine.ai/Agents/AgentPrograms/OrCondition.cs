namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// Implementation of an OR condition.
    /// </summary>
    /// <typeparam name="INPUT"></typeparam>
    public class OrCondition<INPUT> : LeftRightConditionBase<INPUT>, ICondition<INPUT>
    {
        public OrCondition(ICondition<INPUT> leftCondition, ICondition<INPUT> rightCondition)
            : base(leftCondition, rightCondition)
        { }

        public override bool Evaluate(INPUT input)
        {
            return leftCondition.Evaluate(input) || rightCondition.Evaluate(input);
        }

        public override string ToString()
        {
            return string.Format("{{{0} || {1}}}", leftCondition, rightCondition);
        }
    }

    /// <summary>
    /// Implementation of an OR condition.
    /// </summary>
    public class OrCondition : OrCondition<IState>, ICondition
    {
        public OrCondition(ICondition<IState> leftCondition, ICondition<IState> rightCondition) 
            : base(leftCondition, rightCondition)
        { }
    }
}
