 namespace aima.core.logic.fol.inference.proof;

 
 
 

import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.parsing.ast.TermEquality;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepClauseParamodulation : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause paramodulated = null;
	private Clause topClause = null;
	private Clause equalityClause = null;
	private TermEquality assertion = null;

	public ProofStepClauseParamodulation(Clause paramodulated,
			Clause topClause, Clause equalityClause, TermEquality assertion) {
		this.paramodulated = paramodulated;
		this.topClause = topClause;
		this.equalityClause = equalityClause;
		this.assertion = assertion;
		this.predecessors.Add(topClause.getProofStep());
		this.predecessors.Add(equalityClause.getProofStep());
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return paramodulated.ToString();
	}

	 
	public string getJustification() {
		return "Paramodulation: " + topClause.getProofStep().getStepNumber()
				+ ", " + equalityClause.getProofStep().getStepNumber() + ", ["
				+ assertion + "]";

	}
	// END-ProofStep
	//
}
