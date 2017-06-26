 namespace aima.core.learning.neural;

 
 

import aima.core.util.math.Vector;

/**
 * @author Ravi Mohan
 * 
 */
public class NNExample {
	private readonly List<double> normalizedInput, normalizedTarget;

	public NNExample(List<double> normalizedInput, List<double> normalizedTarget) {
		this.normalizedInput = normalizedInput;
		this.normalizedTarget = normalizedTarget;
	}

	public NNExample copyExample() {
		List<double> newInput = new List<double>();
		List<double> newTarget = new List<double>();
		for (double d : normalizedInput) {
			newInput.Add(new double(d.doubleValue()));
		}
		for (double d : normalizedTarget) {
			newTarget.Add(new double(d.doubleValue()));
		}
		return new NNExample(newInput, newTarget);
	}

	public Vector getInput() {
		Vector v = new Vector(normalizedInput);
		return v;

	}

	public Vector getTarget() {
		Vector v = new Vector(normalizedTarget);
		return v;

	}

	public bool isCorrect(Vector prediction) {
		/*
		 * compares the index having greatest value in target to indec having
		 * greatest value in prediction. Ifidentical, correct
		 */
		return getTarget().indexHavingMaxValue() == prediction
				.indexHavingMaxValue();
	}
}
