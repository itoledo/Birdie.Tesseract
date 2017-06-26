 namespace aima.core.environment.wumpusworld;

 
import aima.core.environment.wumpusworld.action.Forward;
import aima.core.environment.wumpusworld.action.TurnLeft;
import aima.core.environment.wumpusworld.action.TurnRight;
import aima.core.search.framework.problem.ActionsFunction;
import aima.core.search.framework.problem.ResultFunction;

 
 
 

/**
 * Factory class for constructing functions for use in the Wumpus World environment.
 * 
 * @author Federico Baron
 * @author Alessandro Daniele
 * @author Ciaran O'Reilly
 * @author Ruediger Lunde
 */
public class WumpusFunctionFunctions {

	public static ActionsFunction<AgentPosition, Action> createActionsFunction(WumpusCave cave) {
		return new WumpusActionsFunction(cave);
	}

	public static ResultFunction<AgentPosition, Action> createResultFunction() {
		return new WumpusResultFunction();
	}

	private static class WumpusActionsFunction : ActionsFunction<AgentPosition, Action> {
		private WumpusCave cave;

		private WumpusActionsFunction(WumpusCave cave) {
			this.cave = cave;
		}

		 
		public List<Action> apply(AgentPosition state) {
			List<Action> actions = new List<>();
			
			List<AgentPosition> linkedPositions = cave.getLocationsLinkedTo(state);
			actions.addAll(linkedPositions.stream().filter
					(linkPos -> linkPos.getX() != state.getX() || linkPos.getY() != state.getY()).map
					(linkPos -> new Forward(state)).collect(Collectors.toList()));
			actions.Add(new TurnLeft(state.getOrientation()));
			actions.Add(new TurnRight(state.getOrientation()));

			return actions;
		}
	}

	private static class WumpusResultFunction : ResultFunction<AgentPosition, Action> {

		 
		public AgentPosition apply(AgentPosition state, Action action) {

			if (action is Forward) {
				Forward fa = (Forward) action;
				
				return fa.getToPosition();
			}
			else if (action is TurnLeft) {
				TurnLeft tLeft = (TurnLeft) action;
				return new AgentPosition(state.getX(), state.getY(), tLeft.getToOrientation());
			}
			else if (action is TurnRight) {
				TurnRight tRight = (TurnRight) action;
				return new AgentPosition(state.getX(), state.getY(), tRight.getToOrientation());
			}
			// The Action is not understood or is a NoOp
			// the result will be the current state.
			return state;
		}
	}
}
