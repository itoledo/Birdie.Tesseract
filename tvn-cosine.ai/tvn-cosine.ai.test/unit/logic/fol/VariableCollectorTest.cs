namespace tvn_cosine.ai.test.unit.logic.fol
{
    public class VariableCollectorTest
    {

        FOLParser parser;

        VariableCollector vc;

        @Before
        public void setUp()
        {
            parser = new FOLParser(DomainFactory.crusadesDomain());
            vc = new VariableCollector();
        }

        @Test
        public void testSimplepredicate()
        {
            Set<Variable> variables = vc.collectAllVariables(parser
                    .parse("King(x)"));
            Assert.assertEquals(1, variables.size());
            Assert.assertTrue(variables.contains(new Variable("x")));
        }

        @Test
        public void testMultipleVariables()
        {
            Set<Variable> variables = vc.collectAllVariables(parser
                    .parse("BrotherOf(x) = EnemyOf(y)"));
            Assert.assertEquals(2, variables.size());
            Assert.assertTrue(variables.contains(new Variable("x")));
            Assert.assertTrue(variables.contains(new Variable("y")));
        }

        @Test
        public void testQuantifiedVariables()
        {
            // Note: Should collect quantified variables
            // even if not mentioned in clause.
            Set<Variable> variables = vc.collectAllVariables(parser
                    .parse("FORALL x,y,z (BrotherOf(x) = EnemyOf(y))"));
            Assert.assertEquals(3, variables.size());
            Assert.assertTrue(variables.contains(new Variable("x")));
            Assert.assertTrue(variables.contains(new Variable("y")));
            Assert.assertTrue(variables.contains(new Variable("z")));
        }
    }

}
