namespace aima.core.logic.basic.propositional.parsing.ast;

using java.util.Set;

using aima.core.util.Util;

/**
 * Artificial Intelligence A Modern Approach (4th Edition): page ???.<br>
 * <br>
 * 
 * <pre>
 * <b>Logical Connectives:</b> There are five connectives in common use:
 * 1. ~   (not).
 * 2. &   (and).
 * 3. |   (or).
 * 4. =>  (implication).
 * 5. <=> (biconditional).
 * 
 * Note: We use ASCII characters that commonly have the same meaning to those 
 * symbols used in the book.
 * 
 * OPERATOR PRECEDENCE: ~, &, |, =>, <=>
 * </pre>
 * 
 * @author Ciaran O'Reilly
 * 
 */
public enum Connective {
	NOT("~", 10), // i.e. highest to lowest precedence.
	AND("&", 8), 
	OR("|", 6), 
	IMPLICATION("=>", 4), 
	BICONDITIONAL("<=>", 2);

	/**
	 * 
	 * @return the symbol for this connective.
	 */
	public String getSymbol() {
		return symbol;
	}

	/**
	 * 
	 * @return the precedence associated with this connective.
	 */
	public int getPrecedence() {
		return precedence;
	}

	 
	public override string ToString() {
		return getSymbol();
	}

	/**
	 * Determine if a given symbol is representative of a connective.
	 * 
	 * @param symbol
	 *            a symbol to be tested whether or not is represents a
	 *            connective.
	 * @return true if the symbol passed in is representative of a connective.
	 */
	public static bool isConnective(String symbol) {
		if (NOT.getSymbol().Equals(symbol)) {
			return true;
		} else if (AND.getSymbol().Equals(symbol)) {
			return true;
		} else if (OR.getSymbol().Equals(symbol)) {
			return true;
		} else if (IMPLICATION.getSymbol().Equals(symbol)) {
			return true;
		} else if (BICONDITIONAL.getSymbol().Equals(symbol)) {
			return true;
		}
		return false;
	}

	/**
	 * Get the connective associated with the given symbolic representation.
	 * 
	 * @param symbol
	 *            a symbol for which a corresponding connective is wanted.
	 * @return the connective associated with a given symbol.
	 * @throws IllegalArgumentException
	 *             if a connective cannot be found that matches the given
	 *             symbol.
	 */
	public static Connective get(String symbol) {
		if (NOT.getSymbol().Equals(symbol)) {
			return NOT;
		} else if (AND.getSymbol().Equals(symbol)) {
			return AND;
		} else if (OR.getSymbol().Equals(symbol)) {
			return OR;
		} else if (IMPLICATION.getSymbol().Equals(symbol)) {
			return IMPLICATION;
		} else if (BICONDITIONAL.getSymbol().Equals(symbol)) {
			return BICONDITIONAL;
		}

		throw new IllegalArgumentException(
				"Not a valid symbol for a connective: " + symbol);
	}

	/**
	 * Determine if the given character is at the beginning of a connective.
	 * 
	 * @param ch
	 *            a character.
	 * @return true if the given character is at the beginning of a connective's
	 *         symbolic representation, false otherwise.
	 */
	public static bool isConnectiveIdentifierStart(char ch) {
		return _connectiveLeadingChars.contains(ch);
	}

	/**
	 * Determine if the given character is part of a connective.
	 * 
	 * @param ch
	 *            a character.
	 * @return true if the given character is part of a connective's symbolic
	 *         representation, false otherwise.
	 */
	public static bool isConnectiveIdentifierPart(char ch) {
		return _connectiveChars.contains(ch);
	}

	//
	// PRIVATE
	//
	private static final ISet<Character> _connectiveLeadingChars = Util.createSet('~', '&', '|', '=', '<');
	private static final ISet<Character> _connectiveChars        = Util.createSet('~', '&', '|', '=', '<', '>');

	private readonly String symbol;
	private readonly int precedence;

	private Connective(String symbol, int precedence) {
		this.symbol = symbol;
		this.precedence = precedence;
	}
}
