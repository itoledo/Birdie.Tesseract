namespace tvn_cosine.ai.test.unit.nlp.parse
{
    public class ProbContextFreeGrammarTest
    {

        ProbUnrestrictedGrammar g;
        ProbUnrestrictedGrammar cfG;

        @Before
        public void setup()
        {
            g = new ProbUnrestrictedGrammar();
            cfG = ProbContextFreeExamples.buildWumpusGrammar();
        }

        @Test
        public void testValidRule()
        {
            // This rule is a valid Context-Free rule
            Rule validR = new Rule(new ArrayList<String>(Arrays.asList("W")),
                                      new ArrayList<String>(Arrays.asList("a", "s")), (float)0.5);
            // This rule is of correct form but not a context-free rule
            Rule invalidR = new Rule(new ArrayList<String>(Arrays.asList("W", "A")), null, (float)0.5);
            assertFalse(cfG.validRule(invalidR));
            assertTrue(cfG.validRule(validR));
        }

    }

}
