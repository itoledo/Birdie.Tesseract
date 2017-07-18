namespace tvn_cosine.ai.test.unit.nlp.parse
{
    public class CYKParseTest
    {

        CYK parser;
        ArrayList<String> words1; ArrayList<String> words2;
        ProbCNFGrammar trivGrammar = ProbCNFGrammarExamples.buildTrivialGrammar();
        // Get Example Grammar 2

        @Before
        public void setUp()
        {
            parser = new CYK();
            words1 = new ArrayList<String>(Arrays.asList("the", "man", "liked", "a", "woman"));

        } // end setUp()

        @Test
        public void testParseReturn()
        {
            float[][][] probTable = null;
            probTable = parser.parse(words1, trivGrammar);
            assertNotNull(probTable);
        }

        @Test
        public void testParse()
        {
            float[][][] probTable;
            probTable = parser.parse(words1, trivGrammar);
            assertTrue(probTable[5][0][4] > 0); // probTable[5,0,4] = [S][Start=0][Length=5] 
        }
    }

}
