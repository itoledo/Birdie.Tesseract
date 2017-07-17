namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface FOLNode extends ParseTreeNode
    {
        String getSymbolicName();

        boolean isCompound();

        List<? extends FOLNode> getArgs();

        Object accept(FOLVisitor v, Object arg);

        FOLNode copy();
    }
}
