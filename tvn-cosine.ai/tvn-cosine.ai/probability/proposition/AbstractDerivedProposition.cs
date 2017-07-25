using tvn.cosine.ai.probability.proposition.api;

namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractDerivedProposition : AbstractProposition, IDerivedProposition
    { 
        private string name = null;

        public AbstractDerivedProposition(string name)
        {
            this.name = name;
        }
         
        public virtual string getDerivedName()
        {
            return name;
        } 
    }
}
