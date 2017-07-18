namespace tvn_cosine.ai.test.unit.logic.propositional.visitors
{
    public class SymbolCollectorTest
    {
        private PLParser parser;

        @Before
       public void setUp()
        {
            parser = new PLParser();
        }

        @Test
       public void testCollectSymbolsFromComplexSentence()
        {
            Sentence sentence = (Sentence)parser.parse("(~B11 | P12 | P21) & (B11 | ~P12) & (B11 | ~P21)");
            Set<PropositionSymbol> s = SymbolCollector.getSymbolsFrom(sentence);
            Assert.assertEquals(3, s.size());
            Sentence b11 = parser.parse("B11");
            Sentence p21 = parser.parse("P21");
            Sentence p12 = parser.parse("P12");
            Assert.assertTrue(s.contains(b11));
            Assert.assertTrue(s.contains(p21));
            Assert.assertTrue(s.contains(p12));
        }
    }
}
