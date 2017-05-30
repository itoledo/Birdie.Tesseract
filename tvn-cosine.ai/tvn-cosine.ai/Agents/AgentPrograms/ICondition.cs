namespace tvn_cosine.ai.Agents.AgentPrograms
{
    public interface ICondition
    {
        bool Evaluate(IState state);
    }
}