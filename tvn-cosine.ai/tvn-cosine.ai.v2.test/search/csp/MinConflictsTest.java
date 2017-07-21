namespace aima.test.unit.search.csp;

using java.util.Arrays;
using java.util.Collection;

using org.junit.Assert;
using org.junit.Test;
using org.junit.runner.RunWith;
using org.junit.runners.Parameterized;
using org.junit.runners.Parameterized.Parameter;
using org.junit.runners.Parameterized.Parameters;

using aima.core.environment.support.CSPFactory;
using aima.core.search.api.Assignment;
using aima.core.search.api.CSP;
using aima.core.search.api.SearchForAssignmentFunction;
using aima.core.search.basic.csp.MinConflicts;

@RunWith(Parameterized.class)
public class MinConflictsTest {

	@Parameters(name = "{index}: {0}")
	public static Collection<Object[]> implementations() {
		return Arrays.asList(new Object[][] { { "DefaultMinConflicts" }});
	}

	@Parameter
	public string searchFunctionName;

	public Assignment searchForAssignment(CSP csp) {
		SearchForAssignmentFunction searchFn;
		switch (searchFunctionName) {
		case "DefaultMinConflicts":
			MinConflicts mc = new MinConflicts();
			
			searchFn = mc;
			break;
		default:
			throw new IllegalArgumentException(searchFunctionName + " not handled properly.");
		}

		return searchFn.apply(csp);
	}

	[TestMethod]
	public void testMapColoringTerritoriesOfAustraliaCSP() {
		CSP csp = CSPFactory.mapColoringTerritoriesOfAustraliaCSP();

		Assignment ans = searchForAssignment(csp);

		Assert.assertTrue(ans.isComplete(csp));
		Assert.assertNotEquals(ans.getAssignment("SA"), ans.getAssignment("WA"));
		Assert.assertNotEquals(ans.getAssignment("SA"), ans.getAssignment("NT"));
		Assert.assertNotEquals(ans.getAssignment("SA"), ans.getAssignment("Q"));
		Assert.assertNotEquals(ans.getAssignment("SA"), ans.getAssignment("NSW"));
		Assert.assertNotEquals(ans.getAssignment("SA"), ans.getAssignment("V"));
		Assert.assertNotEquals(ans.getAssignment("WA"), ans.getAssignment("NT"));
		Assert.assertNotEquals(ans.getAssignment("NT"), ans.getAssignment("Q"));
		Assert.assertNotEquals(ans.getAssignment("Q"), ans.getAssignment("NSW"));
		Assert.assertNotEquals(ans.getAssignment("NSW"), ans.getAssignment("V"));
	}
}