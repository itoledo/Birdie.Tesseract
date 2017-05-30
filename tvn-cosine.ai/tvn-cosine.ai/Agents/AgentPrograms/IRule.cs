namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// a Condition-action rule.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// the action of a condition-action rule.
        /// </summary>
        IAction Action { get; }

        /// <summary>
        /// the condition of a condition-action rule.
        /// </summary>
        ICondition Condition { get; }

        bool Evaluate(IState state);
    }
}
