 namespace aima.core.logic.fol.parsing;

import java.util.HashSet;
 

import aima.core.logic.common.Lexer;
import aima.core.logic.common.LexerException;
import aima.core.logic.common.LogicTokenTypes;
import aima.core.logic.common.Token;
import aima.core.logic.fol.Connectors;
import aima.core.logic.fol.Quantifiers;
import aima.core.logic.fol.domain.FOLDomain;

/**
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 * 
 */
public class FOLLexer : Lexer {
	private FOLDomain domain;
	private ISet<string> connectors, quantifiers;

	public FOLLexer(FOLDomain domain) {
		this.domain = domain;

		connectors = new HashSet<string>();
		connectors.Add(Connectors.NOT);
		connectors.Add(Connectors.AND);
		connectors.Add(Connectors.OR);
		connectors.Add(Connectors.IMPLIES);
		connectors.Add(Connectors.BICOND);

		quantifiers = new HashSet<string>();
		quantifiers.Add(Quantifiers.FORALL);
		quantifiers.Add(Quantifiers.EXISTS);
	}

	public FOLDomain getFOLDomain() {
		return domain;
	}

	 
	public Token nextToken() {
		int startPosition = getCurrentPositionInInput();
		if (lookAhead(1) == '(') {
			consume();
			return new Token(LogicTokenTypes.LPAREN, "(", startPosition);

		} else if (lookAhead(1) == ')') {
			consume();
			return new Token(LogicTokenTypes.RPAREN, ")", startPosition);

		} else if (lookAhead(1) == ',') {
			consume();
			return new Token(LogicTokenTypes.COMMA, ",", startPosition);

		} else if (identifierDetected()) {
			// Console.WriteLine("identifier detected");
			return identifier();
		} else if (Character.isWhitespace(lookAhead(1))) {
			consume();
			return nextToken();
		} else if (lookAhead(1) == (char) -1) {
			return new Token(LogicTokenTypes.EOI, "EOI", startPosition);
		} else {
			throw new LexerException("Lexing error on character " + lookAhead(1)+" at position "+getCurrentPositionInInput(), getCurrentPositionInInput());
		}
	}

	private Token identifier() {
		int startPosition = getCurrentPositionInInput();
		StringBuilder sbuf = new StringBuilder();
		while ((Character.isJavaIdentifierPart(lookAhead(1)))
				|| partOfConnector()) {
			sbuf.Append(lookAhead(1));
			consume();
		}
		String readString = new String(sbuf);
		// Console.WriteLine(readString);
		if (connectors.Contains(readString)) {
			return new Token(LogicTokenTypes.CONNECTIVE, readString, startPosition);
		} else if (quantifiers.Contains(readString)) {
			return new Token(LogicTokenTypes.QUANTIFIER, readString, startPosition);
		} else if (domain.getPredicates().Contains(readString)) {
			return new Token(LogicTokenTypes.PREDICATE, readString, startPosition);
		} else if (domain.getFunctions().Contains(readString)) {
			return new Token(LogicTokenTypes.FUNCTION, readString, startPosition);
		} else if (domain.getConstants().Contains(readString)) {
			return new Token(LogicTokenTypes.CONSTANT, readString, startPosition);
		} else if (isVariable(readString)) {
			return new Token(LogicTokenTypes.VARIABLE, readString, startPosition);
		} else if (readString .Equals("=")) {
			return new Token(LogicTokenTypes.EQUALS, readString, startPosition);
		} else {
			throw new LexerException("Lexing error on character " + lookAhead(1) + " at position "+getCurrentPositionInInput(), getCurrentPositionInInput());
		}
	}

	private bool isVariable(string s) {
		return (Character.isLowerCase(s.charAt(0)));
	}

	private bool identifierDetected() {
		return (Character.isJavaIdentifierStart(lookAhead(1)))
				|| partOfConnector();
	}

	private bool partOfConnector() {
		return (lookAhead(1) == '=') || (lookAhead(1) == '<')
				|| (lookAhead(1) == '>');
	}
}