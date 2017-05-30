using System.Collections.Generic;

namespace tvn_cosine.ai.Agents
{
    /// <summary>
    /// An abstract description of possible discrete Environments in which Agent(s)
    /// can perceive and act.
    /// </summary>
    public interface IEnvironment : IEnvironmentViewNotifier
    {
        /// <summary>
        /// the Agents belonging to this Environment.
        /// </summary>
        IReadOnlyCollection<IAgent> Agents { get; }

        /// <summary>
        /// Add an agent to the Environment.
        /// </summary>
        /// <param name="agent">the agent to be added.</param>
        void AddAgent(IAgent agent);

        /// <summary>
        /// Remove an agent from the environment.
        /// </summary>
        /// <param name="agent">the agent to be removed.</param>
        void RemoveAgent(IAgent agent);

        /// <summary>
        /// the EnvironmentObjects that exist in this Environment.
        /// </summary>
        IReadOnlyCollection<IEnvironmentObject> EnvironmentObjects { get; }

        /// <summary>
        /// Add an EnvironmentObject to the Environment.
        /// </summary>
        /// <param name="environmentObject">the EnvironmentObject to be added.</param>
        void AddEnvironmentObject(IEnvironmentObject environmentObject);

        /// <summary>
        /// Remove an EnvironmentObject from the Environment.
        /// </summary>
        /// <param name="environmentObject">the EnvironmentObject to be removed.</param>
        void RemoveEnvironmentObject(IEnvironmentObject environmentObject);

        /// <summary>
        /// Move the Environment one time step forward.
        /// </summary>
        void Step();

        /// <summary>
        /// Move the Environment n time steps forward.
        /// </summary>
        /// <param name="n">the number of time steps to move the Environment forward.</param>
        void Step(int n);

        /// <summary>
        /// Step through time steps until the Environment has no more tasks.
        /// </summary>
        void StepUntilDone();

        /// <summary>
        /// <code>true</code> if the Environment is finished with its current task(s).
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Retrieve the performance measure associated with an Agent.
        /// </summary>
        /// <param name="agent">the Agent for which a performance measure is to be retrieved.</param>
        /// <returns>the performance measure associated with the Agent.</returns>
        double GetPerformanceMeasure(IAgent agent);

        /// <summary>
        /// the Environment Views loaded for environment
        /// </summary>
        IReadOnlyCollection<IEnvironmentView> EnvironmentViews { get; }

        /// <summary>
        /// Add a view on the Environment.
        /// </summary>
        /// <param name="environmentView">the EnvironmentView to be added.</param>
        void AddEnvironmentView(IEnvironmentView environmentView);

        /// <summary>
        /// Remove a view on the Environment.
        /// </summary>
        /// <param name="environmentView">the EnvironmentView to be removed.</param>
        void RemoveEnvironmentView(IEnvironmentView environmentView);
    }
}