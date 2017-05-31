namespace tvn_cosine.ai.Agents.AgentPrograms
{ 
    public interface IRule<INPUT, RESULT>
    { 
        RESULT Result { get; } 
        ICondition<INPUT> Condition { get; }

        bool Evaluate(INPUT state);
    }

    public interface IRule : IRule<IState, IAction>
    { }
}
