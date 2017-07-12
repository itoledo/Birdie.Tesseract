using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.qsearch;

namespace tvn.cosine.ai.search.informed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 92. 
     *  
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
        public BestFirstSearch(QueueSearch<S, A> impl, HeuristicEvaluationFunction<Node<S, A>> evalFn)
            : base(impl, QueueFactory.createPriorityQueue(Comparer<Node<S, A>>.Default))
        {
            this.h = evalFn;
        }

        /** Modifies the evaluation function if it is a {@link HeuristicEvaluationFunction}. */
        public HeuristicEvaluationFunction<Node<S, A>> h { get; set; }
    } 
}
