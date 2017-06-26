 namespace aima.core.environment.vacuum;

 

import java.util.function.Function;

/**
 * Map fully observable state percepts to their corresponding state
 * representation.
 * 
 * @author Andrew Brown
 */
public class FullyObservableVacuumEnvironmentPerceptToStateFunction : Function<Percept, Object> {

	/**
	 * Default Constructor.
	 */
	public FullyObservableVacuumEnvironmentPerceptToStateFunction() {
	}

	 
	public object apply(Percept p) {
		// Note: VacuumEnvironmentState implements
		// FullyObservableVacuumEnvironmentPercept
		return (VacuumEnvironmentState) p;
	}
}
