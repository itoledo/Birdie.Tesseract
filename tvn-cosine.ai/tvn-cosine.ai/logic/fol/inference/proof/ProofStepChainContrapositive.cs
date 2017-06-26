 namespace aima.core.logic.fol.inference.proof;

 
 
 

import aima.core.logic.fol.kb.data.Chain;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepChainContrapositive : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Chain contrapositive = null;
	private Chain contrapositiveOf = null;

	public ProofStepChainContrapositive(Chain contrapositive,
			Chain contrapositiveOf) {
		this.contrapositive = contrapositive;
		this.contrapositiveOf = contrapositiveOf;
		this.predecessors.Add(contrapositiveOf.getProofStep());
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return contrapositive.ToString();
	}

	 
	public string getJustification() {
		return "Contrapositive: "
				+ contrapositiveOf.getProofStep().getStepNumber();
	}
	// END-ProofStep
	//
}
