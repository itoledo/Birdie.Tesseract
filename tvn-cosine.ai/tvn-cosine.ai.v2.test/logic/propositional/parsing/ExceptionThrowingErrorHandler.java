namespace aima.test.unit.logic.propositional.parsing;

using org.antlr.v4.runtime.DefaultErrorStrategy;
using org.antlr.v4.runtime.InputMismatchException;
using org.antlr.v4.runtime.Parser;
using org.antlr.v4.runtime.RecognitionException;
using org.antlr.v4.runtime.Token;

public class ExceptionThrowingErrorHandler extends DefaultErrorStrategy {
	@Override
	public void recover(Parser recognizer, RecognitionException e) {
		throw new RuntimeException(e);
	}

	@Override
	public Token recoverInline(Parser recognizer) throws RecognitionException {
		throw new RuntimeException(new InputMismatchException(recognizer));
	}

	@Override
	public void sync(Parser recognizer) throws RecognitionException {
	}
}