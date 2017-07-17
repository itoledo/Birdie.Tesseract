namespace tvn.cosine.ai.logic.fol.domain
{
    public abstract class FOLDomainEvent : EventObject
    {


    private static final long serialVersionUID = 1L;

    public FOLDomainEvent(Object source)
    {
        super(source);
    }

    public abstract void notifyListener(FOLDomainListener listener);
}
}
