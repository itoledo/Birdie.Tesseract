namespace aima.core.logic.basic.propositional.parsing;

using aima.core.logic.basic.propositional.parsing.ast.Sentence;

public interface PLParser {
	public Sentence parse(String stringToBeParsed);
}