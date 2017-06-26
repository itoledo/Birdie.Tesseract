 namespace aima.core.logic.fol.inference.proof;

 
 
 
 

import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofFinal : Proof {
	private IDictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
	private ProofStep finalStep = null;
	private List<ProofStep> proofSteps = null;

	public ProofFinal(ProofStep finalStep, IDictionary<Variable, Term> answerBindings) {
		this.finalStep = finalStep;
		this.answerBindings.putAll(answerBindings);
	}

	//
	// START-Proof
	public List<ProofStep> getSteps() {
		// Only calculate if the proof steps are actually requested.
		if (null == proofSteps) {
			calcualteProofSteps();
		}
		return proofSteps;
	}

	public IDictionary<Variable, Term> getAnswerBindings() {
		return answerBindings;
	}

	public void replaceAnswerBindings(IDictionary<Variable, Term> updatedBindings) {
		answerBindings.Clear();
		answerBindings.putAll(updatedBindings);
	}

	// END-Proof
	//

	 
	public override string ToString() {
		return answerBindings.ToString();
	}

	//
	// PRIVATE METHODS
	//
	private void calcualteProofSteps() {
		proofSteps = new List<ProofStep>();
		addToProofSteps(finalStep);

		// Move all premises to the front of the
		// list of steps
		int to = 0;
		for (int i = 0; i < proofSteps.Count; ++i) {
			if (proofSteps.get(i) is ProofStepPremise) {
				ProofStep m = proofSteps.Remove(i);
				proofSteps.Add(to, m);
				to++;
			}
		}

		// Move the Goals after the premises
		for (int i = 0; i < proofSteps.Count; ++i) {
			if (proofSteps.get(i) is ProofStepGoal) {
				ProofStep m = proofSteps.Remove(i);
				proofSteps.Add(to, m);
				to++;
			}
		}

		// Assign the step #s now that all the proof
		// steps have been unwound
		for (int i = 0; i < proofSteps.Count; ++i) {
			proofSteps.get(i).setStepNumber(i + 1);
		}
	}

	private void addToProofSteps(ProofStep step) {
		if (!proofSteps.Contains(step)) {
			proofSteps.Add(0, step);
		} else {
			proofSteps.Remove(step);
			proofSteps.Add(0, step);
		}
		List<ProofStep> predecessors = step.getPredecessorSteps();
		for (int i = predecessors.Count - 1; i >= 0; i--) {
			addToProofSteps(predecessors.get(i));
		}
	}
}
