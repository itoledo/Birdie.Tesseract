 namespace aima.core.logic.fol.inference.proof;

 
 
 

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepPremise : AbstractProofStep {
	//
	private static readonly List<ProofStep> _noPredecessors = new List<ProofStep>();
	//
	private object proof = "";

	public ProofStepPremise(object proof) {
		this.proof = proof;
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(_noPredecessors);
	}

	 
	public string getProof() {
		return proof.ToString();
	}

	 
	public string getJustification() {
		return "Premise";
	}
	// END-ProofStep
	//
}
