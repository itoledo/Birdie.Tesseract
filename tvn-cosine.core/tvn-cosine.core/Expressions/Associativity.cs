namespace tvn.cosine.expressions
{
    public class Associativity  
    {
        private Associativity(string description)
        {
            Description = description;
        }

        public string Description { get; }

        public static readonly Associativity Left = new Associativity("Left");
        public static readonly Associativity Right = new Associativity("Right");
    }
}
