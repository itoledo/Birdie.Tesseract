namespace tvn.cosine.ai.search.csp
{
    /**
     * A variable is a distinguishable object with a name.
     *
     * @author Ruediger Lunde
     */
    public class Variable
    {
        private final string name;

    public Variable(string name)
        {
            this.name = name;
        }

        public final string getName()
        {
            return name;
        }

        public override string ToString()
        {
            return name;
        }

        /** Variables with equal names are equal. */
         
        public final bool equals(object obj)
        {
            return obj is Variable && this.name.Equals(((Variable)obj).name);
        }

         
        public final int hashCode()
        {
            return name.GetHashCode();
        }
    }

}
