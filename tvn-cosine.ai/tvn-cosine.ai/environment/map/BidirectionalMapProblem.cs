using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.environment.map
{
    public class BidirectionalMapProblem : GeneralProblem<string, MoveToAction>, BidirectionalProblem<string, MoveToAction>
    {
        private Problem<string, MoveToAction> reverseProblem;

        public BidirectionalMapProblem(Map map, string initialState, string goalState)
            : this(map, initialState, goalState, goalState.Equals)
        {

        }

        public BidirectionalMapProblem(Map map, string initialState, string goalState, GoalTest<string> goalTest)
            : base(initialState,
                  MapFunctions.createActionsFunction(map),
                  MapFunctions.createResultFunction(),
                  goalTest,
                  MapFunctions.createDistanceStepCostFunction(map))
        {  
            reverseProblem = new GeneralProblem<string, MoveToAction>(
                    goalState, 
                    MapFunctions.createReverseActionsFunction(map),
                    MapFunctions.createResultFunction(),
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
