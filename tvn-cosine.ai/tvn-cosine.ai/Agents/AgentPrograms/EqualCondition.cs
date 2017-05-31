namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// Implementation of a EQUALS condition.
    /// </summary>
    /// <typeparam name="INPUT"></typeparam>
    public class EqualCondition<INPUT> : LeftRightConditionBase<INPUT>
    {
        public EqualCondition(ICondition<INPUT> leftCondition, ICondition<INPUT> rightCondition)
            : base(leftCondition, rightCondition)
        { }

        public override bool Evaluate(INPUT input)
        {
            return leftCondition.Evaluate(input) == rightCondition.Evaluate(input);
        }

        public override string ToString()
        {
            return string.Format("{{{0} == {1}}}", leftCondition, rightCondition);
        }
    }

    /// <summary>
    /// Implementation of a EQUALS condition.
    /// </summary>
    public class EqualCondition : EqualCondition<IState>, ICondition
    {
        public EqualCondition(ICondition<IState> leftCondition, ICondition<IState> rightCondition) 
            : base(leftCondition, rightCondition)
        { }
    }
}
