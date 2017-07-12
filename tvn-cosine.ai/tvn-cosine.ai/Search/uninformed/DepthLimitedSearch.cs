using System;
using System.Collections.Generic;
using System.Threading;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.search.uninformed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.17, page
     * 88. 
     *  
     * 
     * <pre>
     * function DEPTH-LIMITED-SEARCH(problem, limit) returns a solution, or failure/cutoff
     *   return RECURSIVE-DLS(MAKE-NODE(problem.INITIAL-STATE), problem, limit)
     *   
     * function RECURSIVE-DLS(node, problem, limit) returns a solution, or failure/cutoff
     *   if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
     *   else if limit = 0 then return cutoff
     *   else
     *       cutoff_occurred? &lt;- false
     *       for each action in problem.ACTIONS(node.STATE) do
     *           child &lt;- CHILD-NODE(problem, node, action)
     *           result &lt;- RECURSIVE-DLS(child, problem, limit - 1)
     *           if result = cutoff then cutoff_occurred? &lt;- true
     *           else if result != failure then return result
     *       if cutoff_occurred? then return cutoff else return failure
     * </pre>
     * 
     * Figure 3.17 A recursive implementation of depth-limited search.
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class DepthLimitedSearch<S, A> : SearchForActions<S, A>, SearchForStates<S, A>
    {
        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_PATH_COST = "pathCost";

        public readonly Node<S, A> cutoffNode = new Node<S, A>(default(S));
        private readonly int limit;
        private readonly NodeExpander<S, A> nodeExpander;
        private IDictionary<string, double> metrics = new Dictionary<string, double>();

        public DepthLimitedSearch(int limit)
            : this(limit, new NodeExpander<S, A>())
        { }

        public DepthLimitedSearch(int limit, NodeExpander<S, A> nodeExpander)
        {
            this.limit = limit;
            this.nodeExpander = nodeExpander;
        }

        // function DEPTH-LIMITED-SEARCH(problem, limit) returns a solution, or
        // failure/cutoff
        /**
         * Returns a list of actions to reach the goal if a goal was found, or empty.
         * The list itself can be empty if the initial state is a goal state.
         * 
         * @return if goal found, the list of actions to the goal, empty otherwise.
         */

        public IList<A> findActions(IProblem<S, A> p)
        {
            nodeExpander.UseParentLinks(true);
            Node<S, A> node = findNode(p);
            return !isCutoffResult(node) ? SearchUtils.toActions(node) : null;
        }


        public S findState(IProblem<S, A> p)
        {
            nodeExpander.UseParentLinks(false);
            Node<S, A> node = findNode(p);
            return !isCutoffResult(node) ? SearchUtils.toState(node) : default(S);
        }

        public Node<S, A> findNode(IProblem<S, A> p)
        {
            clearMetrics();
            // return RECURSIVE-DLS(MAKE-NODE(INITIAL-STATE[problem]), problem,
            // limit)
            Node<S, A> node = recursiveDLS(nodeExpander.createRootNode(p.getInitialState()), p, limit);
            return node != null ? node : null;
        }

        // function RECURSIVE-DLS(node, problem, limit) returns a solution, or
        // failure/cutoff

        private Node<S, A> recursiveDLS(Node<S, A> node, IProblem<S, A> problem, int limit)
        {
            return recursiveDLS(node, problem, limit, CancellationToken.None);
        }
        /**
         * Returns a solution node, the {@link #cutoffNode}, or null (failure).
         */
        private Node<S, A> recursiveDLS(Node<S, A> node, IProblem<S, A> problem, int limit, CancellationToken cancellationToken)
        {
            // if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
            if (problem.testSolution(node))
            {
                metrics[METRIC_PATH_COST] = node.getPathCost();
                return node;
            }
            else if (0 == limit || cancellationToken.IsCancellationRequested)
            {
                // else if limit = 0 then return cutoff
                return cutoffNode;
            }
            else
            {
                // else
                // cutoff_occurred? <- false
                bool cutoffOccurred = false;
                // for each action in problem.ACTIONS(node.STATE) do
                ++metrics[METRIC_NODES_EXPANDED];
                foreach (Node<S, A> child in nodeExpander.expand(node, problem))
                {
                    // child <- CHILD-NODE(problem, node, action)
                    // result <- RECURSIVE-DLS(child, problem, limit - 1)
                    Node<S, A> result = recursiveDLS(child, problem, limit - 1);
                    // if result = cutoff then cutoff_occurred? <- true
                    if (result == cutoffNode)
                    {
                        cutoffOccurred = true;
                    }
                    else if (result != null)
                    {
                        // else if result != failure then return result
                        return result;
                    }
                }
                // if cutoff_occurred? then return cutoff else return failure
                return cutoffOccurred ? cutoffNode : null;
            }
        }

        public bool isCutoffResult(Node<S, A> node)
        {
            return node != null && node == cutoffNode;
        }

        /**
         * Returns all the search metrics.
         */

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

        /**
         * Sets the nodes expanded and path cost metrics to zero.
         */
        private void clearMetrics()
        {
            metrics[METRIC_NODES_EXPANDED] = 0;
            metrics[METRIC_PATH_COST] = 0;
        }
    }

}
