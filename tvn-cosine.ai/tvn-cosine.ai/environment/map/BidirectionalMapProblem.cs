 namespace aima.core.environment.map;

import aima.core.search.framework.problem.BidirectionalProblem;
import aima.core.search.framework.problem.GeneralProblem;
import aima.core.search.framework.problem.GoalTest;
import aima.core.search.framework.problem.Problem;

/**
 * @author Ruediger Lunde
 * 
 */
public class BidirectionalMapProblem : GeneralProblem<String, MoveToAction>
		implements BidirectionalProblem<String, MoveToAction> {

	private Problem<String, MoveToAction> reverseProblem;

	public BidirectionalMapProblem(Map map, string initialState, string goalState) {
		this(map, initialState, goalState, GoalTest.isEqual(goalState));
	}

	public BidirectionalMapProblem(Map map, string initialState, string goalState, GoalTest<string> goalTest) {
		base(initialState, MapFunctions.createActionsFunction(map), MapFunctions.createResultFunction(),
				goalTest, MapFunctions.createDistanceStepCostFunction(map));

		reverseProblem = new GeneralProblem<>(goalState, MapFunctions.createReverseActionsFunction(map),
				MapFunctions.createResultFunction(), GoalTest.isEqual(initialState),
				MapFunctions.createDistanceStepCostFunction(map));
	}

	public Problem<String, MoveToAction> getOriginalProblem() {
		return this;
	}

	public Problem<String, MoveToAction> getReverseProblem() {
		return reverseProblem;
	}
}
