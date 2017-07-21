namespace aima.test.unit.logic.firstorder;

using java.util.LinkedHashMap;
using java.util.Map;

using org.antlr.v4.runtime.ANTLRInputStream;
using org.antlr.v4.runtime.CommonTokenStream;
using org.antlr.v4.runtime.TokenStream;
using org.antlr.v4.runtime.tree.ParseTree;
using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

using aima.core.logic.basic.firstorder.SubstVisitor;
using aima.core.logic.basic.firstorder.parsing.ast.Constant;
using aima.core.logic.basic.firstorder.parsing.ast.Sentence;
using aima.core.logic.basic.firstorder.parsing.ast.Term;
using aima.core.logic.basic.firstorder.parsing.ast.Variable;
using aima.extra.logic.firstorder.parser.FirstOrderLogicLexer;
using aima.extra.logic.firstorder.parser.FirstOrderLogicParser;
using aima.extra.logic.firstorder.parser.FirstOrderVisitor;

/**
 * @author Ravi Mohan
 * @author Anurag Rai
 */
public class SubstVisitorTest {

	private SubstVisitor sv;

	@Before
	public void setUp() {
		//parser = new FirstOrderLogicParser(DomainFactory.crusadesDomain());
		sv = new SubstVisitor();
	}

	[TestMethod]
	public void testSubstSingleVariableSucceedsWithPredicate() {
		Sentence beforeSubst = parseToSentence("King(x)");
		Sentence expectedAfterSubst = parseToSentence(" King(John) ");
		Sentence expectedAfterSubstCopy = expectedAfterSubst.copy();

		Assert.assertEquals(expectedAfterSubst, expectedAfterSubstCopy);
		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(beforeSubst, parseToSentence("King(x)"));
	}

	[TestMethod]
	public void testSubstSingleVariableFailsWithPredicate() {
		Sentence beforeSubst = parseToSentence("King(x)");
		Sentence expectedAfterSubst = parseToSentence(" King(x) ");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("y"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(beforeSubst, parseToSentence("King(x)"));
	}

	[TestMethod]
	public void testMultipleVariableSubstitutionWithPredicate() {
		Sentence beforeSubst = parseToSentence("King(x,y)");
		Sentence expectedAfterSubst = parseToSentence(" King(John ,England) ");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));
		p.put(new Variable("y"), new Constant("England"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(beforeSubst, parseToSentence("King(x,y)"));
	}

	[TestMethod]
	public void testMultipleVariablePartiallySucceedsWithPredicate() {
		Sentence beforeSubst = parseToSentence("King(x,y)");
		Sentence expectedAfterSubst = parseToSentence(" King(John ,y) ");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));
		p.put(new Variable("z"), new Constant("England"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(beforeSubst, parseToSentence("King(x,y)"));
	}

	[TestMethod]
	public void testSubstSingleVariableSucceedsWithTermEquality() {
		Sentence beforeSubst = parseToSentence("BrotherOf(x) = EnemyOf(y)");
		Sentence expectedAfterSubst = parseToSentence("BrotherOf(John) = EnemyOf(Saladin)");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));
		p.put(new Variable("y"), new Constant("Saladin"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(beforeSubst, parseToSentence("BrotherOf(x) = EnemyOf(y)"));
	}

	[TestMethod]
	public void testSubstSingleVariableSucceedsWithTermEquality2() {
		Sentence beforeSubst = parseToSentence("BrotherOf(John) = x");
		Sentence expectedAfterSubst = parseToSentence("BrotherOf(John) = Richard");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("Richard"));
		p.put(new Variable("y"), new Constant("Saladin"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("BrotherOf(John) = x"), beforeSubst);
	}

	[TestMethod]
	public void testSubstWithUniversalQuantifierAndSngleVariable() {
		Sentence beforeSubst = parseToSentence("FORALL x King(x)");
		Sentence expectedAfterSubst = parseToSentence("King(John)");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("FORALL x King(x)"), beforeSubst);
	}

	[TestMethod]
	public void testSubstWithUniversalQuantifierAndZeroVariablesMatched() {
		Sentence beforeSubst = parseToSentence("FORALL x King(x)");
		Sentence expectedAfterSubst = parseToSentence("FORALL x King(x)");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("y"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("FORALL x King(x)"), beforeSubst);
	}

	[TestMethod]
	public void testSubstWithUniversalQuantifierAndOneOfTwoVariablesMatched() {
		Sentence beforeSubst = parseToSentence("FORALL x,y King(x,y)");
		Sentence expectedAfterSubst = parseToSentence("FORALL x King(x,John)");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("y"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("FORALL x,y King(x,y)"), beforeSubst);
	}

	[TestMethod]
	public void testSubstWithExistentialQuantifierAndSngleVariable() {
		Sentence beforeSubst = parseToSentence("EXISTS x King(x)");
		Sentence expectedAfterSubst = parseToSentence("King(John)");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);

		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("EXISTS x King(x)"), beforeSubst);
	}

	[TestMethod]
	public void testSubstWithNOTSentenceAndSngleVariable() {
		Sentence beforeSubst = parseToSentence("~ King(x)");
		Sentence expectedAfterSubst = parseToSentence("~ King(John)");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("~ King(x)"), beforeSubst);
	}

	[TestMethod]
	public void testConnectiveANDSentenceAndSngleVariable() {
		Sentence beforeSubst = parseToSentence("EXISTS x ( King(x) & BrotherOf(x) = EnemyOf(y) )");
		Sentence expectedAfterSubst = parseToSentence("( King(John) & BrotherOf(John) = EnemyOf(Saladin) )");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));
		p.put(new Variable("y"), new Constant("Saladin"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("EXISTS x ( King(x) & BrotherOf(x) = EnemyOf(y) )"), beforeSubst);
	}

	[TestMethod]
	public void testParanthisedSingleVariable() {
		Sentence beforeSubst = parseToSentence("((( King(x) )))");
		Sentence expectedAfterSubst = parseToSentence("King(John) ");

		Map<Variable, Term> p = new LinkedHashMap<Variable, Term>();
		p.put(new Variable("x"), new Constant("John"));

		Sentence afterSubst = sv.subst(p, beforeSubst);
		Assert.assertEquals(expectedAfterSubst, afterSubst);
		Assert.assertEquals(parseToSentence("((( King(x))))"), beforeSubst);
	}
	
	private Sentence parseToSentence(string stringToBeParsed) {
		FirstOrderLogicLexer lexer = new FirstOrderLogicLexer(new ANTLRInputStream(stringToBeParsed));
		TokenStream tokens = new CommonTokenStream(lexer);
		FirstOrderLogicParser parser = new FirstOrderLogicParser(tokens);

		ParseTree tree = parser.parse();
		Sentence node = (Sentence) new FirstOrderVisitor().visit(tree,parser);
		return node;
	}
}
