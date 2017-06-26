 namespace aima.core.logic.fol;

 
 

import aima.core.logic.fol.kb.data.Chain;
import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.FOLVisitor;
import aima.core.logic.fol.parsing.ast.ConnectedSentence;
import aima.core.logic.fol.parsing.ast.Constant;
import aima.core.logic.fol.parsing.ast.Function;
import aima.core.logic.fol.parsing.ast.NotSentence;
import aima.core.logic.fol.parsing.ast.Predicate;
import aima.core.logic.fol.parsing.ast.QuantifiedSentence;
import aima.core.logic.fol.parsing.ast.Sentence;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.TermEquality;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class VariableCollector : FOLVisitor {

	public VariableCollector() {
	}

	// Note: The set guarantees the order in which they were
	// found.
	public ISet<Variable> collectAllVariables(Sentence sentence) {
		Set<Variable> variables = new HashSet<Variable>();

		sentence.accept(this, variables);

		return variables;
	}

	public ISet<Variable> collectAllVariables(Term term) {
		Set<Variable> variables = new HashSet<Variable>();

		term.accept(this, variables);

		return variables;
	}

	public ISet<Variable> collectAllVariables(Clause clause) {
		Set<Variable> variables = new HashSet<Variable>();

		for (Literal l : clause.getLiterals()) {
			l.getAtomicSentence().accept(this, variables);
		}

		return variables;
	}

	public ISet<Variable> collectAllVariables(Chain chain) {
		Set<Variable> variables = new HashSet<Variable>();

		for (Literal l : chain.getLiterals()) {
			l.getAtomicSentence().accept(this, variables);
		}

		return variables;
	}

	@SuppressWarnings("unchecked")
	public object visitVariable(Variable var, object arg) {
		Set<Variable> variables = (ISet<Variable>) arg;
		variables.Add(var);
		return var;
	}

	@SuppressWarnings("unchecked")
	public object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg) {
		// Ensure I collect quantified variables too
		Set<Variable> variables = (ISet<Variable>) arg;
		variables.addAll(sentence.getVariables());

		sentence.getQuantified().accept(this, arg);

		return sentence;
	}

	public object visitPredicate(Predicate predicate, object arg) {
		for (Term t : predicate.getTerms()) {
			t.accept(this, arg);
		}
		return predicate;
	}

	public object visitTermEquality(TermEquality equality, object arg) {
		equality.getTerm1().accept(this, arg);
		equality.getTerm2().accept(this, arg);
		return equality;
	}

	public object visitConstant(Constant constant, object arg) {
		return constant;
	}

	public object visitFunction(Function function, object arg) {
		for (Term t : function.getTerms()) {
			t.accept(this, arg);
		}
		return function;
	}

	public object visitNotSentence(NotSentence sentence, object arg) {
		sentence.getNegated().accept(this, arg);
		return sentence;
	}

	public object visitConnectedSentence(ConnectedSentence sentence, object arg) {
		sentence.getFirst().accept(this, arg);
		sentence.getSecond().accept(this, arg);
		return sentence;
	}
}