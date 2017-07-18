using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Factory class for constructing functions for use in the Wumpus World environment.
     *
     * @author Ruediger Lunde
     */
    public class WumpusFunctions
    {

        public static ActionsFunction<AgentPosition, WumpusAction> createActionsFunction(WumpusCave cave)
        {
            return (state) =>
            {
                IQueue<WumpusAction> actions = Factory.CreateQueue<WumpusAction>();

                AgentPosition pos = cave.moveForward(state);
                if (!pos.Equals(state))
                    actions.Add(WumpusAction.FORWARD);

                actions.Add(WumpusAction.TURN_LEFT);
                actions.Add(WumpusAction.TURN_RIGHT);

                return actions;
            };
        }

        public static ResultFunction<AgentPosition, WumpusAction> createResultFunction(WumpusCave cave)
        {
            return (state, action) =>
            {
                AgentPosition result = state;
                if (action == WumpusAction.FORWARD)
                {
                    result = cave.moveForward(state);
                }
                else if (action == WumpusAction.TURN_LEFT)
                {
                    result = cave.turnLeft(state);
                }
                else if (action == WumpusAction.TURN_RIGHT)
                {
                    result = cave.turnRight(state);
                }
                return result;
            };
        } 
    }
}
