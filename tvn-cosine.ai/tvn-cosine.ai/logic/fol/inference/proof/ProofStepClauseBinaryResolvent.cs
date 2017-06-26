 namespace aima.core.logic.fol.inference.proof;

 
 
 
 
 

import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepClauseBinaryResolvent : AbstractProofStep {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause resolvent = null;
	private Literal posLiteral = null;
	private Literal negLiteral = null;
	private Clause parent1, parent2 = null;
	private IDictionary<Variable, Term> subst = new Dictionary<Variable, Term>();
	private IDictionary<Variable, Term> renameSubst = new Dictionary<Variable, Term>();

	public ProofStepClauseBinaryResolvent(Clause resolvent, Literal pl,
			Literal nl, Clause parent1, Clause parent2,
			IDictionary<Variable, Term> subst, IDictionary<Variable, Term> renameSubst) {
		this.resolvent = resolvent;
		this.posLiteral = pl;
		this.negLiteral = nl;
		this.parent1 = parent1;
		this.parent2 = parent2;
		this.subst.putAll(subst);
		this.renameSubst.putAll(renameSubst);
		this.predecessors.Add(parent1.getProofStep());
		this.predecessors.Add(parent2.getProofStep());
	}

	//
	// START-ProofStep
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	public string getProof() {
		return resolvent.ToString();
	}

	public string getJustification() {
		int lowStep = parent1.getProofStep().getStepNumber();
		int highStep = parent2.getProofStep().getStepNumber();

		if (lowStep > highStep) {
			lowStep = highStep;
			highStep = parent1.getProofStep().getStepNumber();
		}

		return "Resolution: " + lowStep + ", " + highStep + "  [" + posLiteral
				+ ", " + negLiteral + "], subst=" + subst + ", renaming="
				+ renameSubst;
	}
	// END-ProofStep
	//
}
