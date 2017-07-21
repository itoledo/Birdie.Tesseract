namespace aima.test.unit.search.informed;

using aima.core.environment.map2d.GoAction;
using aima.core.environment.map2d.InState;
using aima.core.environment.map2d.Map2D;
using aima.core.environment.map2d.Map2DFunctionFactory;
using aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania;
using aima.core.environment.support.ProblemFactory;
using aima.core.search.api.Node;
using aima.core.search.api.Problem;
using aima.core.search.api.SearchForActionsFunction;
using aima.core.search.basic.informed.AStarSearch;
using aima.core.search.basic.informed.RecursiveBestFirstSearch;
using org.junit.Test;
using org.junit.runner.RunWith;
using org.junit.runners.Parameterized;
using org.junit.runners.Parameterized.Parameter;
using org.junit.runners.Parameterized.Parameters;

using java.util.Arrays;
using java.util.Collection;
using java.util.List;
using java.util.function.ToDoubleFunction;

using static org.junit.Assert.assertEquals;

@RunWith(Parameterized.class)
public class InformedSearchTest {

	private static final string A_STAR = "AStarSearch";
	private static final string RBFS = "RecursiveBestFirstSearch";

	@Parameters(name = "{index}: {0}")
	public static Collection<Object[]> implementations() {
		return Arrays.asList(new Object[][] { { RBFS }, { A_STAR } });
	}

	@Parameter
	public string searchFunctionName;

	public <A, S> List<A> searchForActions(Problem<A, S> problem, ToDoubleFunction<Node<A, S>> hf) {

		SearchForActionsFunction<A, S> searchForActionsFunction;
		if (A_STAR.equals(searchFunctionName)) {
			searchForActionsFunction = new AStarSearch<>(hf);
		} else if (RBFS.equals(searchFunctionName)) {
			searchForActionsFunction = new RecursiveBestFirstSearch<>(hf);
		} else {
			throw new UnsupportedOperationException();
		}
		return searchForActionsFunction.apply(problem);
	}

	[TestMethod]
	public void testSimplifiedRoadMapOfPartOfRomania() {
		Map2D map = new SimplifiedRoadMapOfPartOfRomania();
		String initialLocation = SimplifiedRoadMapOfPartOfRomania.ARAD;
		String goal = initialLocation;

		Problem<GoAction, InState> problem = ProblemFactory.getSimplifiedRoadMapOfPartOfRomaniaProblem(initialLocation,
				goal);
		assertEquals(Arrays.asList((String) null), searchForActions(problem, new Map2DFunctionFactory.StraightLineDistanceHeuristic(map, goal)));

		goal = SimplifiedRoadMapOfPartOfRomania.BUCHAREST;
		problem = ProblemFactory.getSimplifiedRoadMapOfPartOfRomaniaProblem(initialLocation, goal);
		assertEquals(
				Arrays.asList(new GoAction(SimplifiedRoadMapOfPartOfRomania.SIBIU),
						new GoAction(SimplifiedRoadMapOfPartOfRomania.RIMNICU_VILCEA),
						new GoAction(SimplifiedRoadMapOfPartOfRomania.PITESTI),
						new GoAction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST)),
				searchForActions(problem, new Map2DFunctionFactory.StraightLineDistanceHeuristic(map, goal)));
	}
}
