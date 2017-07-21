namespace aima.test.unit.search.uninformed;

using java.util.Arrays;
using java.util.Collection;
using java.util.Collections;
using java.util.List;

using org.junit.Assert;
using org.junit.Test;
using org.junit.runner.RunWith;
using org.junit.runners.Parameterized;
using org.junit.runners.Parameterized.Parameter;
using org.junit.runners.Parameterized.Parameters;

using aima.core.environment.map2d.GoAction;
using aima.core.environment.map2d.SimplifiedRoadMapOfPartOfRomania;
using aima.core.environment.support.ProblemFactory;
using aima.core.environment.vacuum.VELocalState;
using aima.core.environment.vacuum.VacuumEnvironment;
using aima.core.search.api.Problem;
using aima.core.search.api.SearchForActionsFunction;
using aima.core.search.basic.uninformed.BreadthFirstSearch;
using aima.extra.search.pqueue.QueueSearchForActions;
using aima.extra.search.pqueue.uninformed.BreadthFirstQueueSearch;
using aima.extra.search.pqueue.uninformed.GraphGoalTestedFirstQueueSearch;
using aima.extra.search.pqueue.uninformed.GraphQueueSearch;
using aima.extra.search.pqueue.uninformed.TreeGoalTestedFirstQueueSearch;
using aima.extra.search.pqueue.uninformed.TreeQueueSearch;

@RunWith(Parameterized.class)
public class BreadthFirstSearchTest {

	@Parameters(name = "{index}: {0}")
	public static Collection<Object[]> implementations() {
		return Arrays.asList(new Object[][] { { "BreadthFirstSearch" },
				{ "BreadthFirstQueueSearch-GraphGoalTestFirst" }, { "BreadthFirstQueueSearch-Graph" },
				{ "BreadthFirstQueueSearch-TreeGoalTestedFirst" }, { "BreadthFirstQueueSearch-Tree" } });
	}

	@Parameter
	public string searchFunctionName;

	public <A, S> List<A> searchForActions(Problem<A, S> problem) {
		SearchForActionsFunction<A, S> searchForActionsFunction;
		if ("BreadthFirstSearch".equals(searchFunctionName)) {
			searchForActionsFunction = new BreadthFirstSearch<A, S>();
		} else {
			QueueSearchForActions<A, S> qsearchVariant;
			String variant = searchFunctionName.split("-")[1];
			// NOTE: Matches version in book
			if (variant.equals("GraphGoalTestFirst")) {
				qsearchVariant = new GraphGoalTestedFirstQueueSearch<>();
			} else if (variant.equals("Graph")) {
				qsearchVariant = new GraphQueueSearch<>();
			} else if (variant.equals("TreeGoalTestedFirst")) {
				qsearchVariant = new TreeGoalTestedFirstQueueSearch<>();
			} else {
				qsearchVariant = new TreeQueueSearch<>();
			}
			searchForActionsFunction = new BreadthFirstQueueSearch<A, S>(qsearchVariant);
		}

		return searchForActionsFunction.apply(problem);
	}

