using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.api;

namespace tvn.cosine.ai.search.informed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 92.<br>
     * <br>
     * Greedy best-first search tries to expand the node that is closest to the
     * goal, on the grounds that this is likely to lead to a solution quickly. Thus,
     * it evaluates nodes by using just the heuristic function; that is, f(n) = h(n)
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class GreedyBestFirstSearch<S, A> : BestFirstSearch<S, A>
    {

        /**
         * Constructs a greedy best-first search from a specified search space
         * exploration strategy and a heuristic function.
         * 
         * @param impl
         *            a search space exploration strategy (e.g. TreeSearch,
         *            GraphSearch).
         * @param h
         *            a heuristic function <em>h(n)</em>, which estimates the
         *            cheapest path from the state at node <em>n</em> to a goal
         *            state.
         */
        public GreedyBestFirstSearch(QueueSearch<S, A> impl, IToDoubleFunction<Node<S, A>> h)
                : base(impl, new EvalFunction(h))
        { }

        public class EvalFunction : HeuristicEvaluationFunction<S, A>
        {
            public EvalFunction(IToDoubleFunction<Node<S, A>> h)
            {
                this.h = h;
            }

            /**
             * Returns the heuristic cost <em>h(n)</em> to get from the specified node to the goal.
             *
             * @param n a node
             * @return h(n)
             */

            public override double applyAsDouble(Node<S, A> n)
            {
                // f(n) = h(n)
                return h.applyAsDouble(n);
            }
        }
    }
}
