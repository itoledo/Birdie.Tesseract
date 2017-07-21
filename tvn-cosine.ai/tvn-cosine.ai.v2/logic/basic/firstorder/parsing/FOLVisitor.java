namespace aima.core.logic.basic.firstorder.parsing;

using aima.core.logic.basic.firstorder.parsing.ast.ConnectedSentence;
using aima.core.logic.basic.firstorder.parsing.ast.Constant;
using aima.core.logic.basic.firstorder.parsing.ast.Function;
using aima.core.logic.basic.firstorder.parsing.ast.NotSentence;
using aima.core.logic.basic.firstorder.parsing.ast.Predicate;
using aima.core.logic.basic.firstorder.parsing.ast.QuantifiedSentence;
using aima.core.logic.basic.firstorder.parsing.ast.TermEquality;
using aima.core.logic.basic.firstorder.parsing.ast.Variable;

/**
 * @author Ravi Mohan
 * 
 */
public interface FOLVisitor {
	public Object visitPredicate(Predicate p, Object arg);

	public Object visitTermEquality(TermEquality equality, Object arg);

	public Object visitVariable(Variable variable, Object arg);

	public Object visitConstant(Constant constant, Object arg);

	public Object visitFunction(Function function, Object arg);

	public Object visitNotSentence(NotSentence sentence, Object arg);

	public Object visitConnectedSentence(ConnectedSentence sentence, Object arg);

	public Object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg);
}
