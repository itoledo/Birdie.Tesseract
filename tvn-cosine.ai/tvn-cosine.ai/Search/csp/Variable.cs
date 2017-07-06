namespace tvn.cosine.ai.search.csp
{
    /**
     * A variable is a distinguishable object with a name.
     *
     * @author Ruediger Lunde
     */
    public class Variable
    {
        public string Name { get; }

        public Variable(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        /** Variables with equal names are equal. */ 
        public override bool Equals(object obj)
        {
            return obj is Variable && this.Name.Equals(((Variable)obj).Name);
        }
         
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
