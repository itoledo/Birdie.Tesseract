namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractDerivedProposition<T> : AbstractProposition<T>, DerivedProposition<T>
    { 
        private string name = null;

        public AbstractDerivedProposition(string name)
        {
            this.name = name;
        }

        //
        // START-DerivedProposition
        public string getDerivedName()
        {
            return name;
        }

        // END-DerivedProposition
        //
    }
}
