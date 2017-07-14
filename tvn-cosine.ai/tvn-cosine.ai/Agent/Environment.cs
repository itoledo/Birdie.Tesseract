using System.Collections.Generic;

namespace tvn.cosine.ai.agent
{
    /// <summary>
    /// An abstract description of possible discrete Environments in which Agent(s) can perceive and act.
    /// </summary>
    public interface Environment : EnvironmentViewNotifier
    {
        /// <summary>
        /// Returns the Agents belonging to this Environment.
        /// </summary>
        /// <returns>The Agents belonging to this Environment.</returns>
        IList<Agent> getAgents();

        /// <summary>
        /// Add an agent to the Environment.
        /// </summary>
        /// <param name="agent">the agent to be added.</param>
        void addAgent(Agent agent);

        /// <summary>
        /// Remove an agent from the environment.
        /// </summary>
        /// <param name="agent">the agent to be removed.</param>
        void removeAgent(Agent agent);

        /// <summary>
        /// Returns the EnvironmentObjects that exist in this Environment.
        /// </summary>
        /// <returns>the EnvironmentObjects that exist in this Environment.</returns>
        IList<EnvironmentObject> getEnvironmentObjects();

        /// <summary>
        /// Add an EnvironmentObject to the Environment.
        /// </summary>
        /// <param name="environmentObject">the EnvironmentObject to be added.</param>
        void addEnvironmentObject(EnvironmentObject environmentObject);

        /// <summary>
        /// Remove an EnvironmentObject from the Environment.
        /// </summary>
        /// <param name="environmentObject">the EnvironmentObject to be removed.</param>
        void removeEnvironmentObject(EnvironmentObject environmentObject);

        /// <summary>
        /// Move the Environment one time step forward.
        /// </summary>
        void step();

        /// <summary>
        /// Move the Environment n time steps forward.
        /// </summary>
        /// <param name="n">the number of time steps to move the Environment forward.</param>
        void step(int n);

        /// <summary>
        /// Step through time steps until the Environment has no more tasks.
        /// </summary>
        void stepUntilDone();

        /// <summary>
        /// Returns true if the Environment is finished with its current task(s).
        /// </summary>
        /// <returns>true if the Environment is finished with its current task(s).</returns>
        bool isDone();
         
        /// <summary>
        /// Retrieve the performance measure associated with an Agent.
        /// </summary>
        /// <param name="agent">the Agent for which a performance measure is to be retrieved.</param>
        /// <returns>the performance measure associated with the Agent.</returns>
        double getPerformanceMeasure(Agent agent);
         
        /// <summary>
        /// Add a view on the Environment.
        /// </summary>
        /// <param name="environmentView">the EnvironmentView to be added.</param>
        void addEnvironmentView(EnvironmentView environmentView);
         
        /// <summary>
        /// Remove a view on the Environment.
        /// </summary>
        /// <param name="environmentView">the EnvironmentView to be removed.</param>
        void removeEnvironmentView(EnvironmentView environmentView);
    }
}
