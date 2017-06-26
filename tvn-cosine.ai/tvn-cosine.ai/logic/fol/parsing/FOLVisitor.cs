 namespace aima.core.logic.fol.parsing;

import aima.core.logic.fol.parsing.ast.ConnectedSentence;
import aima.core.logic.fol.parsing.ast.Constant;
import aima.core.logic.fol.parsing.ast.Function;
import aima.core.logic.fol.parsing.ast.NotSentence;
import aima.core.logic.fol.parsing.ast.Predicate;
import aima.core.logic.fol.parsing.ast.QuantifiedSentence;
import aima.core.logic.fol.parsing.ast.TermEquality;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * @author Ravi Mohan
 * 
 */
public interface FOLVisitor {
	public object visitPredicate(Predicate p, object arg);

	public object visitTermEquality(TermEquality equality, object arg);

	public object visitVariable(Variable variable, object arg);

	public object visitConstant(Constant constant, object arg);

	public object visitFunction(Function function, object arg);

	public object visitNotSentence(NotSentence sentence, object arg);

	public object visitConnectedSentence(ConnectedSentence sentence, object arg);

	public object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg);
}
