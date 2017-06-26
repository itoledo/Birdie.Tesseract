 namespace aima.core.environment.vacuum;

 
 

import aima.core.agent.impl.AbstractAgent;
import aima.core.agent.impl.aprog.SimpleReflexAgentProgram;
import aima.core.agent.impl.aprog.simplerule.EQUALCondition;
 

/**
 * @author Ciaran O'Reilly
 * 
 */
public class SimpleReflexVacuumAgent : AbstractAgent {

	public SimpleReflexVacuumAgent() {
		base(new SimpleReflexAgentProgram(getRuleSet()));
	}

	//
	// PRIVATE METHODS
	//
	private static ISet<Rule> getRuleSet() {
		// Note: Using a LinkedHashSet so that the iteration order (i.e. implied
		// precedence) of rules can be guaranteed.
		Set<Rule> rules = new HashSet<Rule>();

		// Rules based on REFLEX-VACUUM-AGENT:
		// Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.8,
		// page 48.

		rules.Add(new Rule(new EQUALCondition(LocalVacuumEnvironmentPercept.ATTRIBUTE_STATE,
				VacuumEnvironment.LocationState.Dirty),
				VacuumEnvironment.ACTION_SUCK));
		rules.Add(new Rule(new EQUALCondition(
				LocalVacuumEnvironmentPercept.ATTRIBUTE_AGENT_LOCATION,
				VacuumEnvironment.LOCATION_A),
				VacuumEnvironment.ACTION_MOVE_RIGHT));
		rules.Add(new Rule(new EQUALCondition(
				LocalVacuumEnvironmentPercept.ATTRIBUTE_AGENT_LOCATION,
				VacuumEnvironment.LOCATION_B),
				VacuumEnvironment.ACTION_MOVE_LEFT));

		return rules;
	}
}
