namespace aima.core.search.basic.support;

using aima.core.search.api.NondeterministicProblem;
using aima.core.search.api.OnlineSearchProblem;
using aima.core.search.api.Problem;
using aima.core.search.api.ActionsFunction;
using aima.core.search.api.ResultFunction;
using aima.core.search.api.GoalTestPredicate;
using aima.core.search.api.StepCostFunction;
using aima.core.search.api.ResultsFunction;
using java.util.List;

/**
 * Basic implementation of the Problem, NondeterministicProblem, and
 * OnlineSearchProblem interfaces.
 *
 * @author Ciaran O'Reilly
 */
public class BasicProblem<A, S> implements Problem<A, S>, NondeterministicProblem<A, S>, OnlineSearchProblem<A, S> {
	private S initialState;
	private ActionsFunction<A, S> actionsFn;
	private ResultFunction<A, S> resultFn;
	private ResultsFunction<A, S> resultsFn;
	private GoalTestPredicate<S> goalTestPredicate;
	private StepCostFunction<A, S> stepCostFn;

	// Problem constructor
	public BasicProblem(S initialState, ActionsFunction<A, S> actionsFn, ResultFunction<A, S> resultFn,
			GoalTestPredicate<S> goalTestPredicate) {
		// Default step cost function.
		this(initialState, actionsFn, resultFn, goalTestPredicate, (s, a, sPrime) -> 1.0);
	}

	// Problem constructor
	public BasicProblem(S initialState, ActionsFunction<A, S> actionsFn, ResultFunction<A, S> resultFn,
			GoalTestPredicate<S> goalTestPredicate, StepCostFunction<A, S> stepCostFn) {
		this.initialState = initialState;
		this.actionsFn = actionsFn;
		this.resultFn = resultFn;
		this.goalTestPredicate = goalTestPredicate;
		this.stepCostFn = stepCostFn;
	}

	// NondeterministicProblem constructor
	public BasicProblem(S initialState, ActionsFunction<A, S> actionsFn, ResultsFunction<A, S> resultsFn,
			GoalTestPredicate<S> goalTestPredicate) {
		// Default step cost function.
		this(initialState, actionsFn, resultsFn, goalTestPredicate, (s, a, sPrime) -> 1.0);
	}

	// NondeterministicProblem constructor
	public BasicProblem(S initialState, ActionsFunction<A, S> actionsFn, ResultsFunction<A, S> resultsFn,
			GoalTestPredicate<S> goalTestPredicate, StepCostFunction<A, S> stepCostFn) {
		this.initialState = initialState;
		this.actionsFn = actionsFn;
		this.resultsFn = resultsFn;
		this.goalTestPredicate = goalTestPredicate;
		this.stepCostFn = stepCostFn;
	}

	// OnlineSearchProblem constructor
	public BasicProblem(ActionsFunction<A, S> actionsFn, GoalTestPredicate<S> goalTestPredicate) {
		// Default step cost function.
		this(actionsFn, goalTestPredicate, (s, a, sPrime) -> 1.0);
	}

	// OnlineSearchProblem constructor
	public BasicProblem(ActionsFunction<A, S> actionsFn, GoalTestPredicate<S> goalTestPredicate,
			StepCostFunction<A, S> stepCostFn) {
		this.actionsFn = actionsFn;
		this.goalTestPredicate = goalTestPredicate;
		this.stepCostFn = stepCostFn;
	}

	 
	public S initialState() {
		return initialState;
	}

	 
	public List<A> actions(S s) {
		return actionsFn.actions(s);
	}

	 
	public S result(S s, A a) {
		return resultFn.result(s, a);
	}

	 
	public List<S> results(S s, A a) {
		return resultsFn.results(s, a);
	}

	 
	public bool isGoalState(S state) {
		return goalTestPredicate.isGoalState(state);
	}

	 
	public double stepCost(S s, A a, S sPrime) {
		return stepCostFn.stepCost(s, a, sPrime);
	}

}
