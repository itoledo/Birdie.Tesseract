namespace tvn_cosine.ai.test.unit.logic.fol
{
    public class PredicateCollectorTest
    {
        PredicateCollector collector;

        FOLParser parser;

        @Before
        public void setUp()
        {
            collector = new PredicateCollector();
            parser = new FOLParser(DomainFactory.weaponsDomain());
        }

        @Test
        public void testSimpleSentence()
        {
            Sentence s = parser.parse("(Missile(x) => Weapon(x))");
            List<Predicate> predicates = collector.getPredicates(s);
            Assert.assertNotNull(predicates);
        }
    }

}
