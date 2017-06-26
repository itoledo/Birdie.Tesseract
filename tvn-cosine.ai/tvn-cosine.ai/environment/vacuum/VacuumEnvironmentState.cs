 namespace aima.core.environment.vacuum;

 
 
 
 

/**
 * Represents a state in the Vacuum World
 * 
 * @author Ciaran O'Reilly
 * @author Andrew Brown
 */
public class VacuumEnvironmentState : EnvironmentState, FullyObservableVacuumEnvironmentPercept, Cloneable {

	private IDictionary<String, VacuumEnvironment.LocationState> state;
	private IDictionary<Agent, String> agentLocations;

	/**
	 * Constructor
	 */
	public VacuumEnvironmentState() {
		state = new Dictionary<String, VacuumEnvironment.LocationState>();
		agentLocations = new Dictionary<Agent, String>();
	}

	/**
	 * Constructor
	 * 
	 * @param locAState
	 * @param locBState
	 */
	public VacuumEnvironmentState(VacuumEnvironment.LocationState locAState,
			VacuumEnvironment.LocationState locBState) {
		this();
		state.Add(VacuumEnvironment.LOCATION_A, locAState);
		state.Add(VacuumEnvironment.LOCATION_B, locBState);
	}

	 
	public string getAgentLocation(Agent a) {
		return agentLocations.get(a);
	}

	/**
	 * Sets the agent location
	 * 
	 * @param a
	 * @param location
	 */
	public void setAgentLocation(Agent a, string location) {
		agentLocations.Add(a, location);
	}

	 
	public VacuumEnvironment.LocationState getLocationState(string location) {
		return state.get(location);
	}

	/**
	 * Sets the location state
	 * 
	 * @param location
	 * @param s
	 */
	public void setLocationState(string location, VacuumEnvironment.LocationState s) {
		state.Add(location, s);
	}

	 
	public override bool Equals(object obj) {
		if (obj != null && GetType()== obj .GetType()) {
			VacuumEnvironmentState s = (VacuumEnvironmentState) obj;
			return state .Equals(s.state) && agentLocations .Equals(s.agentLocations);
		}
		return false;
	}

	/**
	 * Override hashCode()
	 * 
	 * @return the hash code for this object.
	 */
	 
	public override int GetHashCode() {
		return 3 * state .GetHashCode() + 13 * agentLocations .GetHashCode();
	}

	 
	public VacuumEnvironmentState clone() {
		VacuumEnvironmentState result = null;
		try {
			result = (VacuumEnvironmentState) base.clone();
			result.state = new Dictionary<String, VacuumEnvironment.LocationState>(state);
			agentLocations = new Dictionary<Agent, String>(agentLocations);
		} catch (Exception ex) {
			ex.printStackTrace();
		}
		return result;
	}
	
	/**
	 * Returns a string representation of the environment
	 * 
	 * @return a string representation of the environment
	 */
	 
	public override string ToString() {
		return this.state.ToString();
	}
}