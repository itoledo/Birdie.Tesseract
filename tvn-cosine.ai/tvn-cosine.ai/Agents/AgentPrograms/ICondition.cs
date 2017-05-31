namespace tvn_cosine.ai.Agents.AgentPrograms
{
    public interface ICondition<INPUT>
    {
        bool Evaluate(INPUT input);
    }

    public interface ICondition : ICondition<IState>
    {

    }
}