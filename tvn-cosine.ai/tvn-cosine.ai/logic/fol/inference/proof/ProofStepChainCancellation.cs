 namespace aima.core.logic.fol.inference.proof;

 
 
 
 

import aima.core.logic.fol.kb.data.Chain;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepChainCancellation : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Chain cancellation = null;
	private Chain cancellationOf = null;
	private IDictionary<Variable, Term> subst = null;

	public ProofStepChainCancellation(Chain cancellation, Chain cancellationOf,
			IDictionary<Variable, Term> subst) {
		this.cancellation = cancellation;
		this.cancellationOf = cancellationOf;
		this.subst = subst;
		this.predecessors.Add(cancellationOf.getProofStep());
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return cancellation.ToString();
	}

	 
	public string getJustification() {
		return "Cancellation: " + cancellationOf.getProofStep().getStepNumber()
				+ " " + subst;
	}
	// END-ProofStep
	//
}
