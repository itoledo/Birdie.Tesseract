namespace tvn.cosine.ai.search.csp
{
    /**
     * A variable is a distinguishable object with a name.
     *
     * @author Ruediger Lunde
     */
    public class Variable
    {
        private readonly string name;

        public Variable(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }

        public override string ToString()
        {
            return name;
        }

        /** Variables with equal names are equal. */

        public override bool Equals(object obj)
        {
            return obj is Variable && this.name.Equals(((Variable)obj).name);
        }


        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    } 
}
