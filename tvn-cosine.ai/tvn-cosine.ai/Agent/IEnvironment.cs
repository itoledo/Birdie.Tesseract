using System.Collections.Generic;

namespace tvn.cosine.ai.Agent
{
    /// <summary>
    /// An abstract description of possible discrete Environments in which Agent(s) can perceive and act.
    /// </summary>
    public interface IEnvironment : IEnvironmentViewNotifier
    {
        /// <summary>
        /// The Agents belonging to this Environment.
        /// </summary>
        ISet<IAgent> Agents { get; }

        /// <summary>
        /// The EnvironmentObjects that exist in this Environment.
        /// </summary>
        ISet<IEnvironmentObject> EnvironmentObjects { get; }  
    }
}
