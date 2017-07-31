using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.common;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface FOLNode : ParseTreeNode
    {
        string getSymbolicName(); 
        bool isCompound(); 
        ICollection<FOLNode> getArgs(); 
        object accept(FOLVisitor v, object arg); 
        FOLNode copy();
    }
}
