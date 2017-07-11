using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.parsing
{
    /**
     * @author Ravi Mohan
     * 
     */
    public interface FOLVisitor
    {
        object visitPredicate(Predicate p, object arg);
        
        object visitTermEquality(TermEquality equality, object arg);
        
        object visitVariable(Variable variable, object arg);
        
        object visitConstant(Constant constant, object arg);
        
        object visitFunction(Function function, object arg);
        
        object visitNotSentence(NotSentence sentence, object arg);
        
        object visitConnectedSentence(ConnectedSentence sentence, object arg);
        
        object visitQuantifiedSentence(QuantifiedSentence sentence, object arg);
    }
}
