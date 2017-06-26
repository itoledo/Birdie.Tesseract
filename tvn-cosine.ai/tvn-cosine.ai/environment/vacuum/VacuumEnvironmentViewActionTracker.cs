 namespace aima.core.environment.vacuum;

 

public class VacuumEnvironmentViewActionTracker : EnvironmentView {
	private StringBuilder actions = null;

	public VacuumEnvironmentViewActionTracker(StringBuilder envChanges) {
		this.actions = envChanges;
	}

	//
	// START-EnvironmentView
	public void notify(string msg) {
		// Do nothing by default.
	}

	public void agentAdded(Agent agent, Environment source) {
		// Do nothing by default.
	}

	public void agentActed(Agent agent, Percept percept, Action action, Environment source) {
		actions.Append(action);
	}

	// END-EnvironmentView
	//
}
