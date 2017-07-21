namespace aima.test.unit.environment.wumpusworld;

using java.util.ArrayList;

using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

using aima.core.environment.wumpusworld.AgentPosition;
using aima.core.environment.wumpusworld.WumpusCave;
using aima.core.environment.wumpusworld.WumpusFunctionFactory;
using aima.core.environment.wumpusworld.action.Forward;
using aima.core.environment.wumpusworld.action.TurnLeft;
using aima.core.environment.wumpusworld.action.TurnRight;
using aima.core.environment.wumpusworld.action.WWAction;
using aima.core.search.api.ActionsFunction;
using aima.core.search.api.ResultFunction;

/**
 * 
 * @author Federico Baron
 * @author Alessandro Daniele
 * @author Ciaran O'Reilly
 * @author Anurag Rai
 */
public class WumpusFunctionFactoryTest {

	private ActionsFunction<WWAction, AgentPosition> actionFunction;
	private ResultFunction<WWAction, AgentPosition> resultFunction;

	@Before
	public void setUp() {
		WumpusCave completeCave = new WumpusCave(4, 4);

		actionFunction = WumpusFunctionFactory.getActionsFunction(completeCave);
		resultFunction = WumpusFunctionFactory.getResultFunction();
	}

	[TestMethod]
	public void testSuccessors() {
		ArrayList<AgentPosition> succPositions = new ArrayList<>();
		ArrayList<AgentPosition.Orientation> succOrientation = new ArrayList<>();

		// From every position the possible actions are:
		// - Turn right (change orientation, not position)
		// - Turn left (change orientation, not position)
		// - Forward (change position, not orientation)
		AgentPosition P11U = new AgentPosition(1, 1, AgentPosition.Orientation.FACING_NORTH);
		succPositions.add(new AgentPosition(1, 2, AgentPosition.Orientation.FACING_NORTH));
		succOrientation.add(AgentPosition.Orientation.FACING_EAST);
		succOrientation.add(AgentPosition.Orientation.FACING_WEST);
		for (WWAction a : actionFunction.actions(P11U)) {
			if (a instanceof Forward) {
				Assert.assertTrue(succPositions.contains(((Forward) a).getToPosition()));
				Assert.assertTrue(succPositions.contains(resultFunction.result(P11U, a)));
			} else if (a instanceof TurnLeft) {
				Assert.assertTrue(succOrientation.contains(((TurnLeft) a).getToOrientation()));
				Assert.assertEquals("[1,1]->FacingWest", resultFunction.result(P11U, a).toString());
			} else if (a instanceof TurnRight) {
				Assert.assertTrue(succOrientation.contains(((TurnRight) a).getToOrientation()));
				Assert.assertEquals("[1,1]->FacingEast", resultFunction.result(P11U, a).toString());
			}
		}

		// If you are in front of a wall forward action is not possible
		AgentPosition P31D = new AgentPosition(3, 1, AgentPosition.Orientation.FACING_SOUTH);
		AgentPosition P41R = new AgentPosition(4, 1, AgentPosition.Orientation.FACING_EAST);
		for (WWAction a : actionFunction.actions(P31D)) {
			Assert.assertFalse(a instanceof Forward);
		}

		for (WWAction a : actionFunction.actions(P41R)) {
			Assert.assertFalse(a instanceof Forward);
		}
	}
}
