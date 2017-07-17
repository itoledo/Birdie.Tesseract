namespace tvn.cosine.ai.logic.fol.domain
{
    public class FOLDomainAnswerLiteralAddedEvent extends FOLDomainEvent
    { 
    private String answerLiteralName;

    public FOLDomainAnswerLiteralAddedEvent(Object source,
            String answerLiteralName)
    {
        super(source);

        this.answerLiteralName = answerLiteralName;
    }

    public String getAnswerLiteralNameName()
    {
        return answerLiteralName;
    }

    @Override
    public void notifyListener(FOLDomainListener listener)
    {
        listener.answerLiteralNameAdded(this);
    }
}
}
