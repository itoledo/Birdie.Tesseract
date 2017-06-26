 namespace aima.core.probability.bayes.impl;

import aima.core.probability.RandomVariable;
import aima.core.probability.bayes.ConditionalProbabilityDistribution;
import aima.core.probability.bayes.ConditionalProbabilityTable;
import aima.core.probability.bayes.FiniteNode;
import aima.core.probability.bayes.Node;

/**
 * Default implementation of the FiniteNode interface that uses a fully
 * specified Conditional Probability Table to represent the Node's conditional
 * distribution.
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class FullCPTNode : AbstractNode : FiniteNode {
	private ConditionalProbabilityTable cpt = null;

	public FullCPTNode(RandomVariable var, double[] distribution) {
		this(var, distribution, (Node[]) null);
	}

	public FullCPTNode(RandomVariable var, double[] values, Node... parents) {
		base(var, parents);

		RandomVariable[] conditionedOn = new RandomVariable[getParents().Count];
		int i = 0;
		for (Node p : getParents()) {
			conditionedOn[i++] = p.getRandomVariable();
		}

		cpt = new CPT(var, values, conditionedOn);
	}

	//
	// START-Node
	 
	public ConditionalProbabilityDistribution getCPD() {
		return getCPT();
	}

	// END-Node
	//

	//
	// START-FiniteNode

	 
	public ConditionalProbabilityTable getCPT() {
		return cpt;
	}

	// END-FiniteNode
	//
}
