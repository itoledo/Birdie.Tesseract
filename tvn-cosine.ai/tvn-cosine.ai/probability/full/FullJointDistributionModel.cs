 namespace aima.core.probability.full;

 
 
 
 

import aima.core.probability.CategoricalDistribution;
import aima.core.probability.FiniteProbabilityModel;
import aima.core.probability.ProbabilityModel;
import aima.core.probability.RandomVariable;
import aima.core.probability.proposition.ConjunctiveProposition;
import aima.core.probability.proposition.Proposition;
import aima.core.probability.util.ProbUtil;
import aima.core.probability.util.ProbabilityTable;

/**
 * An implementation of the FiniteProbabilityModel API using a full joint
 * distribution as the underlying model.
 * 
 * @author Ciaran O'Reilly
 */
public class FullJointDistributionModel : FiniteProbabilityModel {

	private ProbabilityTable distribution = null;
	private ISet<RandomVariable> representation = null;

	public FullJointDistributionModel(double[] values, RandomVariable... vars) {
		if (null == vars) {
			throw new ArgumentException(
					"Random Variables describing the model's representation of the World need to be specified.");
		}

		distribution = new ProbabilityTable(values, vars);

		representation = new HashSet<RandomVariable>();
		for (int i = 0; i < vars.Length; ++i) {
			representation.Add(vars[i]);
		}
		representation = new HashSet<>(representation);
	}

	//
	// START-ProbabilityModel
	public bool isValid() {
		// Handle rounding
		return Math.Abs(1 - distribution.getSum()) <= ProbabilityModel.DEFAULT_ROUNDING_THRESHOLD;
	}

	public double prior(Proposition... phi) {
		return probabilityOf(ProbUtil.constructConjunction(phi));
	}

	public double posterior(Proposition phi, Proposition... evidence) {

		Proposition conjEvidence = ProbUtil.constructConjunction(evidence);

		// P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
		Proposition aAndB = new ConjunctiveProposition(phi, conjEvidence);
		double probabilityOfEvidence = prior(conjEvidence);
		if (0 != probabilityOfEvidence) {
			return prior(aAndB) / probabilityOfEvidence;
		}

		return 0;
	}

	public ISet<RandomVariable> getRepresentation() {
		return representation;
	}

	// END-ProbabilityModel
	//

	//
	// START-FiniteProbabilityModel
	public CategoricalDistribution priorDistribution(Proposition... phi) {
		return jointDistribution(phi);
	}

	public CategoricalDistribution posteriorDistribution(Proposition phi,
			Proposition... evidence) {

		Proposition conjEvidence = ProbUtil.constructConjunction(evidence);

		// P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
		CategoricalDistribution dAandB = jointDistribution(phi, conjEvidence);
		CategoricalDistribution dEvidence = jointDistribution(conjEvidence);

		return dAandB.divideBy(dEvidence);
	}

	public CategoricalDistribution jointDistribution(
			Proposition... propositions) {
		ProbabilityTable d = null;
		final Proposition conjProp = ProbUtil
				.constructConjunction(propositions);
		final HashSet<RandomVariable> vars = new HashSet<RandomVariable>(
				conjProp.getUnboundScope());

		if (vars.Count > 0) {
			RandomVariable[] distVars = new RandomVariable[vars.Count];
			vars.toArray(distVars);

			final ProbabilityTable ud = new ProbabilityTable(distVars);
			final object[] values = new object[vars.Count];

			ProbabilityTable.Iterator di = new ProbabilityTable.Iterator() {

				public void iterate(IDictionary<RandomVariable, Object> possibleWorld,
						double probability) {
					if (conjProp.holds(possibleWorld)) {
						int i = 0;
						for (RandomVariable rv : vars) {
							values[i] = possibleWorld.get(rv);
							i++;
						}
						int dIdx = ud.getIndex(values);
						ud.setValue(dIdx, ud.getValues()[dIdx] + probability);
					}
				}
			};

			distribution.iterateOverTable(di);

			d = ud;
		} else {
			// No Unbound Variables, therefore just return
			// the singular probability related to the proposition.
			d = new ProbabilityTable();
			d.setValue(0, prior(propositions));
		}
		return d;
	}

	// END-FiniteProbabilityModel
	//

	//
	// PRIVATE METHODS
	//
	private double probabilityOf(final Proposition phi) {
		final double[] probSum = new double[1];
		ProbabilityTable.Iterator di = new ProbabilityTable.Iterator() {
			public void iterate(IDictionary<RandomVariable, Object> possibleWorld,
					double probability) {
				if (phi.holds(possibleWorld)) {
					probSum[0] += probability;
				}
			}
		};

		distribution.iterateOverTable(di);

		return probSum[0];
	}
}
