using tvn.cosine;
using tvn.cosine.api;

namespace tvn.cosine.ai.logic.fol.domain
{
    public abstract class FOLDomainEvent : EventObject
    {
        public FOLDomainEvent(object source)
                : base(source)
        { }

        public abstract void notifyListener(FOLDomainListener listener);
    }
}
