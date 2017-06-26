 namespace aima.core.probability.bayes.exact;

 
 
 

import aima.core.probability.CategoricalDistribution;
import aima.core.probability.RandomVariable;
import aima.core.probability.bayes.BayesInference;
import aima.core.probability.bayes.BayesianNetwork;
import aima.core.probability.bayes.FiniteNode;
import aima.core.probability.bayes.Node;
import aima.core.probability.domain.FiniteDomain;
import aima.core.probability.proposition.AssignmentProposition;
import aima.core.probability.util.ProbabilityTable;
 

/**
 * Artificial Intelligence A Modern Approach (3rd Edition): Figure 14.9, page
 * 525.<br>
 * <br>
 * 
 * <pre>
 * function ENUMERATION-ASK(X, e, bn) returns a distribution over X
 *   inputs: X, the query variable
 *           e, observed values for variables E
 *           bn, a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden variables //
 *           
 *   Q(X) <- a distribution over X, initially empty
 *   for each value x<sub>i</sub> of X do
 *       Q(x<sub>i</sub>) <- ENUMERATE-ALL(bn.VARS, e<sub>x<sub>i</sub></sub>)
 *          where e<sub>x<sub>i</sub></sub> is e extended with X = x<sub>i</sub>
 *   return NORMALIZE(Q(X))
 *   
 * ---------------------------------------------------------------------------------------------------
 * 
 * function ENUMERATE-ALL(vars, e) returns a real number
 *   if EMPTY?(vars) then return 1.0
 *   Y <- FIRST(vars)
 *   if Y has value y in e
 *       then return P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e)
 *       else return &sum;<sub>y</sub> P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e<sub>y</sub>)
 *           where e<sub>y</sub> is e extended with Y = y
 * </pre>
 * 
 * Figure 14.9 The enumeration algorithm for answering queries on Bayesian
 * networks. <br>
 * <br>
 * <b>Note:</b> The implementation has been extended to handle queries with
 * multiple variables. <br>
 * 
 * @author Ciaran O'Reilly
 */
public class EnumerationAsk : BayesInference {

	public EnumerationAsk() {

	}

	// function ENUMERATION-ASK(X, e, bn) returns a distribution over X
	/**
	 * The ENUMERATION-ASK algorithm in Figure 14.9 evaluates expression trees
	 * (Figure 14.8) using depth-first recursion.
	 * 
	 * @param X
	 *            the query variables.
	 * @param observedEvidence
	 *            observed values for variables E.
	 * @param bn
	 *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
	 *            variables //
	 * @return a distribution over the query variables.
	 */
	public CategoricalDistribution enumerationAsk(final RandomVariable[] X,
			final AssignmentProposition[] observedEvidence,
			final BayesianNetwork bn) {

		// Q(X) <- a distribution over X, initially empty
		final ProbabilityTable Q = new ProbabilityTable(X);
		final ObservedEvidence e = new ObservedEvidence(X, observedEvidence, bn);
		// for each value x<sub>i</sub> of X do
		ProbabilityTable.Iterator di = new ProbabilityTable.Iterator() {
			int cnt = 0;

			/**
			 * <pre>
			 * Q(x<sub>i</sub>) <- ENUMERATE-ALL(bn.VARS, e<sub>x<sub>i</sub></sub>)
			 *   where e<sub>x<sub>i</sub></sub> is e extended with X = x<sub>i</sub>
			 * </pre>
			 */
			public void iterate(IDictionary<RandomVariable, Object> possibleWorld,
					double probability) {
				for (int i = 0; i < X.Length; ++i) {
					e.setExtendedValue(X[i], possibleWorld.get(X[i]));
				}
				Q.setValue(cnt,
						enumerateAll(bn.getVariablesInTopologicalOrder(), e));
				cnt++;
			}
		};
		Q.iterateOverTable(di);

		// return NORMALIZE(Q(X))
		return Q.normalize();
	}

