using System.Text;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;

namespace tvn.cosine.ai.environment.vacuum
{
    /// <summary>
    /// Represents a local percept in the vacuum environment (i.e. details the agent's location and the state of the square the agent is currently at).
    /// </summary>
    public class LocalVacuumEnvironmentPercept : DynamicPercept
    {
        public const string ATTRIBUTE_AGENT_LOCATION = "agentLocation";
        public const string ATTRIBUTE_STATE = "state";

        /// <summary>
        /// Construct a vacuum environment percept from the agent's perception of the current location and state.
        /// </summary>
        /// <param name="agentLocation">the agent's perception of the current location.</param>
        /// <param name="state">the agent's perception of the current state.</param>
        public LocalVacuumEnvironmentPercept(string agentLocation, VacuumEnvironment.LocationState state)
        {
            SetAttribute(ATTRIBUTE_AGENT_LOCATION, agentLocation);
            SetAttribute(ATTRIBUTE_STATE, state);
        }

        /// <summary>
        /// Return the agent's perception of the current location, which is either A or B.
        /// </summary>
        /// <returns>the agent's perception of the current location, which is either A or B.</returns>
        public string getAgentLocation()
        {
            return (string)GetAttribute(ATTRIBUTE_AGENT_LOCATION);
        }

        /// <summary>
        /// Return the agent's perception of the current state, which is either Clean or Dirty.
        /// </summary>
        /// <returns>the agent's perception of the current state, which is either Clean or Dirty.</returns>
        public VacuumEnvironment.LocationState getLocationState()
        {
            return (VacuumEnvironment.LocationState)GetAttribute(ATTRIBUTE_STATE);
        }

        /// <summary>
        /// Determine whether this percept matches an environment state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="agent"></param>
        /// <returns>true of the percept matches an environment state, false otherwise.</returns>
        public bool matches(VacuumEnvironmentState state, Agent agent)
        {
            if (!this.getAgentLocation().Equals(state.getAgentLocation(agent)))
            {
                return false;
            }
            if (!this.getLocationState().Equals(state.getLocationState(this.getAgentLocation())))
            {
                return false;
            }
            return true;
        }

        /**
         * Return string representation of this percept.
         * 
         * @return a string representation of this percept.
         */
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(getAgentLocation());
            sb.Append(", ");
            sb.Append(getLocationState());
            sb.Append("]");
            return sb.ToString();
        }
    }

}
