﻿using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.problem.api;

namespace tvn.cosine.ai.environment.eightpuzzle
{
    public class BidirectionalEightPuzzleProblem
        : GeneralProblem<EightPuzzleBoard, IAction>, IBidirectionalProblem<EightPuzzleBoard, IAction>
    { 
        private readonly IProblem<EightPuzzleBoard, IAction> reverseProblem;

        public BidirectionalEightPuzzleProblem(EightPuzzleBoard initialState)
                : base(initialState,
                       new EightPuzzleFunctions.ActionFunctionEB(),
                       new EightPuzzleFunctions.ResultFunctionEB(),
                       EightPuzzleFunctions.GOAL_STATE.Equals)
        { 
            reverseProblem = new GeneralProblem<EightPuzzleBoard, IAction>(
                EightPuzzleFunctions.GOAL_STATE,
                new EightPuzzleFunctions.ActionFunctionEB(),
                new EightPuzzleFunctions.ResultFunctionEB(),
                initialState.Equals);
        }

        public IProblem<EightPuzzleBoard, IAction> getOriginalProblem()
        {
            return this;
        }

        public IProblem<EightPuzzleBoard, IAction> getReverseProblem()
        {
            return reverseProblem;
        }
    }
}
