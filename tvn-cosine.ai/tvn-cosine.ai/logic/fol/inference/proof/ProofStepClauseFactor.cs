 namespace aima.core.logic.fol.inference.proof;

 
 
 
 
 

import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepClauseFactor : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause factor = null;
	private Clause factorOf = null;
	private Literal lx = null;
	private Literal ly = null;
	private IDictionary<Variable, Term> subst = new Dictionary<Variable, Term>();
	private IDictionary<Variable, Term> renameSubst = new Dictionary<Variable, Term>();

	public ProofStepClauseFactor(Clause factor, Clause factorOf, Literal lx,
			Literal ly, IDictionary<Variable, Term> subst,
			IDictionary<Variable, Term> renameSubst) {
		this.factor = factor;
		this.factorOf = factorOf;
		this.lx = lx;
		this.ly = ly;
		this.subst.putAll(subst);
		this.renameSubst.putAll(renameSubst);
		this.predecessors.Add(factorOf.getProofStep());
	}

	//
	// START-ProofStep
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	public string getProof() {
		return factor.ToString();
	}

	public string getJustification() {
		return "Factor of " + factorOf.getProofStep().getStepNumber() + "  ["
				+ lx + ", " + ly + "], subst=" + subst + ", renaming="
				+ renameSubst;
	}
	// END-ProofStep
	//
}
