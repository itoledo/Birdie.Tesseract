namespace tvn_cosine.ai.test.unit.logic.propositional.inference
{
    public class PLFCEntailsTest
    {
        private PLParser parser;
        private PLFCEntails plfce;

        @Before
        public void setUp()
        {
            parser = new PLParser();
            plfce = new PLFCEntails();
        }

        @Test
        public void testAIMAExample()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("A & P => L");
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");
            PropositionSymbol q = (PropositionSymbol)parser.parse("Q");

            Assert.assertEquals(true, plfce.plfcEntails(kb, q));
        }


    @Test(expected= IllegalArgumentException.class)
	public void testKBWithNonDefiniteClauses()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("~A & P => L"); // Not a definite clause
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");
            PropositionSymbol q = (PropositionSymbol)parser.parse("Q");

            Assert.assertEquals(true, plfce.plfcEntails(kb, q));
        }
    }
}
