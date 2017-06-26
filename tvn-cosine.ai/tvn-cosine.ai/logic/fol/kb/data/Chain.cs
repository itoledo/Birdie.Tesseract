 namespace aima.core.logic.fol.kb.data;

 
 
 
 

import aima.core.logic.fol.inference.proof.ProofStep;
import aima.core.logic.fol.inference.proof.ProofStepChainContrapositive;
import aima.core.logic.fol.inference.proof.ProofStepPremise;

/**
 * 
 * A Chain is a sequence of literals (while a clause is a set) - order is
 * important for a chain.
 * 
 * @see <a
 *      href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf"
 *      >Chain</a>
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class Chain {
	private static List<Literal> _emptyLiteralsList = Collections
			.unmodifiableList(new List<Literal>());
	//
	private List<Literal> literals = new List<Literal>();
	private ProofStep proofStep = null;

	public Chain() {
		// i.e. the empty chain
	}

	public Chain(List<Literal> literals) {
		this.literals.addAll(literals);
	}

	public Chain(ISet<Literal> literals) {
		this.literals.addAll(literals);
	}

	public ProofStep getProofStep() {
		if (null == proofStep) {
			// Assume was a premise
			proofStep = new ProofStepPremise(this);
		}
		return proofStep;
	}

	public void setProofStep(ProofStep proofStep) {
		this.proofStep = proofStep;
	}

	public bool isEmpty() {
		return literals.Count == 0;
	}

	public void addLiteral(Literal literal) {
		literals.Add(literal);
	}

	public Literal getHead() {
		if (0 == literals.Count) {
			return null;
		}
		return literals.get(0);
	}

	public List<Literal> getTail() {
		if (0 == literals.Count) {
			return _emptyLiteralsList;
		}
		return Collections
				.unmodifiableList(literals.subList(1, literals.Count));
	}

	public int getNumberLiterals() {
		return literals.Count;
	}

	public List<Literal> getLiterals() {
		return Collections.unmodifiableList(literals);
	}

	/**
	 * A contrapositive of a chain is a permutation in which a different literal
	 * is placed at the front. The contrapositives of a chain are logically
	 * equivalent to the original chain.
	 * 
	 * @return a list of contrapositives for this chain.
	 */
	public List<Chain> getContrapositives() {
		List<Chain> contrapositives = new List<Chain>();
		List<Literal> lits = new List<Literal>();

		for (int i = 1; i < literals.Count; ++i) {
			lits.Clear();
			lits.Add(literals.get(i));
			lits.addAll(literals.subList(0, i));
			lits.addAll(literals.subList(i + 1, literals.Count));
			Chain cont = new Chain(lits);
			cont.setProofStep(new ProofStepChainContrapositive(cont, this));
			contrapositives.Add(cont);
		}

		return contrapositives;
	}

	 
	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		sb.Append("<");

		for (int i = 0; i < literals.Count; ++i) {
			if (i > 0) {
				sb.Append(",");
			}
			sb.Append(literals.get(i).ToString());
		}

		sb.Append(">");

		return sb.ToString();
	}
}