	[TestMethod]
	public void testSimplifiedRoadmapOfPartOfRomania() {
		Assert.assertEquals(Arrays.asList((String) null),
				searchForActions(ProblemFactory.getSimplifiedRoadMapOfPartOfRomaniaProblem(
						SimplifiedRoadMapOfPartOfRomania.ARAD, SimplifiedRoadMapOfPartOfRomania.ARAD)));

		Assert.assertEquals(
				Arrays.asList(new GoAction(SimplifiedRoadMapOfPartOfRomania.SIBIU),
						new GoAction(SimplifiedRoadMapOfPartOfRomania.FAGARAS),
						new GoAction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST)),
				searchForActions(ProblemFactory.getSimplifiedRoadMapOfPartOfRomaniaProblem(
						SimplifiedRoadMapOfPartOfRomania.ARAD, SimplifiedRoadMapOfPartOfRomania.BUCHAREST)));
	}

	[TestMethod]
	public void testSimpleVacuumEnvironment() {
		Assert.assertEquals(Arrays.asList((String) null),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("A",
						new VELocalState("A", VacuumEnvironment.Status.Clean),
						new VELocalState("B", VacuumEnvironment.Status.Clean))));

		Assert.assertEquals(Arrays.asList(VacuumEnvironment.ACTION_RIGHT, VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("A",
						new VELocalState("A", VacuumEnvironment.Status.Clean),
						new VELocalState("B", VacuumEnvironment.Status.Dirty))));

		Assert.assertEquals(Arrays.asList(VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("A",
						new VELocalState("A", VacuumEnvironment.Status.Dirty),
						new VELocalState("B", VacuumEnvironment.Status.Clean))));

		Assert.assertEquals(
				Arrays.asList(VacuumEnvironment.ACTION_SUCK, VacuumEnvironment.ACTION_RIGHT,
						VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("A",
						new VELocalState("A", VacuumEnvironment.Status.Dirty),
						new VELocalState("B", VacuumEnvironment.Status.Dirty))));

		Assert.assertEquals(Arrays.asList((String) null),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("B",
						new VELocalState("A", VacuumEnvironment.Status.Clean),
						new VELocalState("B", VacuumEnvironment.Status.Clean))));

		Assert.assertEquals(Arrays.asList(VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("B",
						new VELocalState("A", VacuumEnvironment.Status.Clean),
						new VELocalState("B", VacuumEnvironment.Status.Dirty))));

		Assert.assertEquals(Arrays.asList(VacuumEnvironment.ACTION_LEFT, VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("B",
						new VELocalState("A", VacuumEnvironment.Status.Dirty),
						new VELocalState("B", VacuumEnvironment.Status.Clean))));

		Assert.assertEquals(
				Arrays.asList(VacuumEnvironment.ACTION_SUCK, VacuumEnvironment.ACTION_LEFT,
						VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("B",
						new VELocalState("A", VacuumEnvironment.Status.Dirty),
						new VELocalState("B", VacuumEnvironment.Status.Dirty))));

		// A slightly larger world
		Assert.assertEquals(
				Arrays.asList(VacuumEnvironment.ACTION_SUCK, VacuumEnvironment.ACTION_RIGHT,
						VacuumEnvironment.ACTION_SUCK, VacuumEnvironment.ACTION_RIGHT, VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("A",
						new VELocalState("A", VacuumEnvironment.Status.Dirty),
						new VELocalState("B", VacuumEnvironment.Status.Dirty),
						new VELocalState("C", VacuumEnvironment.Status.Dirty))));

		Assert.assertEquals(
				Arrays.asList(VacuumEnvironment.ACTION_SUCK, VacuumEnvironment.ACTION_LEFT,
						VacuumEnvironment.ACTION_SUCK, VacuumEnvironment.ACTION_LEFT, VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("C",
						new VELocalState("A", VacuumEnvironment.Status.Dirty),
						new VELocalState("B", VacuumEnvironment.Status.Dirty),
						new VELocalState("C", VacuumEnvironment.Status.Dirty))));

		Assert.assertEquals(
				// NOTE: This sequence of actions is only correct because we
				// always order our possible actions [Left, Suck, Right] in
				// ProblemFactory.getSimpleVacuumWorldProblem(), if the order
				// was different we would likely get a different answer.
				Arrays.asList(VacuumEnvironment.ACTION_LEFT, VacuumEnvironment.ACTION_SUCK,
						VacuumEnvironment.ACTION_RIGHT, VacuumEnvironment.ACTION_SUCK, VacuumEnvironment.ACTION_RIGHT,
						VacuumEnvironment.ACTION_SUCK),
				searchForActions(ProblemFactory.getSimpleVacuumWorldProblem("B",
						new VELocalState("A", VacuumEnvironment.Status.Dirty),
						new VELocalState("B", VacuumEnvironment.Status.Dirty),
						new VELocalState("C", VacuumEnvironment.Status.Dirty))));
	}

	[TestMethod]
	public void testReachableSimpleBinaryTreeGoals() {
		Assert.assertEquals(Arrays.asList((String) null),
				searchForActions(ProblemFactory.getSimpleBinaryTreeProblem("A", "A")));

		Assert.assertEquals(Arrays.asList("B"), searchForActions(ProblemFactory.getSimpleBinaryTreeProblem("A", "B")));

		Assert.assertEquals(Arrays.asList("C"), searchForActions(ProblemFactory.getSimpleBinaryTreeProblem("A", "C")));

		Assert.assertEquals(Arrays.asList("B", "E"),
				searchForActions(ProblemFactory.getSimpleBinaryTreeProblem("A", "E")));

		Assert.assertEquals(Arrays.asList("C", "F", "L"),
				searchForActions(ProblemFactory.getSimpleBinaryTreeProblem("A", "L")));
	}

	[TestMethod]
	public void testUnreachableSimpleBinaryTreeGoals() {
		Assert.assertEquals(Collections.<String>emptyList(),
				searchForActions(ProblemFactory.getSimpleBinaryTreeProblem("B", "A")));
	}
}