	//
	// START-BayesInference
	public CategoricalDistribution ask(final RandomVariable[] X,
			final AssignmentProposition[] observedEvidence,
			final BayesianNetwork bn) {
		return this.enumerationAsk(X, observedEvidence, bn);
	}

	// END-BayesInference
	//

	//
	// PROTECTED METHODS
	//
	// function ENUMERATE-ALL(vars, e) returns a real number
	protected double enumerateAll(List<RandomVariable> vars, ObservedEvidence e) {
		// if EMPTY?(vars) then return 1.0
		if (0 == vars.Count) {
			return 1;
		}
		// Y <- FIRST(vars)
		RandomVariable Y = Util.first(vars);
		// if Y has value y in e
		if (e.containsValue(Y)) {
			// then return P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e)
			return e.posteriorForParents(Y) * enumerateAll(Util.rest(vars), e);
		}
		/**
		 * <pre>
		 *  else return &sum;<sub>y</sub> P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e<sub>y</sub>)
		 *       where e<sub>y</sub> is e extended with Y = y
		 * </pre>
		 */
		double sum = 0;
		for (object y : ((FiniteDomain) Y.getDomain()).getPossibleValues()) {
			e.setExtendedValue(Y, y);
			sum += e.posteriorForParents(Y) * enumerateAll(Util.rest(vars), e);
		}

		return sum;
	}

	protected class ObservedEvidence {
		private BayesianNetwork bn = null;
		private object[] extendedValues = null;
		private int hiddenStart = 0;
		private int extendedIdx = 0;
		private RandomVariable[] var = null;
		private IDictionary<RandomVariable, int> varIdxs = new Dictionary<RandomVariable, int>();

		public ObservedEvidence(RandomVariable[] queryVariables,
				AssignmentProposition[] e, BayesianNetwork bn) {
			this.bn = bn;

			int maxSize = bn.getVariablesInTopologicalOrder().Count;
			extendedValues = new object[maxSize];
			var = new RandomVariable[maxSize];
			// query variables go first
			int idx = 0;
			for (int i = 0; i < queryVariables.Length; ++i) {
				var[idx] = queryVariables[i];
				varIdxs.Add(var[idx], idx);
				idx++;
			}
			// initial evidence variables go next
			for (int i = 0; i < e.Length; ++i) {
				var[idx] = e[i].getTermVariable();
				varIdxs.Add(var[idx], idx);
				extendedValues[idx] = e[i].getValue();
				idx++;
			}
			extendedIdx = idx - 1;
			hiddenStart = idx;
			// the remaining slots are left open for the hidden variables
			for (RandomVariable rv : bn.getVariablesInTopologicalOrder()) {
				if (!varIdxs.ContainsKey(rv)) {
					var[idx] = rv;
					varIdxs.Add(var[idx], idx);
					idx++;
				}
			}
		}

		public void setExtendedValue(RandomVariable rv, object value) {
			int idx = varIdxs.get(rv);
			extendedValues[idx] = value;
			if (idx >= hiddenStart) {
				extendedIdx = idx;
			} else {
				extendedIdx = hiddenStart - 1;
			}
		}

		public bool containsValue(RandomVariable rv) {
			return varIdxs.get(rv) <= extendedIdx;
		}

		public double posteriorForParents(RandomVariable rv) {
			Node n = bn.getNode(rv);
			if (!(n is FiniteNode)) {
				throw new ArgumentException(
						"Enumeration-Ask only works with finite Nodes.");
			}
			FiniteNode fn = (FiniteNode) n;
			Object[] vals = new object[1 + fn.getParents().Count];
			int idx = 0;
			for (Node pn : n.getParents()) {
				vals[idx] = extendedValues[varIdxs.get(pn.getRandomVariable())];
				idx++;
			}
			vals[idx] = extendedValues[varIdxs.get(rv)];

			return fn.getCPT().getValue(vals);
		}
	}

	//
	// PRIVATE METHODS
	//
}