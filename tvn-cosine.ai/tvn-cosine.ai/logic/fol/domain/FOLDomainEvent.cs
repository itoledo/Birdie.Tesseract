using System;

namespace tvn.cosine.ai.logic.fol.domain
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public abstract class FOLDomainEvent : EventArgs
    {
        public object Source { get; }

        public FOLDomainEvent(object source)

        {
            this.Source = source;
        }

        public abstract void notifyListener(FOLDomainListener listener);
    }

}
