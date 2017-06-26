 namespace aima.core.environment.vacuum;

 
import java.util.Objects;
 

 
 
 
import aima.core.agent.impl.AbstractAgent;
 
 
import aima.core.agent.impl.aprog.ModelBasedReflexAgentProgram;
import aima.core.agent.impl.aprog.simplerule.ANDCondition;
import aima.core.agent.impl.aprog.simplerule.EQUALCondition;
 

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ModelBasedReflexVacuumAgent : AbstractAgent {

	private static readonly string ATTRIBUTE_CURRENT_LOCATION = "currentLocation";
	private static readonly string ATTRIBUTE_CURRENT_STATE = "currentState";
	private static readonly string ATTRIBUTE_STATE_LOCATION_A = "stateLocationA";
	private static readonly string ATTRIBUTE_STATE_LOCATION_B = "stateLocationB";

	public ModelBasedReflexVacuumAgent() {
		base(new ModelBasedReflexAgentProgram() {
			 
			protected void init() {
				setState(new DynamicState());
				setRules(getRuleSet());
			}

			protected DynamicState updateState(DynamicState state,
					Action anAction, Percept percept, Model model) {

				LocalVacuumEnvironmentPercept vep = (LocalVacuumEnvironmentPercept) percept;

				state.setAttribute(ATTRIBUTE_CURRENT_LOCATION,
						vep.getAgentLocation());
				state.setAttribute(ATTRIBUTE_CURRENT_STATE,
						vep.getLocationState());
				// Keep track of the state of the different locations
				if (objects .Equals(VacuumEnvironment.LOCATION_A, vep.getAgentLocation())) {
					state.setAttribute(ATTRIBUTE_STATE_LOCATION_A,
							vep.getLocationState());
				} else {
					state.setAttribute(ATTRIBUTE_STATE_LOCATION_B,
							vep.getLocationState());
				}
				return state;
			}
		});
	}

	//
	// PRIVATE METHODS
	//
	private static ISet<Rule> getRuleSet() {
		// Note: Using a LinkedHashSet so that the iteration order (i.e. implied
		// precedence) of rules can be guaranteed.
		Set<Rule> rules = new HashSet<Rule>();

		rules.Add(new Rule(new ANDCondition(new EQUALCondition(
				ATTRIBUTE_STATE_LOCATION_A,
				VacuumEnvironment.LocationState.Clean), new EQUALCondition(
				ATTRIBUTE_STATE_LOCATION_B,
				VacuumEnvironment.LocationState.Clean)), NoOpAction.NO_OP));
		rules.Add(new Rule(new EQUALCondition(ATTRIBUTE_CURRENT_STATE,
				VacuumEnvironment.LocationState.Dirty),
				VacuumEnvironment.ACTION_SUCK));
		rules.Add(new Rule(new EQUALCondition(ATTRIBUTE_CURRENT_LOCATION,
				VacuumEnvironment.LOCATION_A),
				VacuumEnvironment.ACTION_MOVE_RIGHT));
		rules.Add(new Rule(new EQUALCondition(ATTRIBUTE_CURRENT_LOCATION,
				VacuumEnvironment.LOCATION_B),
				VacuumEnvironment.ACTION_MOVE_LEFT));

		return rules;
	}
}
