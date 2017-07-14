using System.Collections.Generic;
using tvn.cosine.ai.logic.common;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public interface FOLNode : ParseTreeNode
    {
        string getSymbolicName();

        bool isCompound();

        IList<T> getArgs<T>() where T : FOLNode;

        object accept(FOLVisitor v, object arg);

        FOLNode copy();
    } 
}
