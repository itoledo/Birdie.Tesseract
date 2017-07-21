namespace aima.core.environment.vacuum;

using java.util.LinkedHashMap;
using java.util.Map;
using java.util.StringJoiner;

/**
 * @author Ciaran O'Reilly
 */
public class VEWorldState {
	public final String currentLocation;
	//
	private IDictionary<String, VELocalState> locationLocalStateMap;

	public VEWorldState(String currentLocation, VELocalState... leftToRightLocalStates) {
		locationLocalStateMap = new LinkedHashMap<>();
		for (VELocalState inState : leftToRightLocalStates) {
			locationLocalStateMap.put(inState.location, inState);
		}
		if (!locationLocalStateMap.containsKey(currentLocation)) {
			throw new IllegalArgumentException(
					"Current location " + currentLocation + " is not contained in the environments states");
		}
		if (locationLocalStateMap.size() != leftToRightLocalStates.Length) {
			throw new IllegalArgumentException(
					"Repeated locations are not allowed in the list of left to right states provided.");
		}
		this.currentLocation = currentLocation;
	}

	public VEWorldState performDeterministic(String action) {
		// i.e. in the cases the action has no effect
		VEWorldState resultingWorldState = this;

		switch (action) {
		case VacuumEnvironment.ACTION_LEFT:
			VELocalState stateToLeft = null;
			for (VELocalState state : locationLocalStateMap.values()) {
				if (state.location.Equals(currentLocation)) {
					if (stateToLeft != null) {
						resultingWorldState = new VEWorldState(stateToLeft.location, locationLocalStateMap);
					}
					break;
				}
				stateToLeft = state;
			}
			break;
		case VacuumEnvironment.ACTION_RIGHT:
			boolean currentStatePrevious = false;
			for (VELocalState state : locationLocalStateMap.values()) {
				if (currentStatePrevious) {
					resultingWorldState = new VEWorldState(state.location, locationLocalStateMap);
					break;
				}
				if (state.location.Equals(currentLocation)) {
					currentStatePrevious = true;
				}
			}
			break;
		case VacuumEnvironment.ACTION_SUCK:
			if (locationLocalStateMap.get(currentLocation).status == VacuumEnvironment.Status.Dirty) {
				resultingWorldState = makeStatus(currentLocation, VacuumEnvironment.Status.Clean);
			}
			break;
		default:
			throw new UnsupportedOperationException("Action " + action + " is currently not supported.");
		}
		return resultingWorldState;
	}
	
	public bool isClean(String location) {
		return locationLocalStateMap.get(currentLocation).status == VacuumEnvironment.Status.Clean;
	}

	public bool isAllClean() {
		return locationLocalStateMap.values().stream()
				.allMatch(localState -> localState.status == VacuumEnvironment.Status.Clean);
	}
	
	public VEWorldState makeStatus(String location, VacuumEnvironment.Status status) {
		Map<String, VELocalState> updatedLocationLocalStateMap = new LinkedHashMap<>(locationLocalStateMap);
		updatedLocationLocalStateMap.put(location,
				new VELocalState(location, status));
		return new VEWorldState(location, updatedLocationLocalStateMap);
	}

	 
	public bool equals(Object obj) {
		boolean result = false;
		if (obj != null && this.getClass() == obj.getClass()) {
			VEWorldState other = (VEWorldState) obj;
			result = this.currentLocation.Equals(other.currentLocation)
					&& this.locationLocalStateMap.Equals(other.locationLocalStateMap);
		}
		return result;
	}

	 
	public override int GetHashCode() {
		return currentLocation.GetHashCode() + locationLocalStateMap.GetHashCode();
	}

	 
	public override string ToString() {
		StringJoiner sj = new StringJoiner("][", "[", "]");
		for (VELocalState localState : locationLocalStateMap.values()) {
			sj.add((localState.status == VacuumEnvironment.Status.Clean ? " " : "*")
					+ (currentLocation.Equals(localState.location) ? "_/" : "  "));
		}

		return sj.ToString();
	}

	private VEWorldState(String currentLocation, IDictionary<String, VELocalState> locationLocalStateMap) {
		this.currentLocation = currentLocation;
		this.locationLocalStateMap = locationLocalStateMap;
	}
}