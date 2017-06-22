using System.Collections.Generic;
using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach(3rd Edition): Figure 3.10, page 79. 
    ///
    /// Figure 3.10 Nodes are the data structures from which the search tree is
    /// constructed.Each has a parent, a state, and various bookkeeping fields.
    /// Arrows point from child to parent. 
    /// 
    /// Search algorithms require a data structure to keep track of the search tree
    /// that is being constructed. For each node n of the tree, we have a structure
    /// that contains four components: 
    /// * n.STATE: the state in the state space to which the node corresponds; 
    /// * n.PARENT: the node in the search tree that generated this node; 
    /// * n.ACTION: the action that was applied to the parent to generate the node; 
    /// * n.PATH-COST: the cost, traditionally denoted by g(n), of the path from the initial state to the node, as indicated by the parent pointers.  
    /// </summary>
    public class Node
    {
        /// <summary>
        /// n.STATE: the state in the state space to which the node corresponds;
        /// </summary>
        private IState state;

        /// <summary>
        /// n.PARENT: the node in the search tree that generated this node;
        /// </summary>
        private Node parent;

        /// <summary>
        /// n.ACTION: the action that was applied to the parent to generate the node;
        /// </summary>
        private IAction action;

        /// <summary>
        /// n.PATH-COST: the cost, traditionally denoted by g(n), of the path from the initial state to the node, as indicated by the parent pointers.
        /// </summary>
        private double pathCost;
         
        /// <summary>
        /// Constructs a node with the specified state.
        /// </summary>
        /// <param name="state">the state in the state space to which the node corresponds.</param>
        public Node(IState state)
        {
            this.state = state;
            pathCost = 0.0;
        }
         
        /// <summary>
        /// Constructs a node with the specified state, parent, action, and path cost.
        /// </summary>
        /// <param name="state">the state in the state space to which the node corresponds.</param>
        /// <param name="parent">the node in the search tree that generated the node.</param>
        /// <param name="action">the action that was applied to the parent to generate the node.</param>
        /// <param name="pathCost">full pathCost from the root node to here, typically the root's path costs plus the step costs for executing the the specified action.</param>
        public Node(IState state, Node parent, IAction action, double pathCost)
            : this(state)
        {
            this.parent = parent;
            this.action = action;
            this.pathCost = pathCost;
        }
         
        /// <summary>
        /// Returns the state in the state space to which the node corresponds.
        /// </summary>
        /// <returns>the state in the state space to which the node corresponds.</returns>
        public IState getState()
        {
            return state;
        }
         
        /// <summary>
        /// Returns this node's parent node, from which this node was generated.
        /// </summary>
        /// <returns>the node's parenet node, from which this node was generated.</returns>
        public Node getParent()
        {
            return parent;
        }
         
        /// <summary>
        /// Returns the action that was applied to the parent to generate the node.
        /// </summary>
        /// <returns>the action that was applied to the parent to generate the node.</returns>
        public IAction getAction()
        {
            return action;
        }
         
        /// <summary>
        /// Returns the cost of the path from the initial state to this node as indicated by the parent pointers.
        /// </summary>
        /// <returns>the cost of the path from the initial state to this node as indicated by the parent pointers.</returns>
        public double getPathCost()
        {
            return pathCost;
        }
         
        /// <summary>
        /// Returns true if the node has no parent.
        /// </summary>
        /// <returns>true if the node has no parent.</returns>
        public bool isRootNode()
        {
            return parent == null;
        }
         
        /// <summary>
        /// Returns the path from the root node to this node.
        /// </summary>
        /// <returns>the path from the root node to this node.</returns>
        public List<Node> getPathFromRoot()
        {
            List<Node> path = new List<Node>();
            Node current = this;
            while (!current.isRootNode())
            {
                path.Insert(0, current);
                current = current.getParent();
            }
            // ensure the root node is added
            path.Insert(0, current);
            return path;
        }

        public override string ToString()
        {
            return string.Format("{{parent: \"{0}\", action: \"{1}\", state=\"{2}\", pathCost=\"{3}\"}}",
                                           parent, action, state, pathCost);
        }
    }
}
