﻿namespace tvn_cosine.ai.test.unit.search.informed
{
    public class GreedyBestFirstSearchTest
    {

        @Test
        public void testGreedyBestFirstSearch()
        {
            try
            {
                // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
                // {2,0,5,6,4,8,3,7,1});
                // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
                // {0,8,7,6,5,4,3,2,1});
                EightPuzzleBoard board = new EightPuzzleBoard(new int[] { 7, 1, 8, 0, 4, 6, 2, 3, 5 });

                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(board);
                SearchForActions<EightPuzzleBoard, Action> search = new GreedyBestFirstSearch<>
                        (new GraphSearch<>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<>(problem, search);

                Assert.assertEquals(49, agent.getActions().size()); // GraphSearchReducedFrontier: "49"
                Assert.assertEquals("332", // GraphSearchReducedFrontier: "197"
                        agent.getInstrumentation().getProperty("nodesExpanded"));
                Assert.assertEquals("241", // GraphSearchReducedFrontier: "140"
                        agent.getInstrumentation().getProperty("queueSize"));
                Assert.assertEquals("242", // GraphSearchReducedFrontier: "141"
                        agent.getInstrumentation().getProperty("maxQueueSize"));
            }
            catch (Exception e)
            {
                e.printStackTrace();
                Assert.fail("Exception thrown.");
            }
        }

        @Test
        public void testGreedyBestFirstSearchReducedFrontier()
        {
            try
            {
                // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
                // {2,0,5,6,4,8,3,7,1});
                // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
                // {0,8,7,6,5,4,3,2,1});
                EightPuzzleBoard board = new EightPuzzleBoard(new int[] { 7, 1, 8, 0, 4, 6, 2, 3, 5 });

                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(board);
                QueueBasedSearch<EightPuzzleBoard, Action> search = new GreedyBestFirstSearch<>
                        (new GraphSearchReducedFrontier<>(), EightPuzzleFunctions.createManhattanHeuristicFunction());

                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<>(problem, search);
                Assert.assertEquals(49, agent.getActions().size());
                Assert.assertEquals("197", agent.getInstrumentation().getProperty("nodesExpanded"));
                Assert.assertEquals("140", agent.getInstrumentation().getProperty("queueSize"));
                Assert.assertEquals("141", agent.getInstrumentation().getProperty("maxQueueSize"));
            }
            catch (Exception e)
            {
                e.printStackTrace();
                Assert.fail("Exception thrown.");
            }
        }

        @Test
        public void testAIMA3eFigure3_23() throws Exception
        {
            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
        Problem<String, MoveToAction> problem = new GeneralProblem<>(SimplifiedRoadMapOfPartOfRomania.ARAD,
                MapFunctions.createActionsFunction(romaniaMap), MapFunctions.createResultFunction(),
                GoalTest.isEqual(SimplifiedRoadMapOfPartOfRomania.BUCHAREST),
                MapFunctions.createDistanceStepCostFunction(romaniaMap));

        SearchForActions<String, MoveToAction> search = new GreedyBestFirstSearch<>(new TreeSearch<>(),
                MapFunctions.createSLDHeuristicFunction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST, romaniaMap));
        SearchAgent<String, MoveToAction> agent = new SearchAgent<>(problem, search);
        Assert.assertEquals(
				"[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==Fagaras], Action[name==moveTo, location==Bucharest]]",
				agent.getActions().toString());
		Assert.assertEquals(3, agent.getActions().size());
		Assert.assertEquals("3", agent.getInstrumentation().getProperty("nodesExpanded"));
		Assert.assertEquals("6", agent.getInstrumentation().getProperty("queueSize"));
		Assert.assertEquals("7", agent.getInstrumentation().getProperty("maxQueueSize"));
	}

    @Test
    public void testAIMA3eFigure3_23_using_GraphSearch() throws Exception
    {
        Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
    Problem<String, MoveToAction> problem = new GeneralProblem<>(SimplifiedRoadMapOfPartOfRomania.ARAD,
            MapFunctions.createActionsFunction(romaniaMap), MapFunctions.createResultFunction(),
            GoalTest.isEqual(SimplifiedRoadMapOfPartOfRomania.BUCHAREST),
            MapFunctions.createDistanceStepCostFunction(romaniaMap));

    SearchForActions<String, MoveToAction> search = new GreedyBestFirstSearch<>(new GraphSearch<>(),
            MapFunctions.createSLDHeuristicFunction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST, romaniaMap));
    SearchAgent<String, MoveToAction> agent = new SearchAgent<>(problem, search);
    Assert.assertEquals(
				"[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==Fagaras], Action[name==moveTo, location==Bucharest]]",
				agent.getActions().toString());
		Assert.assertEquals(3, agent.getActions().size());
		Assert.assertEquals("3", agent.getInstrumentation().getProperty("nodesExpanded"));
		Assert.assertEquals("4", agent.getInstrumentation().getProperty("queueSize"));
		Assert.assertEquals("5", agent.getInstrumentation().getProperty("maxQueueSize"));
	}
}

}