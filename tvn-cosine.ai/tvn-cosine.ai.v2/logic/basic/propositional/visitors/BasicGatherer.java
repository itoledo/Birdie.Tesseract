namespace aima.core.logic.basic.propositional.visitors;

using java.util.Set;

using aima.core.logic.basic.propositional.parsing.PLVisitor;
using aima.core.logic.basic.propositional.parsing.ast.ComplexSentence;
using aima.core.logic.basic.propositional.parsing.ast.PropositionSymbol;
using aima.core.util.SetOps;

/**
 * Super class of Visitors that are "read only" and gather information from an
 * existing parse tree .
 * 
 * @author Ravi Mohan
 * 
 * @param <T>
 *            the type of elements to be gathered.
 */
public abstract class BasicGatherer<T> implements PLVisitor<Set<T>, ISet<T>> {

	 
	public ISet<T> visitPropositionSymbol(PropositionSymbol s, ISet<T> arg) {
		return arg;
	}

	 
	public ISet<T> visitUnarySentence(ComplexSentence s, ISet<T> arg) {
		return SetOps.union(arg, s.getSimplerSentence(0).accept(this, arg));
	}

	 
	public ISet<T> visitBinarySentence(ComplexSentence s, ISet<T> arg) {
		Set<T> termunion = SetOps.union(
				s.getSimplerSentence(0).accept(this, arg), s
						.getSimplerSentence(1).accept(this, arg));
		return SetOps.union(arg, termunion);
	}
}
