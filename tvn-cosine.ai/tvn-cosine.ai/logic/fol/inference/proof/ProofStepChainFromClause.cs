 namespace aima.core.logic.fol.inference.proof;

 
 
 

import aima.core.logic.fol.kb.data.Chain;
import aima.core.logic.fol.kb.data.Clause;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepChainFromClause : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Chain chain = null;
	private Clause fromClause = null;

	public ProofStepChainFromClause(Chain chain, Clause fromClause) {
		this.chain = chain;
		this.fromClause = fromClause;
		this.predecessors.Add(fromClause.getProofStep());
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return chain.ToString();
	}

	 
	public string getJustification() {
		return "Chain from Clause: "
				+ fromClause.getProofStep().getStepNumber();
	}
	// END-ProofStep
	//
}
