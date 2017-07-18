using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Heuristic for calculating the Manhattan distance between two rooms within a Wumpus World cave.
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     */
    public class ManhattanHeuristicFunction : ToDoubleFunction<Node<AgentPosition, WumpusAction>>
    {
        private IQueue<AgentPosition> goals = Factory.CreateQueue<AgentPosition>();

        public ManhattanHeuristicFunction(ISet<AgentPosition> goals)
        {
            this.goals.AddAll(goals);
        }


        public double applyAsDouble(Node<AgentPosition, WumpusAction> node)
        {
            AgentPosition pos = node.getState();
            int nearestGoalDist = int.MaxValue;
            foreach (AgentPosition g in goals)
            {
                int tmp = evaluateManhattanDistanceOf(pos.getX(), pos.getY(), g.getX(), g.getY());
                if (tmp < nearestGoalDist)
                    nearestGoalDist = tmp;
            }
            return (double)nearestGoalDist;
        }

        //
        // PRIVATE
        //
        private int evaluateManhattanDistanceOf(int x1, int y1, int x2, int y2)
        {
            return System.Math.Abs(x1 - x2) + System.Math.Abs(y1 - y2);
        }
    }
}
