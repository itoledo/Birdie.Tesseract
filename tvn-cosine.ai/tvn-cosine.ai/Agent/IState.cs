namespace tvn.cosine.ai.agent
{
    /// <summary>
    /// The most effective way to handle partial observability is for the agent to
    /// keep track of the part of the world it can't see now. That is, the agent
    /// should maintain some sort of internal state that depends on the percept
    /// history and thereby reflects at least some of the unobserved aspects of the
    /// current state.
    /// </summary>
    public interface IState
    {
    }
}
