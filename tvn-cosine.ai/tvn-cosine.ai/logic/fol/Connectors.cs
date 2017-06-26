 namespace aima.core.logic.fol;

/**
 * @author Ravi Mohan
 * 
 */
public class Connectors {
	public static readonly string AND = "AND";

	public static readonly string OR = "OR";

	public static readonly string NOT = "NOT";

	public static readonly string IMPLIES = "=>";

	public static readonly string BICOND = "<=>";

	public static bool isAND(string connector) {
		return AND .Equals(connector);
	}

	public static bool isOR(string connector) {
		return OR .Equals(connector);
	}

	public static bool isNOT(string connector) {
		return NOT .Equals(connector);
	}

	public static bool isIMPLIES(string connector) {
		return IMPLIES .Equals(connector);
	}

	public static bool isBICOND(string connector) {
		return BICOND .Equals(connector);
	}
}