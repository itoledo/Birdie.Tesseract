 namespace aima.core.probability.hmm.impl;

 
import java.util.Arrays;
 
 

import aima.core.probability.CategoricalDistribution;
import aima.core.probability.RandomVariable;
import aima.core.probability.domain.FiniteDomain;
import aima.core.probability.hmm.HiddenMarkovModel;
import aima.core.probability.proposition.AssignmentProposition;
import aima.core.probability.util.ProbabilityTable;
 
import aima.core.util.math.Matrix;

/**
 * Default implementation of the HiddenMarkovModel interface.
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 */
public class HMM : HiddenMarkovModel {
	private RandomVariable stateVariable = null;
	private FiniteDomain stateVariableDomain = null;
	private Matrix transitionModel = null;
	private IDictionary<Object, Matrix> sensorModel = null;
	private Matrix prior = null;

	/**
	 * Instantiate a Hidden Markov Model.
	 * 
	 * @param stateVariable
	 *            the single discrete random variable used to describe the
	 *            process states 1,...,S.
	 * @param transitionModel
	 *            the transition model:<br>
	 *            <b>P</b>(X<sub>t</sub> | X<sub>t-1</sub>)<br>
	 *            is represented by an S * S matrix <b>T</b> where<br>
	 *            <b>T</b><sub>ij</sub> = P(X<sub>t</sub> = j | X<sub>t-1</sub>
	 *            = i).
	 * @param sensorModel
	 *            the sensor model in matrix form:<br>
	 *            P(e<sub>t</sub> | X<sub>t</sub> = i) for each state i. For
	 *            mathematical convenience we place each of these values into an
	 *            S * S diagonal matrix.
	 * @param prior
	 *            the prior distribution represented as a column vector in
	 *            Matrix form.
	 */
	public HMM(RandomVariable stateVariable, Matrix transitionModel,
			IDictionary<Object, Matrix> sensorModel, Matrix prior) {
		if (!stateVariable.getDomain().isFinite()) {
			throw new ArgumentException(
					"State Variable for HHM must be finite.");
		}
		this.stateVariable = stateVariable;
		stateVariableDomain = (FiniteDomain) stateVariable.getDomain();
		if (transitionModel.getRowDimension() != transitionModel
				.getColumnDimension()) {
			throw new ArgumentException(
					"Transition Model row and column dimensions must match.");
		}
		if (stateVariableDomain.Count != transitionModel.getRowDimension()) {
			throw new ArgumentException(
					"Transition Model Matrix does not map correctly to the HMM's State Variable.");
		}
		this.transitionModel = transitionModel;
		for (Matrix smVal : sensorModel.values()) {
			if (smVal.getRowDimension() != smVal.getColumnDimension()) {
				throw new ArgumentException(
						"Sensor Model row and column dimensions must match.");
			}
			if (stateVariableDomain.Count != smVal.getRowDimension()) {
				throw new ArgumentException(
						"Sensor Model Matrix does not map correctly to the HMM's State Variable.");
			}
		}
		this.sensorModel = sensorModel;
		if (transitionModel.getRowDimension() != prior.getRowDimension()
				&& prior.getColumnDimension() != 1) {
			throw new ArgumentException(
					"Prior is not of the correct dimensions.");
		}
		this.prior = prior;
	}

	//
	// START-HiddenMarkovModel
	 
	public RandomVariable getStateVariable() {
		return stateVariable;
	}

	 
	public Matrix getTransitionModel() {
		return transitionModel;
	}

	 
	public IDictionary<Object, Matrix> getSensorModel() {
		return sensorModel;
	}

	 
	public Matrix getPrior() {
		return prior;
	}

	 
	public Matrix getEvidence(List<AssignmentProposition> evidence) {
		if (evidence.Count != 1) {
			throw new ArgumentException(
					"Only a single evidence observation value should be provided.");
		}
		Matrix e = sensorModel.get(evidence.get(0).getValue());
		if (null == e) {
			throw new ArgumentException(
					"Evidence does not map to sensor model.");
		}
		return e;
	}

	 
	public Matrix createUnitMessage() {
		double[] values = new double[stateVariableDomain.Count];
		Arrays.fill(values, 1.0);
		return new Matrix(values, values.Length);
	}

	 
	public Matrix convert(CategoricalDistribution fromCD) {
		double[] values = fromCD.getValues();
		return new Matrix(values, values.Length);
	}

	 
	public CategoricalDistribution convert(Matrix fromMessage) {
		return new ProbabilityTable(fromMessage.getRowPackedCopy(),
				stateVariable);
	}

	 
	public List<CategoricalDistribution> convert(List<Matrix> matrixs) {
		List<CategoricalDistribution> cds = new List<CategoricalDistribution>();
		for (Matrix m : matrixs) {
			cds.Add(convert(m));
		}
		return cds;
	}

	 
	public Matrix normalize(Matrix m) {
		double[] values = m.getRowPackedCopy();
		return new Matrix(Util.normalize(values), values.Length);
	}

	// END-HiddenMarkovModel
	//
}
