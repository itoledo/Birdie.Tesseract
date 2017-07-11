namespace tvn.cosine.ai.logic.fol.domain
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLDomainSkolemConstantAddedEvent : FOLDomainEvent
    {
        private string skolemConstantName;

        public FOLDomainSkolemConstantAddedEvent(object source, string skolemConstantName)
            : base(source)
        {
            this.skolemConstantName = skolemConstantName;
        }

        public string getSkolemConstantName()
        {
            return skolemConstantName;
        }

        public override void notifyListener(FOLDomainListener listener)
        {
            listener.skolemConstantAdded(this);
        }
    } 
}
