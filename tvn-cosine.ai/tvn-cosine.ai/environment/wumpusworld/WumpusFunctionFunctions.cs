using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.wumpusworld.action;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Factory class for constructing functions for use in the Wumpus World environment.
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     */
    public class WumpusFunctionFunctions
    {

        public static ActionsFunction<AgentPosition, IAction> createActionsFunction(WumpusCave cave)
        {
            return (state) =>
            {
                List<IAction> actions = new List<IAction>();

                IList<AgentPosition> linkedPositions = cave.getLocationsLinkedTo(state);
                actions.AddRange(linkedPositions.Where(linkPos => linkPos.getX() != state.getX() || linkPos.getY() != state.getY())
                    .Select(linkPos => new Forward(state)).ToList());
                actions.Add(new TurnLeft(state.getOrientation()));
                actions.Add(new TurnRight(state.getOrientation()));

                return actions;
            };
        }

        public static ResultFunction<AgentPosition, IAction> createResultFunction()
        {
            return (state, action) =>
            {
                if (action is Forward)
                {
                    Forward fa = (Forward)action;

                    return fa.getToPosition();
                }
                else if (action is TurnLeft)
                {
                    TurnLeft tLeft = (TurnLeft)action;
                    return new AgentPosition(state.getX(), state.getY(), tLeft.getToOrientation());
                }
                else if (action is TurnRight)
                {
                    TurnRight tRight = (TurnRight)action;
                    return new AgentPosition(state.getX(), state.getY(), tRight.getToOrientation());
                }
                // The Action is not understood or is a NoOp
                // the result will be the current state.
                return state;
            };
        }
    }

}
