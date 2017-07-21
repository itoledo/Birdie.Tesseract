namespace aima.core.logic.basic.firstorder.parsing.ast;

using java.util.List;

using aima.core.logic.basic.firstorder.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 * @author Anurag Rai
 */
public interface FOLNode {
	String getSymbolicName();

	boolean isCompound();

	List<? extends FOLNode> getArgs();

	Object accept(FOLVisitor v, Object arg);

	FOLNode copy();
}
