using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.informed;

namespace tvn.cosine.ai.search.local
{
    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.2, page
    * 122.<br>
    * <br>
    * <p>
    * <pre>
    * function HILL-CLIMBING(problem) returns a state that is a local maximum
    *
    *   current &lt;- MAKE-NODE(problem.INITIAL-STATE)
    *   loop do
    *     neighbor &lt;- a highest-valued successor of current
    *     if neighbor.VALUE &lt;= current.VALUE then return current.STATE
    *     current &lt;- neighbor
    * </pre>
    * <p>
    * Figure 4.2 The hill-climbing search algorithm, which is the most basic local
    * search technique. At each step the current node is replaced by the best
    * neighbor; in this version, that means the neighbor with the highest VALUE,
    * but if a heuristic cost estimate h is used, we would find the neighbor with
    * the lowest h.
    *
    * @author Ravi Mohan
    * @author Mike Stampone
    * @author Ruediger Lunde
    */
    public class HillClimbingSearch<S, A> : SearchForActions<S, A>, SearchForStates<S, A>, Informed<S, A>
    {
        public enum SearchOutcome
        {
            FAILURE, SOLUTION_FOUND
        }

        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_NODE_VALUE = "nodeValue";

        public HeuristicEvaluationFunction<Node<S, A>> h { get; set; }
        private readonly NodeExpander<S, A> nodeExpander;
        private SearchOutcome outcome = SearchOutcome.FAILURE;
        private S lastState;
        private IDictionary<string, double> metrics = new Dictionary<string, double>();

        /**
         * Constructs a hill-climbing search from the specified heuristic function.
         *
         * @param h a heuristic function
         */
        public HillClimbingSearch(HeuristicEvaluationFunction<Node<S, A>> h)
            : this(h, new NodeExpander<S, A>())
        {  }

        public HillClimbingSearch(HeuristicEvaluationFunction<Node<S, A>> h, NodeExpander<S, A> nodeExpander)
        {
            this.h = h;
            this.nodeExpander = nodeExpander;
            nodeExpander.addNodeListener((node) => ++metrics[METRIC_NODES_EXPANDED]);
        }


        public void setHeuristicFunction(HeuristicEvaluationFunction<Node<S, A>> h)
        {
            this.h = h;
        }


        public IList<A> findActions(IProblem<S, A> p)
        {
            nodeExpander.UseParentLinks(true);
            return SearchUtils.toActions(findNode(p));
        }


        public S findState(IProblem<S, A> p)
        {
            nodeExpander.UseParentLinks(false);
            return SearchUtils.toState(findNode(p));
        }

        public Node<S, A> findNode(IProblem<S, A> p)
        {
            return findNode(p, CancellationToken.None);
        }
        /**
         * Returns a node corresponding to a local maximum or empty if the search was
         * cancelled by the user.
         *
         * @param p the search problem
         * @return a node or empty
         */
        // function HILL-CLIMBING(problem) returns a state that is a local maximum
        public Node<S, A> findNode(IProblem<S, A> p, CancellationToken cancellationToken)
        {
            clearMetrics();
            outcome = SearchOutcome.FAILURE;
            // current <- MAKE-NODE(problem.INITIAL-STATE)
            Node<S, A> current = nodeExpander.createRootNode(p.getInitialState());
            Node<S, A> neighbor;
            // loop do
            while (!cancellationToken.IsCancellationRequested)
            {
                lastState = current.getState();
                metrics[METRIC_NODE_VALUE] = getValue(current);
                List<Node<S, A>> children = nodeExpander.expand(current, p);
                // neighbor <- a highest-valued successor of current
                neighbor = getHighestValuedNodeFrom(children);

                // if neighbor.VALUE <= current.VALUE then return current.STATE
                if (neighbor == null || getValue(neighbor) <= getValue(current))
                {
                    if (p.testSolution(current))
                        outcome = SearchOutcome.SOLUTION_FOUND;
                    return current;
                }
                // current <- neighbor
                current = neighbor;
            }
            return null;
        }

        /**
         * Returns SOLUTION_FOUND if the local maximum is a goal state, or FAILURE
         * if the local maximum is not a goal state.
         *
         * @return SOLUTION_FOUND if the local maximum is a goal state, or FAILURE
         * if the local maximum is not a goal state.
         */
        public SearchOutcome getOutcome()
        {
            return outcome;
        }

        /**
         * Returns the last state from which the hill climbing search found the
         * local maximum.
         *
         * @return the last state from which the hill climbing search found the
         * local maximum.
         */
        public S getLastSearchState()
        {
            return lastState;
        }

        /**
         * Returns all the search metrics.
         */
        public IDictionary<string, double> getMetrics()
        {
            return metrics;
        }

        /**
         * Sets all metrics to zero.
         */
        private void clearMetrics()
        {
            metrics[METRIC_NODES_EXPANDED] = 0;
            metrics[METRIC_NODE_VALUE] = 0;
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

        private Node<S, A> getHighestValuedNodeFrom(List<Node<S, A>> children)
        {
            double highestValue = double.NegativeInfinity;
            Node<S, A> nodeWithHighestValue = null;
            foreach (Node<S, A> child in children)
            {
                double value = getValue(child);
                if (value > highestValue)
                {
                    highestValue = value;
                    nodeWithHighestValue = child;
                }
            }
            return nodeWithHighestValue;
        }

        private double getValue(Node<S, A> n)
        {
            // assumption greater heuristic value =>
            // HIGHER on hill; 0 == goal state;
            return -1 * h(n);
        }
    }
}
