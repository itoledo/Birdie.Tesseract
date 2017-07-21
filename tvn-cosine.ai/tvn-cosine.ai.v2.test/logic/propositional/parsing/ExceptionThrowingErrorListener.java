namespace aima.test.unit.logic.propositional.parsing;

using org.antlr.v4.runtime.BaseErrorListener;
using org.antlr.v4.runtime.RecognitionException;
using org.antlr.v4.runtime.Recognizer;

/**
 * Error handler which throws exception on any parsing error.
 */

public class ExceptionThrowingErrorListener extends BaseErrorListener {
	@Override
	public void syntaxError(Recognizer<?,?> recognizer,
            Object offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e) {
		throw new RuntimeException(e);
	}
}