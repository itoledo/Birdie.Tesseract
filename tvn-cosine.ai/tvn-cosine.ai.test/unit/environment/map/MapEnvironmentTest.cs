namespace tvn_cosine.ai.test.unit.environment.map
{
    public class MapEnvironmentTest
    {
        MapEnvironment me;

        SimpleMapAgent ma;

        @Before
        public void setUp()
        {
            ExtendableMap aMap = new ExtendableMap();
            aMap.addBidirectionalLink("A", "B", 5.0);
            aMap.addBidirectionalLink("A", "C", 6.0);
            aMap.addBidirectionalLink("B", "C", 4.0);
            aMap.addBidirectionalLink("C", "D", 7.0);
            aMap.addUnidirectionalLink("B", "E", 14.0);

            me = new MapEnvironment(aMap);
            ma = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<>(),
                    new String[] { "A" });
        }

        @Test
        public void testAddAgent()
        {
            me.addAgent(ma, "E");
            Assert.assertEquals(me.getAgentLocation(ma), "E");
        }

        @Test
        public void testExecuteAction()
        {
            me.addAgent(ma, "D");
            me.executeAction(ma, new MoveToAction("C"));
            Assert.assertEquals(me.getAgentLocation(ma), "C");
        }

        @Test
        public void testPerceptSeenBy()
        {
            me.addAgent(ma, "D");
            DynamicPercept p = (DynamicPercept)me.getPerceptSeenBy(ma);
            Assert.assertEquals(p.getAttribute(DynAttributeNames.PERCEPT_IN), "D");
        }

        @Test
        public void testTwoAgentsSupported()
        {
            SimpleMapAgent ma1 = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<>(),
                    new String[] { "A" });
            SimpleMapAgent ma2 = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<>(),
                    new String[] { "A" });

            me.addAgent(ma1, "A");
            me.addAgent(ma2, "A");
            me.executeAction(ma1, new MoveToAction("B"));
            me.executeAction(ma2, new MoveToAction("C"));

            Assert.assertEquals(me.getAgentLocation(ma1), "B");
            Assert.assertEquals(me.getAgentLocation(ma2), "C");
        }
    }

}
