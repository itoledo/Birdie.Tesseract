 namespace aima.core.logic.fol.inference.proof;

 
 
 

import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.parsing.ast.Sentence;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepClauseClausifySentence : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause clausified = null;

	public ProofStepClauseClausifySentence(Clause clausified,
			Sentence origSentence) {
		this.clausified = clausified;
		this.predecessors.Add(new ProofStepPremise(origSentence));
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return clausified.ToString();
	}

	 
	public string getJustification() {
		return "Clausified " + predecessors.get(0).getStepNumber();
	}
	// END-ProofStep
	//
}
