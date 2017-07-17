namespace tvn.cosine.ai.logic.fol.domain
{
    public interface FOLDomainListener
    {
        void skolemConstantAdded(FOLDomainSkolemConstantAddedEvent event);


    void skolemFunctionAdded(FOLDomainSkolemFunctionAddedEvent event);


    void answerLiteralNameAdded(FOLDomainAnswerLiteralAddedEvent event);
        }
    }
