using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.common;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface FOLNode : ParseTreeNode
    {
        string getSymbolicName();

        bool isCompound();

        IQueue<T> getArgs<T>() where T : FOLNode;

        object accept(FOLVisitor v, object arg);

        FOLNode copy();
    }
}
