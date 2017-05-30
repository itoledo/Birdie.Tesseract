using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn_cosine.ai.Agents
{
    /// <summary>
    /// Allows external applications/logic to view the interaction of Agent(s) with an Environment.
    /// </summary>
    public interface IEnvironmentView
    {
        /// <summary>
        /// A simple notification message from an object in the Environment.
        /// </summary>
        /// <param name="message">the message received.</param>
        void Notify(string message);

        /// <summary>
        /// Indicates an Agent has been added to the environment and what it perceives initially.
        /// </summary>
        /// <param name="agent">the Agent just added to the Environment.</param>
        /// <param name="source">the Environment to which the agent was added.</param>
        void AgentAdded(IAgent agent, IEnvironment source);

        /// <summary>
        /// Indicates the Environment has changed as a result of an Agent's action.
        /// </summary>
        /// <param name="agent">the Agent that performed the Action.</param>
        /// <param name="percept">the Percept the Agent received from the environment.</param>
        /// <param name="action">the Action the Agent performed.</param>
        /// <param name="source">the Environment in which the agent has acted.</param>
        void AgentActed(IAgent agent, IPercept percept, IAction action, IEnvironment source);
    }
}
