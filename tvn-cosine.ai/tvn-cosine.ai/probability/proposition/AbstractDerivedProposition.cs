namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractDerivedProposition : AbstractProposition, DerivedProposition
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
