using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public interface FOLNode : ParseTreeNode
    {
        String getSymbolicName();

        boolean isCompound();

        List<? extends FOLNode> getArgs();

        Object accept(FOLVisitor v, Object arg);

        FOLNode copy();
    }

}
