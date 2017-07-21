namespace aima.test.unit.logic.propositional.parsing;

using org.junit.Test;

using aima.core.logic.basic.propositional.parsing.ast.ComplexSentence;
using aima.core.logic.basic.propositional.parsing.ast.Connective;
using aima.core.logic.basic.propositional.parsing.ast.PropositionSymbol;
using aima.core.logic.basic.propositional.parsing.ast.Sentence;

public class ComplexSentenceTest {
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_1() {
		new ComplexSentence(null, new Sentence[] {new PropositionSymbol("A"), new PropositionSymbol("B")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_2() {
		new ComplexSentence(Connective.NOT, (Sentence[]) null);
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_3() {
		new ComplexSentence(Connective.NOT, new Sentence[]{});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_4() {
		new ComplexSentence(Connective.NOT, new Sentence[] {new PropositionSymbol("A"), new PropositionSymbol("B")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_5() {
		new ComplexSentence(Connective.AND, new Sentence[]{new PropositionSymbol("A")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_6() {
		new ComplexSentence(Connective.AND, new Sentence[]{new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_7() {
		new ComplexSentence(Connective.OR, new Sentence[]{new PropositionSymbol("A")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_8() {
		new ComplexSentence(Connective.OR, new Sentence[]{new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_9() {
		new ComplexSentence(Connective.IMPLICATION, new Sentence[]{new PropositionSymbol("A")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_10() {
		new ComplexSentence(Connective.IMPLICATION, new Sentence[]{new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_11() {
		new ComplexSentence(Connective.BICONDITIONAL, new Sentence[]{new PropositionSymbol("A")});
	}
	
	[TestMethod][ExpectedException(typeof(IllegalArgumentException))]
	public void test_IllegalArgumentOnConstruction_12() {
		new ComplexSentence(Connective.BICONDITIONAL, new Sentence[]{new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C")});
	}
}