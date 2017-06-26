 namespace aima.core.probability.bayes.impl;

 
 
 
import java.util.HashSet;
 
 
 
 

import aima.core.probability.RandomVariable;
import aima.core.probability.bayes.BayesianNetwork;
import aima.core.probability.bayes.Node;

/**
 * Default implementation of the BayesianNetwork interface.
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 */
public class BayesNet : BayesianNetwork {
	protected ISet<Node> rootNodes = new HashSet<Node>();
	protected List<RandomVariable> variables = new List<RandomVariable>();
	protected IDictionary<RandomVariable, Node> varToNodeMap = new Dictionary<RandomVariable, Node>();

	public BayesNet(Node... rootNodes) {
		if (null == rootNodes) {
			throw new ArgumentException(
					"Root Nodes need to be specified.");
		}
		for (Node n : rootNodes) {
			this.rootNodes.Add(n);
		}
		if (this.rootNodes.Count != rootNodes.Length) {
			throw new ArgumentException(
					"Duplicate Root Nodes Passed in.");
		}
		// Ensure is a DAG
		checkIsDAGAndCollectVariablesInTopologicalOrder();
		variables = Collections.unmodifiableList(variables);
	}

	//
	// START-BayesianNetwork
	 
	public List<RandomVariable> getVariablesInTopologicalOrder() {
		return variables;
	}

	 
	public Node getNode(RandomVariable rv) {
		return varToNodeMap.get(rv);
	}

	// END-BayesianNetwork
	//

	//
	// PRIVATE METHODS
	//
	private void checkIsDAGAndCollectVariablesInTopologicalOrder() {

		// Topological sort based on logic described at:
		// http://en.wikipedia.org/wiki/Topoligical_sorting
		Set<Node> seenAlready = new HashSet<Node>();
		IDictionary<Node, List<Node>> incomingEdges = new Dictionary<Node, List<Node>>();
		Set<Node> s = new HashSet<Node>();
		for (Node n : this.rootNodes) {
			walkNode(n, seenAlready, incomingEdges, s);
		}
		while (!s.isEmpty()) {
			Node n = s.iterator().next();
			s.Remove(n);
			variables.Add(n.getRandomVariable());
			varToNodeMap.Add(n.getRandomVariable(), n);
			for (Node m : n.getChildren()) {
				List<Node> edges = incomingEdges.get(m);
				edges.Remove(n);
				if (edges.isEmpty()) {
					s.Add(m);
				}
			}
		}

		for (List<Node> edges : incomingEdges.values()) {
			if (!edges.isEmpty()) {
				throw new ArgumentException(
						"Network contains at least one cycle in it, must be a DAG.");
			}
		}
	}

	private void walkNode(Node n, ISet<Node> seenAlready,
			IDictionary<Node, List<Node>> incomingEdges, ISet<Node> rootNodes) {
		if (!seenAlready.Contains(n)) {
			seenAlready.Add(n);
			// Check if has no incoming edges
			if (n.isRoot()) {
				rootNodes.Add(n);
			}
			incomingEdges.Add(n, new List<Node>(n.getParents()));
			for (Node c : n.getChildren()) {
				walkNode(c, seenAlready, incomingEdges, rootNodes);
			}
		}
	}
}
