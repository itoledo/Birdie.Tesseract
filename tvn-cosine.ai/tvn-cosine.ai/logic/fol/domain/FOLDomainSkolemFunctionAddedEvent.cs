﻿namespace tvn.cosine.ai.logic.fol.domain
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLDomainSkolemFunctionAddedEvent : FOLDomainEvent
    {
        private string skolemFunctionName;

        public FOLDomainSkolemFunctionAddedEvent(object source, string skolemFunctionName)
            : base(source)
        {
            this.skolemFunctionName = skolemFunctionName;
        }

        public string getSkolemConstantName()
        {
            return skolemFunctionName;
        }

        public override void notifyListener(FOLDomainListener listener)
        {
            listener.skolemFunctionAdded(this);
        }
    } 
}
