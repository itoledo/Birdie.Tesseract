 namespace aima.core.environment.vacuum;

 
 

/**
 * Represents a local percept in the vacuum environment (i.e. details the
 * agent's location and the state of the square the agent is currently at).
 * 
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 * @author Mike Stampone
 * @author Andrew Brown
 */
public class LocalVacuumEnvironmentPercept : DynamicPercept {

	public static readonly string ATTRIBUTE_AGENT_LOCATION = "agentLocation";
	public static readonly string ATTRIBUTE_STATE = "state";

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
			VacuumEnvironment.LocationState state) {
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
	public string getAgentLocation() {
		return (string) getAttribute(ATTRIBUTE_AGENT_LOCATION);
	}

	/**
	 * Return the agent's perception of the current state, which is either
	 * <em>Clean</em> or <em>Dirty</em>.
	 * 
	 * @return the agent's perception of the current state, which is either
	 *         <em>Clean</em> or <em>Dirty</em>.
	 */
	public VacuumEnvironment.LocationState getLocationState() {
		return (VacuumEnvironment.LocationState) getAttribute(ATTRIBUTE_STATE);
	}

	/**
	 * Determine whether this percept matches an environment state
	 * 
	 * @param state
	 * @param agent
	 * @return true of the percept matches an environment state, false otherwise.
	 */
	public bool matches(VacuumEnvironmentState state, Agent agent) {
		if (!this.getAgentLocation() .Equals(state.getAgentLocation(agent))) {
			return false;
		}
		if (!this.getLocationState() .Equals(
				state.getLocationState(this.getAgentLocation()))) {
			return false;
		}
		return true;
	}

	/**
	 * Return string representation of this percept.
	 * 
	 * @return a string representation of this percept.
	 */
	 
	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		sb.Append("[");
		sb.Append(getAgentLocation());
		sb.Append(", ");
		sb.Append(getLocationState());
		sb.Append("]");
		return sb.ToString();
	}
}
