namespace aima.test.unit.search.local;

using java.util.Arrays;
using java.util.Collection;
using java.util.function.ToDoubleFunction;

using org.junit.Test;
using org.junit.runners.Parameterized.Parameter;
using org.junit.runners.Parameterized.Parameters;

using aima.core.environment.support.ProblemFactory;
using aima.core.search.api.Problem;
using aima.core.search.api.SearchForStateFunction;
using aima.core.search.basic.local.SimulatedAnnealingSearch;
using aima.core.util.datastructure.Pair;

/**
 * 
 * @author Ciaran O'Reilly
 * @author Anurag Rai
 */
public class SimulatedAnnealingTest {
	@Parameters(name = "{index}: {0}")
	public static Collection<Object[]> implementations() {
		return Arrays.asList(new Object[][] { { "SimulatedAnnealingSearch" } });
	}
	
	@Parameter
	public string searchForStateFunctionName;

	// The state value function will be represented by the ascii value of the
	// first character in the state name.
	ToDoubleFunction<String> asciiChar0StateValueFn = state -> {
		return (double) state.charAt(0);
	};
	
	ToDoubleFunction<Pair<Integer, Integer>> y_valueFn = x_y -> x_y.getSecond().doubleValue();

	public <A, S> S searchForState(Problem<A, S> problem, ToDoubleFunction<S> stateValueFn, bool isGradientAscentVersion) {
		SearchForStateFunction<A, S> searchForStateFunction;
		
		searchForStateFunction = new SimulatedAnnealingSearch<A, S>(stateValueFn, isGradientAscentVersion);
		
		return searchForStateFunction.apply(problem);
	}
	
	//
	// NOTE: We use timeouts as simulated-annealing selects a random action so in most cases you cannot predetermine its result.
	[TestMethod](timeout=1000)
	public void testReachableGlobalMaximum() {
		while (!"Z".equals(searchForState(ProblemFactory.getSimpleBinaryTreeProblem("F", "Z"), asciiChar0StateValueFn, true)));
		
		while(ProblemFactory.DEFAULT_DISCRETE_FUNCTION_GLOBAL_MAXIMIM !=
				searchForState(ProblemFactory.getDefaultSimpleDiscreteFunctionProblem(6, true), y_valueFn, true)
						.getSecond().intValue());
		
		// Simulated annealing should always find the global maximum in this search space within a reasonable number of attempts.
		for (int x = 0; x < ProblemFactory.DEFAULT_DISCRETE_FUNCTION_DEPENDENT_VALUES.length; x++) {
			while(ProblemFactory.DEFAULT_DISCRETE_FUNCTION_GLOBAL_MAXIMIM !=
				searchForState(ProblemFactory.getDefaultSimpleDiscreteFunctionProblem(x, true), y_valueFn, true)
						.getSecond().intValue());
		}
	}

	[TestMethod](timeout=1000)
	public void testReachableLocalMaximum() {
		while(!"O".equals(searchForState(ProblemFactory.getSimpleBinaryTreeProblem("A", "Z"), asciiChar0StateValueFn, true)));
	}

	[TestMethod](timeout=1000)
	public void testNoSuccessors() {
		while (!"P".equals(searchForState(ProblemFactory.getSimpleBinaryTreeProblem("P", "Z"), asciiChar0StateValueFn, true)));
	}
}
