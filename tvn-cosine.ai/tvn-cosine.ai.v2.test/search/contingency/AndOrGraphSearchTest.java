namespace aima.test.unit.search.contingency;

using java.util.ArrayList;
using java.util.Arrays;
using java.util.Collection;
using java.util.List;

using org.junit.Assert;
using org.junit.Test;
using org.junit.runner.RunWith;
using org.junit.runners.Parameterized;
using org.junit.runners.Parameterized.Parameter;
using org.junit.runners.Parameterized.Parameters;

using aima.core.environment.support.NondeterministicProblemFactory;
using aima.core.environment.vacuum.VELocalState;
using aima.core.environment.vacuum.VEWorldState;
using static aima.core.environment.vacuum.VacuumEnvironment.Status.Clean;
using static aima.core.environment.vacuum.VacuumEnvironment.Status.Dirty;
using aima.core.search.api.ConditionalPlan;
using aima.core.search.api.NondeterministicProblem;
using aima.core.search.api.SearchForConditionalPlanFunction;
using aima.core.search.basic.contingency.AndOrGraphSearch;

@RunWith(Parameterized.class)
public class AndOrGraphSearchTest {

	@Parameters(name = "{index}: {0}")
	public static Collection<Object[]> implementations() {
		return Arrays.asList(new Object[][] { { "AndOrGraphSearch" } });
	}

	@Parameter
	public string searchFunctionName;

	public <A, S> ConditionalPlan<A, S> searchForConditionalPlan(NondeterministicProblem<A, S> problem) {
		SearchForConditionalPlanFunction<A, S> cpSearch = new AndOrGraphSearch<>();

		ConditionalPlan<A, S> cp = cpSearch.apply(problem);

		return cp;
	}

	[TestMethod]
	public void testErraticVacuumWorld() {
		// State 1 [*_/][* ]
		ConditionalPlan<String, VEWorldState> cp;
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("A",
				new VELocalState("A", Dirty), new VELocalState("B", Dirty)));

		Assert.assertEquals("[Suck, if State = [ _/][*  ] then [Right, Suck] else []]", cp.toString());

		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("A", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Suck", null));

		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("A", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Dirty)),
						// Right
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Suck", "Right", "Suck", null));

		// State 2 [* ][*_/]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("B",
				new VELocalState("A", Dirty), new VELocalState("B", Dirty)));

		Assert.assertEquals("[Left, Suck, if State = [ _/][*  ] then [Right, Suck] else []]", cp.toString());

		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("B", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Left
						new VEWorldState("A", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Left", "Suck", null));
		
		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("B", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Left
						new VEWorldState("A", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Dirty)),
						// Right
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Left", "Suck", "Right", "Suck", null));

		// State 3 [*_/][ ]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("A",
				new VELocalState("A", Dirty), new VELocalState("B", Clean)));

		Assert.assertEquals("[Suck]", cp.toString());
		
		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("A", new VELocalState("A", Dirty), new VELocalState("B", Clean)),
						// Suck
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Suck", null));

		// State 4 [* ][ _/]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("B",
				new VELocalState("A", Dirty), new VELocalState("B", Clean)));

		Assert.assertEquals("[Left, Suck]", cp.toString());
		
		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("B", new VELocalState("A", Dirty), new VELocalState("B", Clean)),
						// Left
						new VEWorldState("A", new VELocalState("A", Dirty), new VELocalState("B", Clean)),
						// Suck
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Left", "Suck", null));

		// State 5 [ _/][* ]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("A",
				new VELocalState("A", Clean), new VELocalState("B", Dirty)));

		Assert.assertEquals("[Right, Suck]", cp.toString());
		
		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Dirty)),
						// Right
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Right", "Suck", null));

		// State 6 [ ][*_/]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("B",
				new VELocalState("A", Clean), new VELocalState("B", Dirty)));

		Assert.assertEquals("[Suck]", cp.toString());
		
		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Dirty)),
						// Suck
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList("Suck", null));

		// State 7 [ _/][ ]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("A",
				new VELocalState("A", Clean), new VELocalState("B", Clean)));

		Assert.assertEquals("[]", cp.toString());
		
		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("A", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList((String) null));

		// State 8 [ ][ _/]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("B",
				new VELocalState("A", Clean), new VELocalState("B", Clean)));

		Assert.assertEquals("[]", cp.toString());
		
		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("B", new VELocalState("A", Clean), new VELocalState("B", Clean))),
				Arrays.asList((String) null));
	}
	
	[TestMethod]
	public void testBadStateToConditionalPlan() {
		ConditionalPlan<String, VEWorldState> cp;
		
		// State 2 [* ][*_/]
		cp = searchForConditionalPlan(NondeterministicProblemFactory.getSimpleErraticVacuumWorldProblem("B",
				new VELocalState("A", Dirty), new VELocalState("B", Dirty)));

		Assert.assertEquals("[Left, Suck, if State = [ _/][*  ] then [Right, Suck] else []]", cp.toString());

		testPlan(cp,
				Arrays.asList(// Initial State
						new VEWorldState("B", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Left 
						new VEWorldState("B", new VELocalState("A", Dirty), new VELocalState("B", Dirty)),
						// Suck - had no effect, BAD STATE won't match any of the conditioned plans
						new VEWorldState("B", new VELocalState("A", Dirty), new VELocalState("B", Dirty))),
				Arrays.asList("Left", "Suck", null));
	}

	public <A, S> void testPlan(ConditionalPlan<A, S> cp, List<S> encounteredStates, List<A> expectedActions) {
		List<A> plansActions = new ArrayList<>();
		ConditionalPlan.Interator<A, S> iterator = cp.iterator();

		for (S state : encounteredStates) {
			plansActions.add(iterator.next(state));
		}
		Assert.assertEquals(expectedActions, plansActions);
	}
}
