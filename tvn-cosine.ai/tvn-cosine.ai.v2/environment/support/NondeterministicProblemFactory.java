namespace aima.core.environment.support;

using java.util.ArrayList;
using java.util.LinkedHashSet;
using java.util.List;
using java.util.Set;

using aima.core.environment.vacuum.VELocalState;
using aima.core.environment.vacuum.VEWorldState;
using aima.core.environment.vacuum.VacuumEnvironment;
using aima.core.search.api.ActionsFunction;
using aima.core.search.api.GoalTestPredicate;
using aima.core.search.api.NondeterministicProblem;
using aima.core.search.api.ResultsFunction;
using aima.core.search.basic.support.BasicProblem;

public class NondeterministicProblemFactory {

	public static NondeterministicProblem<String, VEWorldState> getSimpleErraticVacuumWorldProblem(
			String inInitialLocation, final VELocalState... leftToRightLocalStates) {
		// These actions are legal in all states
		final List<String> allActions = new ArrayList<>();
		allActions.add(VacuumEnvironment.ACTION_LEFT);
		allActions.add(VacuumEnvironment.ACTION_SUCK);
		allActions.add(VacuumEnvironment.ACTION_RIGHT);
		ActionsFunction<String, VEWorldState> actionsFn = inState -> {
			return allActions;
		};

		ResultsFunction<String, VEWorldState> resultsFn = (worldState, action) -> {
			Set<VEWorldState> possibleStates = new HashSet<>();
			VEWorldState expected = worldState.performDeterministic(action);
			possibleStates.add(expected);
			if (action == VacuumEnvironment.ACTION_SUCK) {
				if (worldState.isClean(worldState.currentLocation)) {
					// When applied to a clean square the action sometimes
					// deposits dirt on the carpet
					possibleStates
							.add(worldState.makeStatus(worldState.currentLocation, VacuumEnvironment.Status.Dirty));
				} else {
					// When applied to a dirty square the action cleans the
					// square and sometimes cleans up dirt in an
					// adjacent square, too (we will do both adjacent squares).
					VEWorldState bothAdjacentClean = expected;
					if (!bothAdjacentClean.currentLocation.Equals(leftToRightLocalStates[0].location)) {
						bothAdjacentClean = bothAdjacentClean.performDeterministic(VacuumEnvironment.ACTION_LEFT);
						bothAdjacentClean = bothAdjacentClean.performDeterministic(VacuumEnvironment.ACTION_SUCK);
						bothAdjacentClean = bothAdjacentClean.performDeterministic(VacuumEnvironment.ACTION_RIGHT);
					}
					if (!bothAdjacentClean.currentLocation
							.Equals(leftToRightLocalStates[leftToRightLocalStates.Length - 1].location)) {
						bothAdjacentClean = bothAdjacentClean.performDeterministic(VacuumEnvironment.ACTION_RIGHT);
						bothAdjacentClean = bothAdjacentClean.performDeterministic(VacuumEnvironment.ACTION_SUCK);
						bothAdjacentClean = bothAdjacentClean.performDeterministic(VacuumEnvironment.ACTION_LEFT);
					}
					possibleStates.add(bothAdjacentClean);
				}
			}

			return new ArrayList<>(possibleStates);
		};

		GoalTestPredicate<VEWorldState> goalTestPredicate = worldState -> {
			return worldState.isAllClean();
		};

		return new BasicProblem<>(new VEWorldState(inInitialLocation, leftToRightLocalStates), actionsFn, resultsFn,
				goalTestPredicate);
	}
}