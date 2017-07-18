namespace tvn_cosine.ai.test.unit.search.csp
{
    public class MapCSPTest
    {
        private CSP<Variable, String> csp;

        @Before
        public void setUp()
        {
            csp = new MapCSP();
        }

        @Test
        public void testBackTrackingSearch()
        {
            Optional<Assignment<Variable, String>> results = new FlexibleBacktrackingSolver<Variable, String>().solve(csp);
            Assert.assertTrue(results.isPresent());
            Assert.assertEquals(MapCSP.GREEN, results.get().getValue(MapCSP.WA));
            Assert.assertEquals(MapCSP.RED, results.get().getValue(MapCSP.NT));
            Assert.assertEquals(MapCSP.BLUE, results.get().getValue(MapCSP.SA));
            Assert.assertEquals(MapCSP.GREEN, results.get().getValue(MapCSP.Q));
            Assert.assertEquals(MapCSP.RED, results.get().getValue(MapCSP.NSW));
            Assert.assertEquals(MapCSP.GREEN, results.get().getValue(MapCSP.V));
            Assert.assertEquals(MapCSP.RED, results.get().getValue(MapCSP.T));
        }

        @Test
        public void testMCSearch()
        {
            new MinConflictsSolver<Variable, String>(100).solve(csp);
        }
    }

}
