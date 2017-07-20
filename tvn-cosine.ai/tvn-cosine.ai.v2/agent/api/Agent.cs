namespace tvn_cosine.ai.v2.agent.api
{
    /// <summary>
    /// Agents interact with environments through sensors and actuators.
    /// </summary>
    /// <typeparam name="A">the type of actions the agent can take.</typeparam>
    /// <typeparam name="P">the specific type of perceptual information the agent can perceive through its sensors.</typeparam>
    public interface Agent<A, P>
    {
        /// <summary>
        /// Call the Agent's program, which maps any given percept sequences to an action.
        /// </summary>
        /// <param name="percept">The current percept of a sequence perceived by the Agent within its environment.</param>
        /// <returns>the Action to be taken in response to the currently perceived percept.</returns>
        A perceive(P percept);
    } 
}
