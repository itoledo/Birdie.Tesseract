﻿using tvn.cosine.ai.agent;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.environment.eightpuzzle
{
    /**
     * @author Ruediger Lunde
     * 
     */
    public class BidirectionalEightPuzzleProblem : GeneralProblem<EightPuzzleBoard, IAction>, IBidirectionalProblem<EightPuzzleBoard, IAction>
    {
        private readonly IProblem<EightPuzzleBoard, IAction> reverseProblem;
         
        public BidirectionalEightPuzzleProblem(EightPuzzleBoard initialState)
            : base(initialState, EightPuzzleFunctions.getActions, EightPuzzleFunctions.getResult, EightPuzzleFunctions.GOAL_STATE.Equals)
        {
            reverseProblem = new GeneralProblem<EightPuzzleBoard, IAction>(EightPuzzleFunctions.GOAL_STATE,
                    EightPuzzleFunctions.getActions, EightPuzzleFunctions.getResult,
                    initialState.Equals);
        }

        public IProblem<EightPuzzleBoard, IAction> GetOriginalProblem()
        {
            return this;
        }

        public IProblem<EightPuzzleBoard, IAction> GetReverseProblem()
        {
            return reverseProblem;
        }
    }
}
