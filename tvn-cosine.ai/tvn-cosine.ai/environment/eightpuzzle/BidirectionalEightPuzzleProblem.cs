using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static bool isEqual(EightPuzzleBoard state)
        {
            return null == state ? false : true;
        }

        public BidirectionalEightPuzzleProblem(EightPuzzleBoard initialState)
            : base(initialState, EightPuzzleFunctions.getActions, EightPuzzleFunctions.getResult, EightPuzzleFunctions.GOAL_STATE.Equals)
        {
            reverseProblem = new GeneralProblem<EightPuzzleBoard, Action>(EightPuzzleFunctions.GOAL_STATE,
                    EightPuzzleFunctions.getActions, EightPuzzleFunctions.getResult,
                    initialState.Equals);
        }

        public IProblem<EightPuzzleBoard, Action> getOriginalProblem()
        {
            return this;
        }

        public IProblem<EightPuzzleBoard, Action> getReverseProblem()
        {
            return reverseProblem;
        }
    }
}
