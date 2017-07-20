using tvn.cosine.ai.agent;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.environment.eightpuzzle
{
    public class BidirectionalEightPuzzleProblem : 
        GeneralProblem<EightPuzzleBoard, IAction>, BidirectionalProblem<EightPuzzleBoard, IAction> {

        private readonly Problem<EightPuzzleBoard, IAction> reverseProblem;

        public BidirectionalEightPuzzleProblem(EightPuzzleBoard initialState)
            : base(initialState,
                  EightPuzzleFunctions.getActions, 
                  EightPuzzleFunctions.getResult,
                  EightPuzzleFunctions.GOAL_STATE.Equals)
        {
            
            reverseProblem = new GeneralProblem<EightPuzzleBoard, IAction>(
                EightPuzzleFunctions.GOAL_STATE,
                EightPuzzleFunctions.getActions, 
                EightPuzzleFunctions.getResult,
                initialState.Equals);
        }

        public Problem<EightPuzzleBoard, IAction> getOriginalProblem()
        {
            return this;
        }

        public Problem<EightPuzzleBoard, IAction> getReverseProblem()
        {
            return reverseProblem;
        }
    }
}
