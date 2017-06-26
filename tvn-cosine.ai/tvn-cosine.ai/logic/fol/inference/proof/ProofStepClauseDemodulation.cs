 namespace aima.core.logic.fol.inference.proof;

 
 
 

import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.parsing.ast.TermEquality;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepClauseDemodulation : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause demodulated = null;
	private Clause origClause = null;
	private TermEquality assertion = null;

	public ProofStepClauseDemodulation(Clause demodulated, Clause origClause,
			TermEquality assertion) {
		this.demodulated = demodulated;
		this.origClause = origClause;
		this.assertion = assertion;
		this.predecessors.Add(origClause.getProofStep());
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return demodulated.ToString();
	}

	 
	public string getJustification() {
		return "Demodulation: " + origClause.getProofStep().getStepNumber()
				+ ", [" + assertion + "]";
	}
	// END-ProofStep
	//
}
