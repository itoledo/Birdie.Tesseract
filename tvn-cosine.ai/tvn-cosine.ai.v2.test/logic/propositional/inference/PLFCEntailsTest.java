namespace aima.test.unit.logic.propositional.inference;

using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

using aima.core.logic.basic.propositional.inference.PLFCEntails;
using aima.core.logic.basic.propositional.kb.BasicKnowledgeBase;
using aima.extra.logic.propositional.parser.PLParserWrapper;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 * @author Anurag Rai
 * 
 */
public class PLFCEntailsTest {
	private PLFCEntails plfce;

	@Before
	public void setUp() {
		plfce = new PLFCEntails();
	}
	
	[TestMethod]
	public void testModusPonens() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());
		kb.tell("A => B");
		kb.tell("A");
		
		Assert.assertEquals(true, plfce.plfcEntails(kb, "B", new PLParserWrapper()));
	}
	
	[TestMethod]
	public void testPLFCEntails1() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());
		kb.tell("A & B => C");
		kb.tell("C & D => E");
		kb.tell("C & F => G");
		kb.tell("A");
		kb.tell("B");
		kb.tell("D");
		
		Assert.assertEquals(true, plfce.plfcEntails(kb, "E", new PLParserWrapper()));
	}
	
	[TestMethod]
	public void testAIMAExample() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());
		kb.tell("P => Q");
		kb.tell("L & M => P");
		kb.tell("B & L => M");
		kb.tell("A & P => L");
		kb.tell("A & B => L");
		kb.tell("A");
		kb.tell("B");
		
		Assert.assertEquals(true, plfce.plfcEntails(kb, "Q", new PLParserWrapper()));
	}
	
	[TestMethod](expected=IllegalArgumentException))]
	public void testKBWithNonDefiniteClauses() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());
		kb.tell("P => Q");
		kb.tell("L & M => P");
		kb.tell("B & L => M");
		kb.tell("~A & P => L"); // Not a definite clause
		kb.tell("A & B => L");
		kb.tell("A");
		kb.tell("B");
		
		Assert.assertEquals(true, plfce.plfcEntails(kb, "Q", new PLParserWrapper()));
	}
	
	//TODO: Make Exception more specific
	[TestMethod](expected=Exception.class)
	public void testParserException() {
		BasicKnowledgeBase kb = new BasicKnowledgeBase(new PLParserWrapper());
		kb.tell("A => B");
		kb.tell("A");
		
		Assert.assertEquals(true, plfce.plfcEntails(kb, "b b", new PLParserWrapper()));
	}
}