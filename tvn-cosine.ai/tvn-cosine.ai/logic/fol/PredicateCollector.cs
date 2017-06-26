 namespace aima.core.logic.fol;

 
 

import aima.core.logic.fol.parsing.FOLVisitor;
import aima.core.logic.fol.parsing.ast.ConnectedSentence;
import aima.core.logic.fol.parsing.ast.Constant;
import aima.core.logic.fol.parsing.ast.Function;
import aima.core.logic.fol.parsing.ast.NotSentence;
import aima.core.logic.fol.parsing.ast.Predicate;
import aima.core.logic.fol.parsing.ast.QuantifiedSentence;
import aima.core.logic.fol.parsing.ast.Sentence;
import aima.core.logic.fol.parsing.ast.TermEquality;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ravi Mohan
 * 
 */
public class PredicateCollector : FOLVisitor {

	public PredicateCollector() {

	}

	@SuppressWarnings("unchecked")
	public List<Predicate> getPredicates(Sentence s) {
		return (List<Predicate>) s.accept(this, new List<Predicate>());
	}

	@SuppressWarnings("unchecked")
	public object visitPredicate(Predicate p, object arg) {
		List<Predicate> predicates = (List<Predicate>) arg;
		predicates.Add(p);
		return predicates;
	}

	public object visitTermEquality(TermEquality equality, object arg) {
		return arg;
	}

	public object visitVariable(Variable variable, object arg) {
		return arg;
	}

	public object visitConstant(Constant constant, object arg) {
		return arg;
	}

	public object visitFunction(Function function, object arg) {
		return arg;
	}

	public object visitNotSentence(NotSentence sentence, object arg) {
		sentence.getNegated().accept(this, arg);
		return arg;
	}

	public object visitConnectedSentence(ConnectedSentence sentence, object arg) {
		sentence.getFirst().accept(this, arg);
		sentence.getSecond().accept(this, arg);
		return arg;
	}

	public object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg) {
		sentence.getQuantified().accept(this, arg);
		return arg;
	}
}