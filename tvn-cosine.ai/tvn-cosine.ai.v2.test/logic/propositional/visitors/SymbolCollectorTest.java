namespace aima.test.unit.logic.propositional.visitors;

using java.util.Set;

using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

using aima.core.logic.basic.propositional.parsing.PLParser;
using aima.core.logic.basic.propositional.parsing.ast.PropositionSymbol;
using aima.core.logic.basic.propositional.parsing.ast.Sentence;
using aima.core.logic.basic.propositional.visitors.SymbolCollector;
using aima.extra.logic.propositional.parser.PLParserWrapper;

/**
 * @author Ravi Mohan
 * 
 */
public class SymbolCollectorTest {
	private PLParser parser;

	@Before
	public void setUp() {
		parser = new PLParserWrapper();
	}

	[TestMethod]
	public void testCollectSymbolsFromComplexSentence() {
		Sentence sentence = (Sentence) parser.parse("(~B11 | P12 | P21) & (B11 | ~P12) & (B11 | ~P21)");
		Set<PropositionSymbol> s = SymbolCollector.getSymbolsFrom(sentence);
		Assert.assertEquals(3, s.size());
		Sentence b11 =  parser.parse("B11");
		Sentence p21 =  parser.parse("P21");
		Sentence p12 =  parser.parse("P12");
		Sentence p22 =  parser.parse("P22");
		Assert.assertTrue(s.contains(b11));
		Assert.assertTrue(s.contains(p21));
		Assert.assertTrue(s.contains(p12));
		Assert.assertFalse(s.contains(p22));
	}
}