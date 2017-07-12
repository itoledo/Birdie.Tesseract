using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.qsearch;

namespace tvn.cosine.ai.search.informed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 93. 
     *  
     * The most widely known form of best-first search is called A* Search
     * (pronounced "A-star search"). It evaluates nodes by combining g(n), the cost
     * to reach the node, and h(n), the cost to get from the node to the goal: 
     * f(n) = g(n) + h(n). 
     *  
     * Since g(n) gives the path cost from the start node to node n, and h(n) is the
     * estimated cost of the cheapest path from n to the goal, we have 
     * f(n) = estimated cost of the cheapest solution through n.
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class AStarSearch<S, A> : BestFirstSearch<S, A>
    {
        /**
         * Constructs an A* search from a specified search space exploration
         * strategy and a heuristic function.
         *
         * @param impl a search space exploration strategy (e.g. TreeSearch, GraphSearch).
         * @param h   a heuristic function <em>h(n)</em>, which estimates the cost
         *             of the cheapest path from the state at node <em>n</em> to a
         *             goal state.
         */
        public AStarSearch(QueueSearch<S, A> impl, HeuristicEvaluationFunction<Node<S, A>> h)
            : base(impl, (node) => h(node) + g(node))
        { }

        public static double g(Node<S,A> node)
        {
            return node.getPathCost();
        }
    }
}