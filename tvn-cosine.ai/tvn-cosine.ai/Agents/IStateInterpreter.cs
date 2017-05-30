namespace tvn_cosine.ai.Agents
{
    public interface IStateInterpreter<INPUT>
    {
        IState Interpret(INPUT input);
    }
}
