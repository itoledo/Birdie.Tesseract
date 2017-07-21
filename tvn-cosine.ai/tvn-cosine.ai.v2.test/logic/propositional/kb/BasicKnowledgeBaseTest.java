namespace aima.test.unit.logic.propositional.kb;

using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

using aima.core.logic.basic.propositional.kb.BasicKnowledgeBase;
using aima.extra.logic.propositional.parser.PLParserWrapper;

/**
 * @author Ravi Mohan
 * @author Anurag Rai
 */
public class BasicKnowledgeBaseTest {
	private BasicKnowledgeBase kb;

	@Before
	public void setUp() {
		kb = new BasicKnowledgeBase(new PLParserWrapper());
	}

	[TestMethod]
	public void testTellInsertsSentence() {
		kb.tell("(A & B)");
		Assert.assertEquals(1, kb.size());
	}

	[TestMethod]
	public void testTellDoesNotInsertSameSentenceTwice() {
		kb.tell("(A & B)");
		Assert.assertEquals(1, kb.size());
		kb.tell("(A & B)");
		Assert.assertEquals(1, kb.size());
	}

	[TestMethod]
	public void testEmptyKnowledgeBaseIsAnEmptyString() {
		Assert.assertEquals("", kb.toString());
	}

	[TestMethod]
	public void testKnowledgeBaseWithOneSentenceToString() {
		kb.tell("(A & B)");
		Assert.assertEquals("A & B", kb.toString());
	}

	[TestMethod]
	public void testKnowledgeBaseWithTwoSentencesToString() {
		kb.tell("(A & B)");
		kb.tell("(C & D)");
		Assert.assertEquals("A & B & C & D", kb.toString());
	}

	[TestMethod]
	public void testKnowledgeBaseWithThreeSentencesToString() {
		kb.tell("(A & B)");
		kb.tell("(C & D)");
		kb.tell("(E & F)");
		Assert.assertEquals(
				"A & B & C & D & E & F",
				kb.toString());
	}
	
	[TestMethod]
	public void testKnowledgeBaseWithNestedSentencesToString() {
		kb.tell("(A & B) & (C & D)");
		kb.tell("(C & D)");
		Assert.assertEquals("A & B & C & D & C & D", kb.toString());
	}
}
