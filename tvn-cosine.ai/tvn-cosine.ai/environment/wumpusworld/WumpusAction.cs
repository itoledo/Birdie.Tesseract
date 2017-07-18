using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 237.<br>
     * <br>
     * The agent can move Forward, TurnLeft by 90 degree, or TurnRight by 90 degree. The
     * agent dies a miserable death if it enters a square containing a pit or a live wumpus.
     * If an agent tries to move
     * forward and bumps into a wall, then the agent does not move. The action Grab can be
     * used to pick up the gold if it is in the same square as the agent. The action Shoot can
     * be used to fire an arrow in a straight line in the direction the agent is facing. The arrow
     * continues until it either hits (and hence kills) the wumpus or hits a wall. The agent has
     * only one arrow, so only the first Shoot action has any effect. Finally, the action Climb
     * can be used to climb out of the cave, but only from square [1,1].
     *
     * @author Ruediger Lunde
     */
    public class WumpusAction : Action
    {
        private static readonly ISet<WumpusAction> _values = Factory.CreateSet<WumpusAction>();

        public static readonly WumpusAction FORWARD = new WumpusAction("Forward");
        public static readonly WumpusAction TURN_LEFT = new WumpusAction("TurnLeft");
        public static readonly WumpusAction TURN_RIGHT = new WumpusAction("TurnRight");
        public static readonly WumpusAction GRAB = new WumpusAction("Grab");
        public static readonly WumpusAction SHOOT = new WumpusAction("Shoot");
        public static readonly WumpusAction CLIMB = new WumpusAction("Climb");

        public string getSymbol()
        {
            return symbol;
        }
         
        public bool isNoOp()
        {
            return false;
        }

        public static ISet<WumpusAction> values()
        {
            return _values;
        }

        private string symbol;

        WumpusAction(string sym)
        {
            symbol = sym;
            _values.Add(this);
        }
    }
}
