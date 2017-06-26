 namespace aima.core.logic.fol.inference.proof;

 
 
 
 

import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class ProofStepFoChAssertFact : AbstractProofStep {
	//
	private List<ProofStep> predecessors = new List<ProofStep>();
	//
	private Clause implication = null;
	private Literal fact = null;
	private IDictionary<Variable, Term> bindings = null;

	public ProofStepFoChAssertFact(Clause implication, Literal fact,
			IDictionary<Variable, Term> bindings, ProofStep predecessor) {
		this.implication = implication;
		this.fact = fact;
		this.bindings = bindings;
		if (null != predecessor) {
			predecessors.Add(predecessor);
		}
	}

	//
	// START-ProofStep
	 
	public List<ProofStep> getPredecessorSteps() {
		return Collections.unmodifiableList(predecessors);
	}

	 
	public string getProof() {
		StringBuilder sb = new StringBuilder();
		List<Literal> nLits = implication.getNegativeLiterals();
		for (int i = 0; i < implication.getNumberNegativeLiterals(); ++i) {
			sb.Append(nLits.get(i).getAtomicSentence());
			if (i != (implication.getNumberNegativeLiterals() - 1)) {
				sb.Append(" AND ");
			}
		}
		sb.Append(" => ");
		sb.Append(implication.getPositiveLiterals().get(0));
		return sb.ToString();
	}

	 
	public string getJustification() {
		return "Assert fact " + fact.ToString() + ", " + bindings;
	}
	// END-ProofStep
	//
}
