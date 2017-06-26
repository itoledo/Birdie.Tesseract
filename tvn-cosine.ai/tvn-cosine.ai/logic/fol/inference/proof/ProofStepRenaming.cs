 namespace aima.core.logic.fol.inference.proof;

 
 
 

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepRenaming : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private object proof = "";

	public ProofStepRenaming(object proof, ProofStep predecessor) {
		this.proof = proof;
		this.predecessors.Add(predecessor);
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return proof.ToString();
	}

	 
	public string getJustification() {
		return "Renaming of " + predecessors.get(0).getStepNumber();
	}
	// END-ProofStep
	//
}
