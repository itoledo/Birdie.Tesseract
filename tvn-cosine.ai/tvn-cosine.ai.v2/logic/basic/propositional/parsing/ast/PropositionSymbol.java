namespace aima.core.logic.basic.propositional.parsing.ast;

using javax.lang.model.SourceVersion;

/**
 * Artificial Intelligence A Modern Approach (4th Edition): page ???.<br>
 * <br>
 * <b>Proposition Symbol:</b> Each such symbol stands for a proposition that can
 * be true or false. There are two proposition symbols with fixed meanings:
 * <i>True</i> the always-true proposition and <i>False</i> the always-false
 * proposition.<br>
 * <br>
 * <b>Note</b>: While the book states:<br>
 * 'We use symbols that start with an upper case letter and may contain other
 * letters or subscripts'. In this implementation we allow any legal java
 * identifier to stand in for a proposition symbol.
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 * 
 * @see SourceVersion#isIdentifier(CharSequence)
 */
public class PropositionSymbol extends AtomicSentence {
	//
	public static final String TRUE_SYMBOL  = "True";
	public static final String FALSE_SYMBOL = "False";
	public static final PropositionSymbol TRUE  = new PropositionSymbol(TRUE_SYMBOL);
	public static final PropositionSymbol FALSE = new PropositionSymbol(FALSE_SYMBOL);
	//
	private String symbol;

	/**
	 * Constructor.
	 * 
	 * @param symbol
	 *            the symbol uniquely identifying the proposition.
	 */
	public PropositionSymbol(String symbol) {
		// Ensure differing cases for the 'True' and 'False'
		// propositional constants are represented in a canonical form.
		if (TRUE_SYMBOL.equalsIgnoreCase(symbol)) {
			this.symbol = TRUE_SYMBOL;
		} else if (FALSE_SYMBOL.equalsIgnoreCase(symbol)) {
			this.symbol = FALSE_SYMBOL;
		} else if (isPropositionSymbol(symbol)){
			this.symbol = symbol;
		}
		else {
			throw new IllegalArgumentException("Not a legal proposition symbol: "+symbol);
		}
	}

	/**
	 * 
	 * @return true if this is the always 'True' proposition symbol, false
	 *         otherwise.
	 */
	public bool isAlwaysTrue() {
		return TRUE_SYMBOL.Equals(symbol);
	}

	/**
	 * 
	 * @return true if the symbol passed in is the always 'True' proposition
	 *         symbol, false otherwise.
	 */
	public static bool isAlwaysTrueSymbol(String symbol) {
		return TRUE_SYMBOL.equalsIgnoreCase(symbol);
	}

	/**
	 * 
	 * @return true if this is the always 'False' proposition symbol, false
	 *         other.
	 */
	public bool isAlwaysFalse() {
		return FALSE_SYMBOL.Equals(symbol);
	}

	/**
	 * 
	 * @return true if the symbol passed in is the always 'False' proposition
	 *         symbol, false other.
	 */
	public static bool isAlwaysFalseSymbol(String symbol) {
		return FALSE_SYMBOL.equalsIgnoreCase(symbol);
	}

	/**
	 * Determine if the given symbol is a legal proposition symbol.
	 * 
	 * @param symbol
	 *            a symbol to be tested.
	 * @return true if the given symbol is a legal proposition symbol, false
	 *         otherwise.
	 */
	public static bool isPropositionSymbol(String symbol) {
		return SourceVersion.isIdentifier(symbol);
	}

	/**
	 * Determine if the given character can be at the beginning of a proposition
	 * symbol.
	 * 
	 * @param ch
	 *            a character.
	 * @return true if the given character can be at the beginning of a
	 *         proposition symbol representation, false otherwise.
	 */
	public static bool isPropositionSymbolIdentifierStart(char ch) {
		return Character.isJavaIdentifierStart(ch);
	}

	/**
	 * Determine if the given character is part of a proposition symbol.
	 * 
	 * @param ch
	 *            a character.
	 * @return true if the given character is part of a proposition symbols
	 *         representation, false otherwise.
	 */
	public static bool isPropositionSymbolIdentifierPart(char ch) {
		return Character.isJavaIdentifierPart(ch);
	}

	/**
	 * 
	 * @return the symbol uniquely identifying the proposition.
	 */
	public String getSymbol() {
		return symbol;
	}

	 
	public override bool Equals(object o) {
		if (this == o) {
			return true;
		}
		if ((o == null) || (this.getClass() != o.getClass())) {
			return false;
		}
		PropositionSymbol sym = (PropositionSymbol) o;
		return symbol.Equals(sym.symbol);

	}

	 
	public override int GetHashCode() {
		return symbol.GetHashCode();
	}

	 
	public override string ToString() {
		return getSymbol();
	}
}