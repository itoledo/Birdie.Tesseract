namespace tvn.cosine.ai.logic.fol.domain
{

    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface FOLDomainListener
    {
        void skolemConstantAdded(FOLDomainSkolemConstantAddedEvent even); 
        void skolemFunctionAdded(FOLDomainSkolemFunctionAddedEvent even); 
        void answerLiteralNameAdded(FOLDomainAnswerLiteralAddedEvent even);
    }
}
