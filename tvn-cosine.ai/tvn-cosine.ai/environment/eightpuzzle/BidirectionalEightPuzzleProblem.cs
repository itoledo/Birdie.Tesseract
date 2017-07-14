using tvn.cosine.ai.agent;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.environment.eightpuzzle
{
    /**
     * @author Ruediger Lunde
     * 
     */
    public class BidirectionalEightPuzzleProblem : GeneralProblem<EightPuzzleBoard, Action>, IBidirectionalProblem<EightPuzzleBoard, Action>
    {
        private readonly IProblem<EightPuzzleBoard, Action> reverseProblem;
         
        public BidirectionalEightPuzzleProblem(EightPuzzleBoard initialState)
            : base(initialState, EightPuzzleFunctions.getActions, EightPuzzleFunctions.getResult, EightPuzzleFunctions.GOAL_STATE.Equals)
        {
            reverseProblem = new GeneralProblem<EightPuzzleBoard, Action>(EightPuzzleFunctions.GOAL_STATE,
                    EightPuzzleFunctions.getActions, EightPuzzleFunctions.getResult,
                    initialState.Equals);
        }

        public IProblem<EightPuzzleBoard, Action> GetOriginalProblem()
        {
            return this;
        }

        public IProblem<EightPuzzleBoard, Action> GetReverseProblem()
        {
            return reverseProblem;
        }
    }
}
