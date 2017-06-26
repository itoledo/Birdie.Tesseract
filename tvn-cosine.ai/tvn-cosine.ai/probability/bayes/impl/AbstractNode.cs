 namespace aima.core.probability.bayes.impl;

 
 
 

import aima.core.probability.RandomVariable;
import aima.core.probability.bayes.ConditionalProbabilityDistribution;
import aima.core.probability.bayes.Node;

/**
 * Abstract base implementation of the Node interface.
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 */
public abstract class AbstractNode : Node {
	private RandomVariable variable = null;
	private ISet<Node> parents = null;
	private ISet<Node> children = null;

	public AbstractNode(RandomVariable var) {
		this(var, (Node[]) null);
	}

	public AbstractNode(RandomVariable var, Node... parents) {
		if (null == var) {
			throw new ArgumentException(
					"Random Variable for Node must be specified.");
		}
		this.variable = var;
		this.parents = new HashSet<Node>();
		if (null != parents) {
			for (Node p : parents) {
				((AbstractNode) p).addChild(this);
				this.parents.Add(p);
			}
		}
		this.parents = new HashSet<>(this.parents);
		this.children = new HashSet<>(new HashSet<Node>());
	}

	//
	// START-Node
	 
	public RandomVariable getRandomVariable() {
		return variable;
	}

	 
	public bool isRoot() {
		return 0 == getParents().Count;
	}

	 
	public ISet<Node> getParents() {
		return parents;
	}

	 
	public ISet<Node> getChildren() {
		return children;
	}

	 
	public ISet<Node> getMarkovBlanket() {
		LinkedHashSet<Node> mb = new HashSet<Node>();
		// Given its parents,
		mb.addAll(getParents());
		// children,
		mb.addAll(getChildren());
		// and children's parents
		for (Node cn : getChildren()) {
			mb.addAll(cn.getParents());
		}

		return mb;
	}

	public abstract ConditionalProbabilityDistribution getCPD();

	// END-Node
	//

	 
	public override string ToString() {
		return getRandomVariable().getName();
	}

	 
	public override bool Equals(object o) {
		if (null == o) {
			return false;
		}
		if (o == this) {
			return true;
		}

		if (o is Node) {
			Node n = (Node) o;

			return getRandomVariable() .Equals(n.getRandomVariable());
		}

		return false;
	}

	 
	public override int GetHashCode() {
		return variable .GetHashCode();
	}

	//
	// PROTECTED METHODS
	//
	protected void addChild(Node childNode) {
		children = new HashSet<Node>(children);

		children.Add(childNode);

		children = new HashSet<>(children);
	}
}
