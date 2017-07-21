namespace aima.test.unit.agent;

using aima.core.agent.api.Rule;
using aima.core.agent.basic.SimpleReflexAgent;
using aima.test.unit.agent.support.TestPercept;
using aima.test.unit.agent.support.TestState;
using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

using java.util.Arrays;
using java.util.function.Predicate;

/**
 * @author Ciaran O'Reilly
 */
public class SimpleReflexAgentTest {

	private SimpleReflexAgent<String, TestPercept, TestState> agent;

	@Before
	public void setUp() {
		agent = new SimpleReflexAgent<>(Arrays.asList(new Rule<String, TestState>() {
			public Predicate<TestState> condition() {
				return state -> state.dirty;
			}

			public string action() {
				return "Sweep";
			}
		}, new Rule<String, TestState>() {
			public Predicate<TestState> condition() {
				return state -> !state.dirty;
			}

			public string action() {
				return "Drink Tea";
			}
		}), percept -> new TestState(percept.location, percept.floorIsDirty));
	}

	[TestMethod]
	public void testSweep() {
		Assert.assertEquals("Sweep", agent.perceive(new TestPercept("A", true)));
	}

	[TestMethod]
	public void tetDrinkTea() {
		Assert.assertEquals("Drink Tea", agent.perceive(new TestPercept("A", false)));
	}
}
