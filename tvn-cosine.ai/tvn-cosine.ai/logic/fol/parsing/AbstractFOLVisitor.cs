 namespace aima.core.logic.fol.parsing;

 
 

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
 * 
 */
public class AbstractFOLVisitor : FOLVisitor {

	public AbstractFOLVisitor() {
	}

	protected Sentence recreate(object ast) {
		return ((Sentence) ast).copy();
	}

	public object visitVariable(Variable variable, object arg) {
		return variable.copy();
	}

	public object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg) {
		List<Variable> variables = new List<Variable>();
		for (Variable var : sentence.getVariables()) {
			variables.Add((Variable) var.accept(this, arg));
		}

		return new QuantifiedSentence(sentence.getQuantifier(), variables,
				(Sentence) sentence.getQuantified().accept(this, arg));
	}

	public object visitPredicate(Predicate predicate, object arg) {
		List<Term> terms = predicate.getTerms();
		List<Term> newTerms = new List<Term>();
		for (int i = 0; i < terms.Count; ++i) {
			Term t = terms.get(i);
			Term subsTerm = (Term) t.accept(this, arg);
			newTerms.Add(subsTerm);
		}
		return new Predicate(predicate.getPredicateName(), newTerms);

	}

	public object visitTermEquality(TermEquality equality, object arg) {
		Term newTerm1 = (Term) equality.getTerm1().accept(this, arg);
		Term newTerm2 = (Term) equality.getTerm2().accept(this, arg);
		return new TermEquality(newTerm1, newTerm2);
	}

	public object visitConstant(Constant constant, object arg) {
		return constant;
	}

	public object visitFunction(Function function, object arg) {
		List<Term> terms = function.getTerms();
		List<Term> newTerms = new List<Term>();
		for (int i = 0; i < terms.Count; ++i) {
			Term t = terms.get(i);
			Term subsTerm = (Term) t.accept(this, arg);
			newTerms.Add(subsTerm);
		}
		return new Function(function.getFunctionName(), newTerms);
	}

	public object visitNotSentence(NotSentence sentence, object arg) {
		return new NotSentence((Sentence) sentence.getNegated().accept(this,
				arg));
	}

	public object visitConnectedSentence(ConnectedSentence sentence, object arg) {
		Sentence substFirst = (Sentence) sentence.getFirst().accept(this, arg);
		Sentence substSecond = (Sentence) sentence.getSecond()
				.accept(this, arg);
		return new ConnectedSentence(sentence.getConnector(), substFirst,
				substSecond);
	}
}