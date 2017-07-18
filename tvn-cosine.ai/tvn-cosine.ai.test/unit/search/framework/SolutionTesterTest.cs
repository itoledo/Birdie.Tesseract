namespace tvn_cosine.ai.test.unit.search.framework
{
    public class SolutionTesterTest
    {

        @Test
        public void testMultiGoalProblem() throws Exception
        {
            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();

        Problem<String, MoveToAction> problem = new GeneralProblem<String, MoveToAction>
                (SimplifiedRoadMapOfPartOfRomania.ARAD,
                MapFunctions.createActionsFunction(romaniaMap), MapFunctions.createResultFunction(),
                GoalTest.< String > isEqual(SimplifiedRoadMapOfPartOfRomania.BUCHAREST).or
                        (GoalTest.isEqual(SimplifiedRoadMapOfPartOfRomania.HIRSOVA)),
                MapFunctions.createDistanceStepCostFunction(romaniaMap)) {
            @Override

            public boolean testSolution(Node<String, MoveToAction> node)
        {
            return testGoal(node.getState()) && node.getPathCost() > 550;
            // accept paths to goal only if their costs are above 550
        }
    };

    SearchForActions<String, MoveToAction> search = new UniformCostSearch<>(new GraphSearch<>());

    SearchAgent<String, MoveToAction> agent = new SearchAgent<>(problem, search);
    Assert.assertEquals(
				"[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==RimnicuVilcea], Action[name==moveTo, location==Pitesti], Action[name==moveTo, location==Bucharest], Action[name==moveTo, location==Urziceni], Action[name==moveTo, location==Hirsova]]",
				agent.getActions().toString());
		Assert.assertEquals(6, agent.getActions().size());
		Assert.assertEquals("15", agent.getInstrumentation().getProperty("nodesExpanded"));
		Assert.assertEquals("1", agent.getInstrumentation().getProperty("queueSize"));
		Assert.assertEquals("5", agent.getInstrumentation().getProperty("maxQueueSize"));
	}
}

}
