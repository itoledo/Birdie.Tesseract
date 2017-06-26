 namespace aima.core.logic.fol;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class Quantifiers {
	public static readonly string FORALL = "FORALL";
	public static readonly string EXISTS = "EXISTS";

	public static bool isFORALL(string quantifier) {
		return FORALL .Equals(quantifier);
	}

	public static bool isEXISTS(string quantifier) {
		return EXISTS .Equals(quantifier);
	}
}
