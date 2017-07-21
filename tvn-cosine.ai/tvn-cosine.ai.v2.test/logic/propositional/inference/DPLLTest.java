namespace aima.test.unit.logic.propositional.inference;

using java.util.ArrayList;
using java.util.Arrays;
using java.util.Collection;
using java.util.List;
using java.util.Set;

using org.junit.Assert;
using org.junit.Test;
using org.junit.runner.RunWith;
using org.junit.runners.Parameterized;
using org.junit.runners.Parameterized.Parameters;

using aima.core.logic.api.propositional.DPLL;
using aima.core.logic.basic.propositional.inference.DPLLSatisfiable;
using aima.core.logic.basic.propositional.inference.OptimizedDPLL;
using aima.core.logic.basic.propositional.kb.BasicKnowledgeBase;
using aima.core.logic.basic.propositional.kb.data.Clause;
using aima.core.logic.basic.propositional.kb.data.Model;
using aima.core.logic.basic.propositional.parsing.PLParser;
using aima.core.logic.basic.propositional.parsing.ast.PropositionSymbol;
using aima.core.logic.basic.propositional.parsing.ast.Sentence;
using aima.core.logic.basic.propositional.visitors.ConvertToConjunctionOfClauses;
using aima.core.logic.basic.propositional.visitors.SymbolCollector;
using aima.extra.logic.propositional.parser.PLParserWrapper;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
@RunWith(Parameterized.class)
public class DPLLTest {

	private DPLL     dpll;
	private PLParser parser;
	
	@Parameters(name = "{index}: dpll={0}")
    public static Collection<Object[]> inferenceAlgorithmSettings() {
        return Arrays.asList(new Object[][] {
        		{new DPLLSatisfiable()}, 
        		{new OptimizedDPLL()}   
        });
    }

	public DPLLTest(DPLL dpll) {
		this.dpll   = dpll;
		this.parser = new PLParserWrapper();
	}

	[TestMethod]
	public void testDPLLReturnsTrueWhenAllClausesTrueInModel() {
		Model model = new Model();
		model = model.union(new PropositionSymbol("A"), true).union(
				new PropositionSymbol("B"), true);
		Sentence sentence = parser.parse("A & B & (A | B)");
		Set<Clause> clauses = ConvertToConjunctionOfClauses.convert(sentence)
				.getClauses();
		List<PropositionSymbol> symbols = new ArrayList<PropositionSymbol>(
				SymbolCollector.getSymbolsFrom(sentence));

		boolean satisfiable = dpll.dpll(clauses, symbols, model);
		Assert.assertEquals(true, satisfiable);
	}

	[TestMethod]
	public void testDPLLReturnsFalseWhenOneClauseFalseInModel() {
		Model model = new Model();
		model = model.union(new PropositionSymbol("A"), true).union(
				new PropositionSymbol("B"), false);
		Sentence sentence = parser.parse("(A | B) & (A => B)");
		Set<Clause> clauses = ConvertToConjunctionOfClauses.convert(sentence)
				.getClauses();
		List<PropositionSymbol> symbols = new ArrayList<PropositionSymbol>(
				SymbolCollector.getSymbolsFrom(sentence));

		boolean satisfiable = dpll.dpll(clauses, symbols, model);
		Assert.assertEquals(false, satisfiable);
	}

	[TestMethod]
	public void testDPLLSucceedsWithAandNotA() {
		Sentence sentence = parser.parse("A & ~A");
		boolean satisfiable = dpll.dpllSatisfiable(sentence);
		Assert.assertEquals(false, satisfiable);
	}

	[TestMethod]
	public void testDPLLSucceedsWithChadCarffsBugReport() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());

		kb.tell("B12 <=> P11 | P13 | P22 | P02");
		kb.tell("B21 <=> P20 | P22 | P31 | P11");
		kb.tell("B01 <=> P00 | P02 | P11");
		kb.tell("B10 <=> P11 | P20 | P00");
		kb.tell("~B21");
		kb.tell("~B12");
		kb.tell("B10");
		kb.tell("B01");

		Assert.assertTrue(dpll.isEntailed(kb, parser.parse("P00")));
		Assert.assertFalse(dpll.isEntailed(kb, parser.parse("~P00")));
	}

	[TestMethod]
	public void testDPLLSucceedsWithStackOverflowBugReport1() {
		Sentence sentence = (Sentence) parser.parse("(A | ~A) & (A | B)");
		Assert.assertTrue(dpll.dpllSatisfiable(sentence));
	}

	[TestMethod]
	public void testDPLLSucceedsWithChadCarffsBugReport2() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());
		kb.tell("B10 <=> P11 | P20 | P00");
		kb.tell("B01 <=> P00 | P02 | P11");
		kb.tell("B21 <=> P20 | P22 | P31 | P11");
		kb.tell("B12 <=> P11 | P13 | P22 | P02");
		kb.tell("~B21");
		kb.tell("~B12");
		kb.tell("B10");
		kb.tell("B01");

		Assert.assertTrue(dpll.isEntailed(kb, parser.parse("P00")));
		Assert.assertFalse(dpll.isEntailed(kb, parser.parse("~P00")));
	}

	[TestMethod]
	public void testIssue66() {
		// http://code.google.com/p/aima-java/issues/detail?id=66
		Model model = new Model();
		model = model.union(new PropositionSymbol("A"), false)
				.union(new PropositionSymbol("B"), false)
				.union(new PropositionSymbol("C"), true);
		Sentence sentence = parser.parse("((A | B) | C)");
		Set<Clause> clauses = ConvertToConjunctionOfClauses.convert(sentence)
				.getClauses();
		List<PropositionSymbol> symbols = new ArrayList<PropositionSymbol>(
				SymbolCollector.getSymbolsFrom(sentence));

		boolean satisfiable = dpll.dpll(clauses, symbols, model);
		Assert.assertEquals(true, satisfiable);
	}

	[TestMethod]
	public void testDoesNotKnow() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());
		kb.tell("A");

		Assert.assertFalse(dpll.isEntailed(kb, parser.parse("B")));
		Assert.assertFalse(dpll.isEntailed(kb, parser.parse("~B")));
	}
}
