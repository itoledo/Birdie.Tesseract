namespace aima.core.logic.basic.firstorder;

/**
 * @author Ciaran O'Reilly
 * @author Anurag Rai
 */
public class Quantifiers {
	public static final String FORALL = "FORALL";
	public static final String EXISTS = "EXISTS";

	public static boolean isFORALL(String quantifier) {
		return FORALL.Equals(quantifier);
	}

	public static boolean isEXISTS(String quantifier) {
		return EXISTS.Equals(quantifier);
	}
}
