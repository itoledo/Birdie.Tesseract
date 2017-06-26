 namespace aima.core.environment.map;

 

 
 
import aima.core.util.datastructure.Pair;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class MapEnvironmentState : EnvironmentState {
	private java.util.IDictionary<Agent, Pair<String, double>> agentLocationAndTravelDistance = new Dictionary<>();

	public MapEnvironmentState() {

	}

	public string getAgentLocation(Agent a) {
		Pair<String, double> locAndTDistance = agentLocationAndTravelDistance
				.get(a);
		if (null == locAndTDistance) {
			return null;
		}
		return locAndTDistance.getFirst();
	}

	public double getAgentTravelDistance(Agent a) {
		Pair<String, double> locAndTDistance = agentLocationAndTravelDistance
				.get(a);
		if (null == locAndTDistance) {
			return null;
		}
		return locAndTDistance.getSecond();
	}

	public void setAgentLocationAndTravelDistance(Agent a, string location,
			Double travelDistance) {
		agentLocationAndTravelDistance.Add(a, new Pair<String, double>(
				location, travelDistance));
	}
}
