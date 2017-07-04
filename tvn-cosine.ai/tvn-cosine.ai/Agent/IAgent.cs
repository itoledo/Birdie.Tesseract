namespace tvn.cosine.ai.Agent
{
    /// <summary>
    /// Agents interact with environments through sensors and actuators.
    /// </summary>
    public interface IAgent : IEnvironmentObject
    {
        /// <summary>
        /// The Agent Program
        /// </summary>
        IAgentProgram AgentProgram { get; }

        /// <summary>
        /// Call the Agent's program, which maps any given percept sequences to an action.
        /// </summary>
        /// <param name="percept">The current percept of a sequence perceived by the Agent.</param>
        /// <returns>the Action to be taken in response to the currently perceived percept.</returns>
        IAction Execute(IPercept percept);

        /// <summary>
        /// Life-cycle indicator as to the liveness of an Agent.
        /// </summary>
        bool Alive { get; set; }
         
    }
}
