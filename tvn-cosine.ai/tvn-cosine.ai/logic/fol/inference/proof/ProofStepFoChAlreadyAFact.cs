 namespace aima.core.logic.fol.inference.proof;

 
 
 

import aima.core.logic.fol.kb.data.Literal;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepFoChAlreadyAFact : AbstractProofStep {
	//
	private static readonly List<ProofStep> _noPredecessors = new List<ProofStep>();
	//
	private Literal fact = null;

	public ProofStepFoChAlreadyAFact(Literal fact) {
		this.fact = fact;
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(_noPredecessors);
	}

	 
	public string getProof() {
		return fact.ToString();
	}

	 
	public string getJustification() {
		return "Already a known fact in the KB.";
	}
	// END-ProofStep
	//
}
