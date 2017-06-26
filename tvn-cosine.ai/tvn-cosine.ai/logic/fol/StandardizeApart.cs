 namespace aima.core.logic.fol;

 
 
import java.util.HashSet;
 
 
 

import aima.core.logic.fol.inference.proof.ProofStepRenaming;
import aima.core.logic.fol.kb.data.Chain;
import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.ast.AtomicSentence;
import aima.core.logic.fol.parsing.ast.Sentence;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class StandardizeApart {
	private VariableCollector variableCollector = null;
	private SubstVisitor substVisitor = null;

	public StandardizeApart() {
		variableCollector = new VariableCollector();
		substVisitor = new SubstVisitor();
	}

	public StandardizeApart(VariableCollector variableCollector,
			SubstVisitor substVisitor) {
		this.variableCollector = variableCollector;
		this.substVisitor = substVisitor;
	}

	// Note: see page 327.
	public StandardizeApartResult standardizeApart(Sentence sentence,
			StandardizeApartIndexical standardizeApartIndexical) {
		Set<Variable> toRename = variableCollector
				.collectAllVariables(sentence);
		IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();
		IDictionary<Variable, Term> reverseSubstitution = new Dictionary<Variable, Term>();

		for (Variable var : toRename) {
			Variable v = null;
			do {
				v = new Variable(standardizeApartIndexical.getPrefix()
						+ standardizeApartIndexical.getNextIndex());
				// Ensure the new variable name is not already
				// accidentally used in the sentence
			} while (toRename.Contains(v));

			renameSubstitution.Add(var, v);
			reverseSubstitution.Add(v, var);
		}

		Sentence standardized = substVisitor.subst(renameSubstitution,
				sentence);

		return new StandardizeApartResult(sentence, standardized,
				renameSubstitution, reverseSubstitution);
	}

	public Clause standardizeApart(Clause clause,
			StandardizeApartIndexical standardizeApartIndexical) {

		Set<Variable> toRename = variableCollector.collectAllVariables(clause);
		IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

		for (Variable var : toRename) {
			Variable v = null;
			do {
				v = new Variable(standardizeApartIndexical.getPrefix()
						+ standardizeApartIndexical.getNextIndex());
				// Ensure the new variable name is not already
				// accidentally used in the sentence
			} while (toRename.Contains(v));

			renameSubstitution.Add(var, v);
		}

		if (renameSubstitution.Count > 0) {
			List<Literal> literals = new List<Literal>();

			for (Literal l : clause.getLiterals()) {
				literals.Add(substVisitor.subst(renameSubstitution, l));
			}
			Clause renamed = new Clause(literals);
			renamed.setProofStep(new ProofStepRenaming(renamed, clause
					.getProofStep()));
			return renamed;
		}

		return clause;
	}

	public Chain standardizeApart(Chain chain,
			StandardizeApartIndexical standardizeApartIndexical) {

		Set<Variable> toRename = variableCollector.collectAllVariables(chain);
		IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

		for (Variable var : toRename) {
			Variable v = null;
			do {
				v = new Variable(standardizeApartIndexical.getPrefix()
						+ standardizeApartIndexical.getNextIndex());
				// Ensure the new variable name is not already
				// accidentally used in the sentence
			} while (toRename.Contains(v));

			renameSubstitution.Add(var, v);
		}

		if (renameSubstitution.Count > 0) {
			List<Literal> lits = new List<Literal>();

			for (Literal l : chain.getLiterals()) {
				AtomicSentence atom = (AtomicSentence) substVisitor.subst(
						renameSubstitution, l.getAtomicSentence());
				lits.Add(l.newInstance(atom));
			}

			Chain renamed = new Chain(lits);

			renamed.setProofStep(new ProofStepRenaming(renamed, chain
					.getProofStep()));

			return renamed;
		}

		return chain;
	}

	public IDictionary<Variable, Term> standardizeApart(List<Literal> l1Literals,
			List<Literal> l2Literals,
			StandardizeApartIndexical standardizeApartIndexical) {
		Set<Variable> toRename = new HashSet<Variable>();

		for (Literal pl : l1Literals) {
			toRename.addAll(variableCollector.collectAllVariables(pl
					.getAtomicSentence()));
		}
		for (Literal nl : l2Literals) {
			toRename.addAll(variableCollector.collectAllVariables(nl
					.getAtomicSentence()));
		}

		IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

		for (Variable var : toRename) {
			Variable v = null;
			do {
				v = new Variable(standardizeApartIndexical.getPrefix()
						+ standardizeApartIndexical.getNextIndex());
				// Ensure the new variable name is not already
				// accidentally used in the sentence
			} while (toRename.Contains(v));

			renameSubstitution.Add(var, v);
		}

		List<Literal> posLits = new List<Literal>();
		List<Literal> negLits = new List<Literal>();

		for (Literal pl : l1Literals) {
			posLits.Add(substVisitor.subst(renameSubstitution, pl));
		}
		for (Literal nl : l2Literals) {
			negLits.Add(substVisitor.subst(renameSubstitution, nl));
		}

		l1Literals.Clear();
		l1Literals.addAll(posLits);
		l2Literals.Clear();
		l2Literals.addAll(negLits);

		return renameSubstitution;
	}
}
