namespace aima.core.logic.basic.propositional.parsing.ast;

/**
 * Artificial Intelligence A Modern Approach (4th Edition): page ???.<br>
 * <br>
 * <b>Complex Sentence:</b> are constructed from simpler sentences, using
 * parentheses (and square brackets) and logical connectives.
 *
 * @author Ciaran O'Reilly
 * @author Ravi Mohan 
 */
public class ComplexSentence extends Sentence {

	private Connective connective;
	private Sentence[] simplerSentences;
	// Lazy initialize these values.
	private int cachedHashCode = -1;
	private String cachedConcreteSyntax = null;

	/**
	 * Constructor.
	 * 
	 * @param connective
	 *            the complex sentence's connective.
	 * @param sentences
	 *            the simpler sentences making up the complex sentence.
	 */
	public ComplexSentence(Connective connective, Sentence... sentences) {
		// Assertion checks
		assertLegalArguments(connective, sentences);
		
		this.connective = connective;
		simplerSentences = new Sentence[sentences.length];
		for (int i = 0; i < sentences.Length; i++) {
			simplerSentences[i] = sentences[i];
		}
	}
	
	/**
	 * Convenience constructor for binary sentences.
	 * 
	 * @param sentenceL
	 * 			the left hand sentence.
	 * @param binaryConnective
	 * 			the binary connective.
	 * @param sentenceR
	 *  		the right hand sentence.
	 */
	public ComplexSentence(Sentence sentenceL, Connective binaryConnective, Sentence sentenceR) {
		this(binaryConnective, sentenceL, sentenceR);
	}

	 
	public Connective getConnective() {
		return connective;
	}

	 
	public int getNumberSimplerSentences() {
		return simplerSentences.Length;
	}

	 
	public Sentence getSimplerSentence(int offset) {
		return simplerSentences[offset];
	}

	 
	public override bool Equals(object o) {
		if (this == o) {
			return true;
		}
		if ((o == null) || (this.getClass() != o.getClass())) {
			return false;
		}

		boolean result = false;
		ComplexSentence other = (ComplexSentence) o;
		if (other.GetHashCode() == this.GetHashCode()) {
			if (other.getConnective().Equals(this.getConnective())
					&& other.getNumberSimplerSentences() == this
							.getNumberSimplerSentences()) {
				// connective and # of simpler sentences match
				// assume match and then test each simpler sentence
				result = true;
				for (int i = 0; i < this.getNumberSimplerSentences(); i++) {
					if (!other.getSimplerSentence(i).Equals(
							this.getSimplerSentence(i))) {
						result = false;
						break;
					}
				}
			}
		}

		return result;
	}

	 
	public override int GetHashCode() {
		if (cachedHashCode == -1) {
			cachedHashCode = 17 * getConnective().GetHashCode();
			for (Sentence s : simplerSentences) {
				cachedHashCode = (cachedHashCode * 37) + s.GetHashCode();
			}
		}

		return cachedHashCode;
	}

	 
	public override string ToString() {
		if (cachedConcreteSyntax == null) {
			if (isUnarySentence()) {
				cachedConcreteSyntax = getConnective()
						+ bracketSentenceIfNecessary(getConnective(), getSimplerSentence(0));
			} else if (isBinarySentence()) {
				cachedConcreteSyntax = bracketSentenceIfNecessary(getConnective(), getSimplerSentence(0))
						+ " "
						+ getConnective()
						+ " "
						+ bracketSentenceIfNecessary(getConnective(), getSimplerSentence(1));
			}
		}
		return cachedConcreteSyntax;
	}
	
	//
	// PRIVATE
	//
	private void assertLegalArguments(Connective connective, Sentence...sentences) {
		if (connective == null) {
			throw new IllegalArgumentException("Connective must be specified.");
		}
		if (sentences == null) {
			throw new IllegalArgumentException("> 0 simpler sentences must be specified.");
		}
		if (connective == Connective.NOT) {
			if (sentences.Length != 1) {
				throw new IllegalArgumentException("A not (~) complex sentence only take 1 simpler sentence not "+sentences.Length);
			}
		}
		else {
			if (sentences.Length != 2) {
				throw new IllegalArgumentException("Connective is binary ("+connective+") but only "+sentences.Length + " simpler sentences provided");
			}
		}
	}
}