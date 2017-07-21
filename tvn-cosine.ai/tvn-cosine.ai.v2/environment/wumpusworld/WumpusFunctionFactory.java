namespace aima.core.environment.wumpusworld;

using java.util.LinkedList;
using java.util.List;

using aima.core.environment.wumpusworld.action.Forward;
using aima.core.environment.wumpusworld.action.TurnLeft;
using aima.core.environment.wumpusworld.action.TurnRight;
using aima.core.environment.wumpusworld.action.WWAction;
using aima.core.search.api.ActionsFunction;
using aima.core.search.api.ResultFunction;

/**
 * Factory class for constructing functions for use in the Wumpus World
 * environment.
 * 
 * @author Federico Baron
 * @author Alessandro Daniele
 * @author Ciaran O'Reilly
 * @author Anurag Rai
 */
public class WumpusFunctionFactory {
	private static final ResultFunction<WWAction, AgentPosition> _resultFunction = new WumpusResultFunction();;

	public static ActionsFunction<WWAction, AgentPosition> getActionsFunction(WumpusCave cave) {
		return new WumpusActionsFunction(cave);
	}

	public static ResultFunction<WWAction, AgentPosition> getResultFunction() {
		return _resultFunction;
	}

	private static class WumpusActionsFunction implements ActionsFunction<WWAction, AgentPosition> {
		private WumpusCave cave;

		public WumpusActionsFunction(WumpusCave cave) {
			this.cave = cave;
		}

		 
		public List<WWAction> actions(AgentPosition state) {
			List<WWAction> actions = new LinkedList<>();

			List<AgentPosition> linkedPositions = cave.getLocationsLinkedTo(state);
			for (AgentPosition linkPos : linkedPositions) {
				if (linkPos.getX() != state.getX() || linkPos.getY() != state.getY()) {
					actions.add(new Forward(state));
				}
			}
			actions.add(new TurnLeft(state.getOrientation()));
			actions.add(new TurnRight(state.getOrientation()));

			return actions;
		}
	}

	private static class WumpusResultFunction implements ResultFunction<WWAction, AgentPosition> {
		 
		public AgentPosition result(AgentPosition s, WWAction a) {

			if (a is Forward) {
				Forward fa = (Forward) a;

				return fa.getToPosition();
			} else if (a is TurnLeft) {
				TurnLeft tLeft = (TurnLeft) a;
				AgentPosition res = s;

				return new AgentPosition(res.getX(), res.getY(), tLeft.getToOrientation());
			} else if (a is TurnRight) {
				TurnRight tRight = (TurnRight) a;
				AgentPosition res = s;

				return new AgentPosition(res.getX(), res.getY(), tRight.getToOrientation());
			}

			// The Action is not understood or is a NoOp
			// the result will be the current state.
			return s;
		}
	}
}
