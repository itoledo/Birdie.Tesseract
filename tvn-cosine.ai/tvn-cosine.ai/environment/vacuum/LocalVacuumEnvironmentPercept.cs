using System.Text;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common;

namespace tvn.cosine.ai.environment.vacuum
{
    /**
     * Represents a local percept in the vacuum environment (i.e. details the
     * agent's location and the state of the square the agent is currently at).
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * @author Andrew Brown
     */
    public class LocalVacuumEnvironmentPercept : DynamicPercept, IToString
    {
        public const string ATTRIBUTE_AGENT_LOCATION = "agentLocation";
        public const string ATTRIBUTE_STATE = "state";

        /**
         * Construct a vacuum environment percept from the agent's perception of the
         * current location and state.
         * 
         * @param agentLocation
         *            the agent's perception of the current location.
         * @param state
         *            the agent's perception of the current state.
         */
        public LocalVacuumEnvironmentPercept(string agentLocation,
                VacuumEnvironment.LocationState state)
        {
            setAttribute(ATTRIBUTE_AGENT_LOCATION, agentLocation);
            setAttribute(ATTRIBUTE_STATE, state);
        }

        /**
         * Return the agent's perception of the current location, which is either A
         * or B.
         * 
         * @return the agent's perception of the current location, which is either A
         *         or B.
         */
        public string getAgentLocation()
        {
            return (string)getAttribute(ATTRIBUTE_AGENT_LOCATION);
        }

        /**
         * Return the agent's perception of the current state, which is either
         * <em>Clean</em> or <em>Dirty</em>.
         * 
         * @return the agent's perception of the current state, which is either
         *         <em>Clean</em> or <em>Dirty</em>.
         */
        public VacuumEnvironment.LocationState getLocationState()
        {
            return (VacuumEnvironment.LocationState)getAttribute(ATTRIBUTE_STATE);
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
