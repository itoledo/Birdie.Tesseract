 namespace aima.core.environment.map;

 
 
 
import aima.core.agent.impl.AbstractEnvironment;
 

/**
 * Represents the environment a SimpleMapAgent can navigate.
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class MapEnvironment : AbstractEnvironment {

	private Map map = null;
	private MapEnvironmentState state = new MapEnvironmentState();

	public MapEnvironment(Map map) {
		this.map = map;
	}

	public void addAgent(Agent a, string startLocation) {
		// Ensure the agent state information is tracked before
		// adding to super, as super will notify the registered
		// EnvironmentViews that is was added.
		state.setAgentLocationAndTravelDistance(a, startLocation, 0.0);
		base.addAgent(a);
	}

	public string getAgentLocation(Agent a) {
		return state.getAgentLocation(a);
	}

	public double getAgentTravelDistance(Agent a) {
		return state.getAgentTravelDistance(a);
	}

	 
	public void executeAction(Agent agent, Action a) {

		if (!a.isNoOp()) {
			MoveToAction act = (MoveToAction) a;

			String currLoc = getAgentLocation(agent);
			Double distance = map.getDistance(currLoc, act.getToLocation());
			if (distance != null) {
				double currTD = getAgentTravelDistance(agent);
				state.setAgentLocationAndTravelDistance(agent,
						act.getToLocation(), currTD + distance);
			}
		}
	}

	 
	public Percept getPerceptSeenBy(Agent anAgent) {
		return new DynamicPercept(DynAttributeNames.PERCEPT_IN,
				getAgentLocation(anAgent));
	}

	public Map getMap() {
		return map;
	}
}