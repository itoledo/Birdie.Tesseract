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
 * @author Ciaran O'Reilly
 * 
 */
public class StandardizeApartInPlace {
	//
	private static CollectAllVariables _collectAllVariables = new CollectAllVariables();

	public static int standardizeApart(Chain c, int saIdx) {
		List<Variable> variables = new List<Variable>();
		for (Literal l : c.getLiterals()) {
			collectAllVariables(l.getAtomicSentence(), variables);
		}

		return standardizeApart(variables, c, saIdx);
	}

	public static int standardizeApart(Clause c, int saIdx) {
		List<Variable> variables = new List<Variable>();
		for (Literal l : c.getLiterals()) {
			collectAllVariables(l.getAtomicSentence(), variables);
		}

		return standardizeApart(variables, c, saIdx);
	}

	//
	// PRIVATE METHODS
	//
	private static int standardizeApart(List<Variable> variables, object expr,
			int saIdx) {
		IDictionary<String, int> indexicals = new Dictionary<String, int>();
		for (Variable v : variables) {
			if (!indexicals.ContainsKey(v.getIndexedValue())) {
				indexicals.Add(v.getIndexedValue(), saIdx++);
			}
		}
		for (Variable v : variables) {
			Integer i = indexicals.get(v.getIndexedValue());
			if (null == i) {
				throw new  Exception("ERROR: duplicate var=" + v
						+ ", expr=" + expr);
			} else {
				v.setIndexical(i);
			}
		}

		return saIdx;
	}

	private static void collectAllVariables(Sentence s, List<Variable> vars) {
		s.accept(_collectAllVariables, vars);
	}
}

class CollectAllVariables : FOLVisitor {
	public CollectAllVariables() {

	}

	@SuppressWarnings("unchecked")
	public object visitVariable(Variable var, object arg) {
		List<Variable> variables = (List<Variable>) arg;
		variables.Add(var);
		return var;
	}

	@SuppressWarnings("unchecked")
	public object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg) {
		// Ensure I collect quantified variables too
		List<Variable> variables = (List<Variable>) arg;
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
