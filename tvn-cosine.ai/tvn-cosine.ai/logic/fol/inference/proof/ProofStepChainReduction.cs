 namespace aima.core.logic.fol.inference.proof;

 
 
 
 

import aima.core.logic.fol.kb.data.Chain;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepChainReduction : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Chain reduction = null;
	private Chain nearParent, farParent = null;
	private IDictionary<Variable, Term> subst = null;

	public ProofStepChainReduction(Chain reduction, Chain nearParent,
			Chain farParent, IDictionary<Variable, Term> subst) {
		this.reduction = reduction;
		this.nearParent = nearParent;
		this.farParent = farParent;
		this.subst = subst;
		this.predecessors.Add(farParent.getProofStep());
		this.predecessors.Add(nearParent.getProofStep());
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		return reduction.ToString();
	}

	 
	public string getJustification() {
		return "Reduction: " + nearParent.getProofStep().getStepNumber() + ","
				+ farParent.getProofStep().getStepNumber() + " " + subst;
	}
	// END-ProofStep
	//
}
