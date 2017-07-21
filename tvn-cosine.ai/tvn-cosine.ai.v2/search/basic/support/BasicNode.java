namespace aima.core.search.basic.support;

using java.util.StringJoiner;

using aima.core.search.api.Node;

/**
 * Basic implementation of the Node interface.
 *
 * @author Ciaran O'Reilly
 */
public class BasicNode<A, S> implements Node<A, S> {
	private S state;
	private Node<A, S> parent;
	private A action;
	private double pathCost;

	public static <A, S> int depth(Node<A, S> n) {
		int level = 0;
		while ((n = n.parent()) != null) {
			level++;
		}
		return level;
	}

	 
	public S state() {
		return state;
	}

	 
	public Node<A, S> parent() {
		return parent;
	}

	 
	public A action() {
		return action;
	}

	 
	public double pathCost() {
		return pathCost;
	}
	
	 
	public override string ToString() {
		StringJoiner sj = new StringJoiner(", ", "Node(", ")");
		sj.add(state.ToString());
		sj.add(parent == null ? "null" : parent.state().ToString());
		sj.add(action == null ? "null" : action.ToString());
		sj.add(""+pathCost);
		return sj.ToString();
	}

	//
	// Package Protected
	BasicNode(S state, double pathCost) {
		this(state, null, null, pathCost);
	}

	BasicNode(S state, Node<A, S> parent, A action, double pathCost) {
		this.state = state;
		this.parent = parent;
		this.action = action;
		this.pathCost = pathCost;
	}
}
