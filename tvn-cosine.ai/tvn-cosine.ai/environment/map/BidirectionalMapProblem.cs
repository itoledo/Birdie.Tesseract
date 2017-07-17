using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.environment.map
{
    public class BidirectionalMapProblem : GeneralProblem<string, MoveToAction>, BidirectionalProblem<string, MoveToAction>
    { 
        private Problem<string, MoveToAction> reverseProblem;

        public BidirectionalMapProblem(IMap map, string initialState, string goalState)
        {
            this(map, initialState, goalState, GoalTest.isEqual(goalState));
        }

        public BidirectionalMapProblem(IMap map, string initialState, string goalState, GoalTest<string> goalTest)
            : base(initialState, 
                  MapFunctions.createActionsFunction(map), 
                  MapFunctions.createResultFunction(),
                  goalTest, 
                  MapFunctions.createDistanceStepCostFunction(map))
        {
           

            reverseProblem = new GeneralProblem<string, MoveToAction>(goalState, 
                MapFunctions.createReverseActionsFunction(map),
                initialState.Equals,
                MapFunctions.createDistanceStepCostFunction(map));
        }

        public Problem<string, MoveToAction> getOriginalProblem()
        {
            return this;
        }

        public Problem<string, MoveToAction> getReverseProblem()
        {
            return reverseProblem;
        }
    }
}
