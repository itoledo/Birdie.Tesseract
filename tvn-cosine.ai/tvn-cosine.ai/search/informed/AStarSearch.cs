﻿using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.search.informed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 93.<br>
     * <br>
     * The most widely known form of best-first search is called A* Search
     * (pronounced "A-star search"). It evaluates nodes by combining g(n), the cost
     * to reach the node, and h(n), the cost to get from the node to the goal:<br>
     * f(n) = g(n) + h(n).<br>
     * <br>
     * Since g(n) gives the path cost from the start node to node n, and h(n) is the
     * estimated cost of the cheapest path from n to the goal, we have<br>
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
        public AStarSearch(QueueSearch<S, A> impl, ToDoubleFunction<Node<S, A>> h)
            : base(impl, new EvalFunction(h))
        { }

        public class EvalFunction : HeuristicEvaluationFunction<S, A>, ToDoubleFunction<Node<S, A>>
        {
            public EvalFunction(ToDoubleFunction<Node<S, A>> h)
            {
                this.h = h; 
            }

            /**
             * Returns <em>g(n)</em> the cost to reach the node, plus <em>h(n)</em> the
             * heuristic cost to get from the specified node to the goal.
             *
             * @param n a node
             * @return g(n) + h(n)
             */

            public double applyAsDouble(Node<S, A> n)
            {
                // f(n) = g(n) + h(n)
                return n.getPathCost() + h.applyAsDouble(n);
            }
        }
    }
}
