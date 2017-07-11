namespace tvn.cosine.ai.logic.fol.domain
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLDomainAnswerLiteralAddedEvent : FOLDomainEvent
    {
        private string answerLiteralName;

        public FOLDomainAnswerLiteralAddedEvent(object source, string answerLiteralName)
            : base(source)
        { 
            this.answerLiteralName = answerLiteralName;
        }

        public string getAnswerLiteralNameName()
        {
            return answerLiteralName;
        }

        public override void notifyListener(FOLDomainListener listener)
        {
            listener.answerLiteralNameAdded(this);
        }
    } 
}
