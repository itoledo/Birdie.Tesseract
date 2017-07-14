namespace tvn.cosine.ai.agent
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.1, page 35.  <para />
    /// Figure 2.1 Agents interact with environments through sensors and actuators.
    /// </summary>
    public interface Agent : EnvironmentObject
    {
        /// <summary>
        /// Call the Agent's program, which maps any given percept sequences to an action.
        /// </summary>
        /// <param name="percept">
        /// The current percept of a sequence perceived by the Agent.</param>
        /// <returns>the Action to be taken in response to the currently perceived percept.</returns>
        Action execute(Percept percept);

        /// <summary>
        /// Life-cycle indicator as to the liveness of an Agent.
        /// </summary>
        /// <returns>true if the Agent is to be considered alive, false otherwise.</returns>
        bool isAlive();

        /// <summary>
        /// Set the current liveness of the Agent.
        /// </summary>
        /// <param name="alive">set to true if the Agent is to be considered alive, false otherwise.</param>
        void setAlive(bool alive);
    }
}
