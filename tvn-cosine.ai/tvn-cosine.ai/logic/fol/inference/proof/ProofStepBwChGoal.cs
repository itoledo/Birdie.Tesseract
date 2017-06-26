 namespace aima.core.logic.fol.inference.proof;

 
 
 
 
 

import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepBwChGoal : AbstractProofStep {
	//
	private List<ProofStep> predecessors = new List<ProofStep>();
	//
	private Clause toProve = null;
	private Literal currentGoal = null;
	private IDictionary<Variable, Term> bindings = new Dictionary<Variable, Term>();

	public ProofStepBwChGoal(Clause toProve, Literal currentGoal,
			IDictionary<Variable, Term> bindings) {
		this.toProve = toProve;
		this.currentGoal = currentGoal;
		this.bindings.putAll(bindings);
	}

	public IDictionary<Variable, Term> getBindings() {
		return bindings;
	}

	public void setPredecessor(ProofStep predecessor) {
		predecessors.Clear();
		predecessors.Add(predecessor);
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		StringBuilder sb = new StringBuilder();
		List<Literal> nLits = toProve.getNegativeLiterals();
		for (int i = 0; i < toProve.getNumberNegativeLiterals(); ++i) {
			sb.Append(nLits.get(i).getAtomicSentence());
			if (i != (toProve.getNumberNegativeLiterals() - 1)) {
				sb.Append(" AND ");
			}
		}
		if (toProve.getNumberNegativeLiterals() > 0) {
			sb.Append(" => ");
		}
		sb.Append(toProve.getPositiveLiterals().get(0));
		return sb.ToString();
	}

	 
	public string getJustification() {
		return "Current Goal " + currentGoal.getAtomicSentence().ToString()
				+ ", " + bindings;
	}
	// END-ProofStep
	//
}
