namespace tvn_cosine.ai.test.unit.search.online
{
    public class LRTAStarAgentTest
    {
        private ExtendableMap aMap;
        private StringBuffer envChanges;
        private ToDoubleFunction<String> h;

        @Before
       public void setUp()
        {
            aMap = new ExtendableMap();
            aMap.addBidirectionalLink("A", "B", 4.0);
            aMap.addBidirectionalLink("B", "C", 4.0);
            aMap.addBidirectionalLink("C", "D", 4.0);
            aMap.addBidirectionalLink("D", "E", 4.0);
            aMap.addBidirectionalLink("E", "F", 4.0);
            h = (state)-> 1.0;

            envChanges = new StringBuffer();
        }

        @Test
       public void testAlreadyAtGoal()
        {
            MapEnvironment me = new MapEnvironment(aMap);
            OnlineSearchProblem<String, MoveToAction> problem = new GeneralProblem<>(null,
                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("A"),
                    MapFunctions.createDistanceStepCostFunction(aMap));
            LRTAStarAgent<String, MoveToAction> agent = new LRTAStarAgent<>
                    (problem, MapFunctions.createPerceptToStateFunction(), h);

            me.addAgent(agent, "A");
            me.addEnvironmentView(new TestEnvironmentView());
            me.stepUntilDone();

            Assert.assertEquals("Action[name==NoOp]->", envChanges.toString());
        }

        @Test
       public void testNormalSearch()
        {
            MapEnvironment me = new MapEnvironment(aMap);
            OnlineSearchProblem<String, MoveToAction> problem = new GeneralProblem<>(null,
                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("F"),
                    MapFunctions.createDistanceStepCostFunction(aMap));
            LRTAStarAgent<String, MoveToAction> agent = new LRTAStarAgent<>
                    (problem, MapFunctions.createPerceptToStateFunction(), h);

            me.addAgent(agent, "A");
            me.addEnvironmentView(new TestEnvironmentView());
            me.stepUntilDone();

            Assert.assertEquals(
                    "Action[name==moveTo, location==B]->Action[name==moveTo, location==A]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==F]->Action[name==NoOp]->",
                    envChanges.toString());
        }

        @Test
       public void testNoPath()
        {
            MapEnvironment me = new MapEnvironment(aMap);
            OnlineSearchProblem<String, MoveToAction> problem = new GeneralProblem<>(null,
                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("G"),
                    MapFunctions.createDistanceStepCostFunction(aMap));
            LRTAStarAgent<String, MoveToAction> agent = new LRTAStarAgent<>
                    (problem, MapFunctions.createPerceptToStateFunction(), h);

            me.addAgent(agent, "A");
            me.addEnvironmentView(new TestEnvironmentView());
            // Note: Will search forever if no path is possible,
            // Therefore restrict the number of steps to something
            // reasonablbe, against which to test.
            me.step(14);

            Assert.assertEquals(
                    "Action[name==moveTo, location==B]->Action[name==moveTo, location==A]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==F]->Action[name==moveTo, location==E]->",
                    envChanges.toString());
        }

        private class TestEnvironmentView implements EnvironmentView
        {

        public void notify(String msg)
        {
            envChanges.append(msg).append("->");
        }

        public void agentAdded(Agent agent, Environment source)
        {
            // Nothing.
        }

        public void agentActed(Agent agent, Percept percept, Action action, Environment source)
        {
            envChanges.append(action).append("->");
        }
    }
}

}
