using System;
using System.Collections.Generic;
using tvn_cosine.ai.Agents;
using tvn_cosine.ai.Search.Framework.Problems;

namespace tvn_cosine.ai.Search.Framework
{
    /// <summary>
    /// Instances of this class are responsible for node creation and expansion.They
    /// compute path costs, support progress tracing, and count the number of expand(Node, Problem) calls.
    /// </summary>
    public class NodeExpander
    {
        protected bool _useParentLinks = true;

        /// <summary>
        /// Modifies _useParentLinks and returns this node expander.When
        /// using local search to search for states, parent links are not needed and
        /// lead to unnecessary memory consumption.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public NodeExpander useParentLinks(bool state)
        {
            _useParentLinks = state;
            return this;
        }

        ///////////////////////////////////////////////////////////////////////
        // expanding nodes

        /// <summary>
        /// Factory method, which creates a root node for the specified state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public Node createRootNode(IState state)
        {
            return new Node(state);
        }


        /// <summary>
        /// Computes the path cost for getting from the root node state via the
        /// parent node state to the specified state, creates a new node for the
        /// specified state, adds it as child of the provided parent(if  _useParentLinks is true), and returns it.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="parent"></param>
        /// <param name="action"></param>
        /// <param name="stepCost"></param>
        /// <returns></returns>
        public Node createNode(IState state, Node parent, IAction action, double stepCost)
        {
            Node p = _useParentLinks ? parent : null;
            return new Node(state, p, action, parent.getPathCost() + stepCost);
        }

        /// <summary>
        /// Returns the children obtained from expanding the specified node in the specified problem.
        /// </summary>
        /// <param name="node">the node to expand</param>
        /// <param name="problem">the problem the specified node is within.</param>
        /// <returns>the children obtained from expanding the specified node in the specified problem.</returns>
        public List<Node> expand(Node node, Problem problem)
        {
            List<Node> successors = new List<Node>();

            IActionsFunction actionsFunction = problem.getActionsFunction();
            IResultFunction resultFunction = problem.getResultFunction();
            IStepCostFunction stepCostFunction = problem.getStepCostFunction();

            foreach (IAction action in actionsFunction.actions(node.getState()))
            {
                IState successorState = resultFunction.result(node.getState(), action);

                double stepCost = stepCostFunction.c(node.getState(), action, successorState);
                successors.Add(createNode(successorState, node, action, stepCost));
            }
            notifyNodeListeners(node);
            ++counter;
            return successors;
        }

        ///////////////////////////////////////////////////////////////////////
        // progress tracing 
        /// <summary>
        /// All node listeners added to this list get informed whenever a node is expanded.
        /// </summary>
        private List<INodeListener> nodeListeners = new List<INodeListener>();

        /// <summary>
        /// Adds a listener to the list of node listeners. It is informed whenever a node is expanded during search.
        /// </summary>
        /// <param name="listener"></param>
        public void addNodeListener(INodeListener listener)
        {
            nodeListeners.Add(listener);
        }

        protected void notifyNodeListeners(Node node)
        {
            foreach (INodeListener listener in nodeListeners)
            {
                listener.onNodeExpanded(node);
            }
        }

        /// <summary>
        /// Interface for progress Tracers
        /// </summary>
        public interface INodeListener
        {
            void onNodeExpanded(Node node);
        }

        ///////////////////////////////////////////////////////////////////////
        // statistical data

        /// <summary>
        /// Counts the number of {@link #expand(Node, Problem)} calls.
        /// </summary>
        protected int counter;

        /// <summary>
        /// Resets the counter for expand(Node, Problem) calls.
        /// </summary>
        public void resetCounter()
        {
            counter = 0;
        }

        /// <summary>
        /// Returns the number of {@link #expand(Node, Problem)} calls since the last counter reset.
        /// </summary>
        /// <returns></returns>
        public int getNumOfExpandCalls()
        {
            return counter;
        }
    }

}
