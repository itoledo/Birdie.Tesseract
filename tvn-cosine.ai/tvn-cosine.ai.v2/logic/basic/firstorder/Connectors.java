namespace aima.core.logic.basic.firstorder;

/**
 * @author Ravi Mohan
 * @author Anurag Rai
 */
public class Connectors {
	public static final String AND = "&";

	public static final String OR = "|";

	public static final String NOT = "~";

	public static final String IMPLIES = "=>";

	public static final String BICOND = "<=>";

	public static bool isAND(String connector) {
		return AND.Equals(connector);
	}

	public static bool isOR(String connector) {
		return OR.Equals(connector);
	}

	public static bool isNOT(String connector) {
		return NOT.Equals(connector);
	}

	public static bool isIMPLIES(String connector) {
		return IMPLIES.Equals(connector);
	}

	public static bool isBICOND(String connector) {
		return BICOND.Equals(connector);
	}
}