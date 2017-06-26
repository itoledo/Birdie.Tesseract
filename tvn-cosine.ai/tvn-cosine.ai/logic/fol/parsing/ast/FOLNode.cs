 namespace aima.core.logic.fol.parsing.ast;

 

import aima.core.logic.common.ParseTreeNode;
import aima.core.logic.fol.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public interface FOLNode : ParseTreeNode {
	String getSymbolicName();

	bool isCompound();

	List<? : FOLNode> getArgs();

	Object accept(FOLVisitor v, object arg);

	FOLNode copy();
}
