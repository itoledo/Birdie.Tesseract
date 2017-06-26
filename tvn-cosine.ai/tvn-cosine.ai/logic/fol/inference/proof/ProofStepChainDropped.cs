 namespace aima.core.logic.fol.inference.proof;

 
 
 

import aima.core.logic.fol.kb.data.Chain;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepChainDropped : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Chain dropped = null;
	private Chain droppedOff = null;

	public ProofStepChainDropped(Chain dropped, Chain droppedOff) {
		this.dropped = dropped;
		this.droppedOff = droppedOff;
		this.predecessors.Add(droppedOff.getProofStep());
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return dropped.ToString();
	}

	 
	public string getJustification() {
		return "Dropped: " + droppedOff.getProofStep().getStepNumber();
	}
	// END-ProofStep
	//
}
