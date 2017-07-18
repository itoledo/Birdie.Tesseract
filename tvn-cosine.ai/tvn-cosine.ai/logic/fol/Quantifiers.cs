namespace tvn.cosine.ai.logic.fol
{
    public class Quantifiers
    {
        public static final string FORALL = "FORALL";
	public static final string EXISTS = "EXISTS";

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
