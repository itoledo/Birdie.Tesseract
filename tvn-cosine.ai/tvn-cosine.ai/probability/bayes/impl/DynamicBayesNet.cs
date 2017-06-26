 namespace aima.core.probability.bayes.impl;

 
 
 
 
 
 

import aima.core.probability.RandomVariable;
import aima.core.probability.bayes.BayesianNetwork;
import aima.core.probability.bayes.DynamicBayesianNetwork;
import aima.core.probability.bayes.Node;
import aima.core.util.SetOps;

/**
 * Default implementation of the DynamicBayesianNetwork interface.
 * 
 * @author Ciaran O'Reilly
 */
public class DynamicBayesNet : BayesNet : DynamicBayesianNetwork {

	private ISet<RandomVariable> X_0 = new HashSet<RandomVariable>();
	private ISet<RandomVariable> X_1 = new HashSet<RandomVariable>();
	private ISet<RandomVariable> E_1 = new HashSet<RandomVariable>();
	private IDictionary<RandomVariable, RandomVariable> X_0_to_X_1 = new Dictionary<RandomVariable, RandomVariable>();
	private IDictionary<RandomVariable, RandomVariable> X_1_to_X_0 = new Dictionary<RandomVariable, RandomVariable>();
	private BayesianNetwork priorNetwork = null;
	private List<RandomVariable> X_1_VariablesInTopologicalOrder = new List<RandomVariable>();

	public DynamicBayesNet(BayesianNetwork priorNetwork,
			IDictionary<RandomVariable, RandomVariable> X_0_to_X_1,
			Set<RandomVariable> E_1, Node... rootNodes) {
		base(rootNodes);

		for (Map.Entry<RandomVariable, RandomVariable> x0_x1 : X_0_to_X_1
				.entrySet()) {
			RandomVariable x0 = x0_x1.Key;
			RandomVariable x1 = x0_x1.getValue();
			this.X_0.Add(x0);
			this.X_1.Add(x1);
			this.X_0_to_X_1.Add(x0, x1);
			this.X_1_to_X_0.Add(x1, x0);
		}
		this.E_1.addAll(E_1);

		// Assert the X_0, X_1, and E_1 sets are of expected sizes
		Set<RandomVariable> combined = new HashSet<RandomVariable>();
		combined.addAll(X_0);
		combined.addAll(X_1);
		combined.addAll(E_1);
		if (SetOps.difference(varToNodeMap.Keys, combined).Count != 0) {
			throw new ArgumentException(
					"X_0, X_1, and E_1 do not map correctly to the Nodes describing this Dynamic Bayesian Network.");
		}
		this.priorNetwork = priorNetwork;

		X_1_VariablesInTopologicalOrder
				.addAll(getVariablesInTopologicalOrder());
		X_1_VariablesInTopologicalOrder.removeAll(X_0);
		X_1_VariablesInTopologicalOrder.removeAll(E_1);
	}

	//
	// START-DynamicBayesianNetwork
	 
	public BayesianNetwork getPriorNetwork() {
		return priorNetwork;
	}

	 
	public ISet<RandomVariable> getX_0() {
		return X_0;
	}

	 
	public ISet<RandomVariable> getX_1() {
		return X_1;
	}

	 
	public List<RandomVariable> getX_1_VariablesInTopologicalOrder() {
		return X_1_VariablesInTopologicalOrder;
	}

	 
	public IDictionary<RandomVariable, RandomVariable> getX_0_to_X_1() {
		return X_0_to_X_1;
	}

	 
	public IDictionary<RandomVariable, RandomVariable> getX_1_to_X_0() {
		return X_1_to_X_0;
	}

	 
	public ISet<RandomVariable> getE_1() {
		return E_1;
	}

	// END-DynamicBayesianNetwork
	//

	//
	// PRIVATE METHODS
	//
}
