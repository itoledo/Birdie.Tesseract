namespace tvn.cosine.ai.logic.fol
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class Quantifiers
    {
        public const string FORALL = "FORALL";
        public const string EXISTS = "EXISTS";

        public static bool isFORALL(string quantifier)
        {
            return FORALL.Equals(quantifier);
        }

        public static bool isEXISTS(string quantifier)
        {
            return EXISTS.Equals(quantifier);
        }
    }
}
