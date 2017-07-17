using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.eightpuzzle
{
    public class BidirectionalEightPuzzleProblem : GeneralProblem<EightPuzzleBoard, Action>, BidirectionalProblem<EightPuzzleBoard, Action> {

        private readonly Problem<EightPuzzleBoard, Action> reverseProblem;

        public BidirectionalEightPuzzleProblem(EightPuzzleBoard initialState)
            : base(initialState,
                  EightPuzzleFunctions.getActions, 
                  EightPuzzleFunctions.getResult,
                  EightPuzzleFunctions.GOAL_STATE.Equals)
        {
            
            reverseProblem = new GeneralProblem<EightPuzzleBoard, Action>(
                EightPuzzleFunctions.GOAL_STATE,
                EightPuzzleFunctions.getActions, 
                EightPuzzleFunctions.getResult,
                initialStat.Equals));
        }

        public Problem<EightPuzzleBoard, Action> getOriginalProblem()
        {
            return this;
        }

        public Problem<EightPuzzleBoard, Action> getReverseProblem()
        {
            return reverseProblem;
        }
    }
}
