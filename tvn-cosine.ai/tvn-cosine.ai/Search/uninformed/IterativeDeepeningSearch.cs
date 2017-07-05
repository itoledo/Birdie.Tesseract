using System;
using System.Collections.Generic;
using System.Threading;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.search.uninformed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.18, page
     * 89.<br>
     * <br>
     * 
     * <pre>
     * function ITERATIVE-DEEPENING-SEARCH(problem) returns a solution, or failure
     *   for depth = 0 to infinity  do
     *     result &lt;- DEPTH-LIMITED-SEARCH(problem, depth)
     *     if result != cutoff then return result
     * </pre>
     * 
     * Figure 3.18 The iterative deepening search algorithm, which repeatedly
     * applies depth-limited search with increasing limits. It terminates when a
     * solution is found or if the depth- limited search returns failure, meaning
     * that no solution exists.
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class IterativeDeepeningSearch<S, A> : SearchForActions<S, A>, SearchForStates<S, A>
    {
        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_PATH_COST = "pathCost";

        private readonly NodeExpander<S, A> nodeExpander;
        private readonly IDictionary<string, double> metrics;

        public IterativeDeepeningSearch()
            : this(new NodeExpander<S, A>())
        { }

        public IterativeDeepeningSearch(NodeExpander<S, A> nodeExpander)
        {
            this.nodeExpander = nodeExpander;
            this.metrics = new Dictionary<string, double>();
        }


        // function ITERATIVE-DEEPENING-SEARCH(problem) returns a solution, or
        // failure

        public List<A> findActions(IProblem<S, A> p)
        {
            nodeExpander.UseParentLinks(true);
            return SearchUtils.toActions(findNode(p));
        }


        public S findState(IProblem<S, A> p)
        {
            nodeExpander.UseParentLinks(false);
            return SearchUtils.toState(findNode(p));
        }

        private Node<S, A> findNode(IProblem<S, A> p)
        {
            return findNode(p, CancellationToken.None);
        }
        /**
         * Returns a solution node if a solution was found, empty if no solution is reachable or the task was cancelled
         * by the user.
         * @param p
         * @return
         */
        private Node<S, A> findNode(IProblem<S, A> p, CancellationToken cancellationToken)
        {
            clearMetrics();
            // for depth = 0 to infinity do
            for (int i = 0; !cancellationToken.IsCancellationRequested; i++)
            {
                // result <- DEPTH-LIMITED-SEARCH(problem, depth)
                DepthLimitedSearch<S, A> dls = new DepthLimitedSearch<S, A>(i, nodeExpander);
                Node<S, A> result = dls.findNode(p);
                updateMetrics(dls.getMetrics());
                // if result != cutoff then return result
                if (!dls.isCutoffResult(result))
                    return result;
            }
            return null;
        }


        public IDictionary<string, double> getMetrics()
        {
            return metrics;
        }


        public void addNodeListener(Action<Node<S, A>> listener)
        {
            nodeExpander.addNodeListener(listener);
        }


        public bool removeNodeListener(Action<Node<S, A>> listener)
        {
            return nodeExpander.removeNodeListener(listener);
        }


        //
        // PRIVATE METHODS
        //

        /**
         * Sets the nodes expanded and path cost metrics to zero.
         */
        private void clearMetrics()
        {
            metrics[METRIC_NODES_EXPANDED] = 0;
            metrics[METRIC_PATH_COST] = 0;
        }

        private void updateMetrics(IDictionary<string, double> dlsMetrics)
        {
            metrics[METRIC_NODES_EXPANDED] = metrics[METRIC_NODES_EXPANDED] + dlsMetrics[METRIC_NODES_EXPANDED];
            metrics.Add(METRIC_PATH_COST, dlsMetrics[METRIC_PATH_COST]);
        }
    }
}
