using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.search.informed
{
    /**
       * Artificial Intelligence A Modern Approach (3rd Edition): page 92.<br>
       * <br>
       * Best-first search is an instance of the general TREE-SEARCH or GRAPH-SEARCH
       * algorithm in which a node is selected for expansion based on an evaluation
       * function, f(n). The evaluation function is construed as a cost estimate, so
       * the node with the lowest evaluation is expanded first. The implementation of
       * best-first graph search is identical to that for uniform-cost search (Figure
       * 3.14), except for the use of f instead of g to order the priority queue.
       *
       * @author Ruediger Lunde
       * @author Ciaran O'Reilly
       * @author Mike Stampone
       */
    public class BestFirstSearch<S, A> : QueueBasedSearch<S, A>, Informed<S, A>
    {
        private readonly ToDoubleFunction<Node<S, A>> evalFn;

        class BestFirstSearchComparer : IComparer<Node<S, A>>
        {
            ToDoubleFunction<Node<S, A>> evalFn;
            System.Collections.Generic.Comparer<double> comparer = System.Collections.Generic.Comparer<double>.Default;

            public BestFirstSearchComparer(ToDoubleFunction<Node<S, A>> evalFn)
            {
                this.evalFn = evalFn;
            }

            public int Compare(Node<S, A> x, Node<S, A> y)
            {
                return comparer.Compare(evalFn.applyAsDouble(x), evalFn.applyAsDouble(y));
            }
        }

        /**
         * Constructs a best first search from a specified search problem and
         * evaluation function.
         * 
         * @param impl
         *            a search space exploration strategy.
         * @param evalFn
         *            an evaluation function, which returns a number purporting to
         *            describe the desirability (or lack thereof) of expanding a
         *            node.
         */
        public BestFirstSearch(QueueSearch<S, A> impl, ToDoubleFunction<Node<S, A>> evalFn)
            : base(impl, CollectionFactory.CreatePriorityQueue<Node<S, A>>(new BestFirstSearchComparer(evalFn)))
        {
            this.evalFn = evalFn;
        }

        /** Modifies the evaluation function if it is a {@link HeuristicEvaluationFunction}. */
        public void setHeuristicFunction(ToDoubleFunction<Node<S, A>> h)
        {
            if (evalFn is HeuristicEvaluationFunction<S, A>)
                ((HeuristicEvaluationFunction<S, A>)evalFn).setHeuristicFunction(h);
        }
    }
}
