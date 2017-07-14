using System;
using System.Collections.Generic;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.probability.mdp;
using tvn.cosine.ai.probability.mdp.impl;

namespace tvn.cosine.ai.probability.example
{
    /**
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class MDPFactory
    {

        /**
         * Constructs an MDP that can be used to generate the utility values
         * detailed in Fig 17.3.
         * 
         * @param cw
         *            the cell world from figure 17.1.
         * @return an MDP that can be used to generate the utility values detailed
         *         in Fig 17.3.
         */
        public static MarkovDecisionProcess<Cell<double>, CellWorldAction> createMDPForFigure17_3(CellWorld<double> cw)
        {

            return new MDP<Cell<double>, CellWorldAction>(cw.GetCells(),
                    cw.getCellAt(1, 1), createActionsFunctionForFigure17_1(cw),
                    createTransitionProbabilityFunctionForFigure17_1(cw),
                    createRewardFunctionForFigure17_1());
        }

        /**
         * Returns the allowed actions from a specified cell within the cell world
         * described in Fig 17.1.
         * 
         * @param cw
         *            the cell world from figure 17.1.
         * @return the set of actions allowed at a particular cell. This set will be
         *         empty if at a terminal state.
         */
        public static mdp.ActionsFunction<Cell<double>, CellWorldAction> createActionsFunctionForFigure17_1(CellWorld<double> cw)
        {
            ISet<Cell<double>> terminals = new HashSet<Cell<double>>();
            terminals.Add(cw.getCellAt(4, 3));
            terminals.Add(cw.getCellAt(4, 2));

            mdp.ActionsFunction<Cell<double>, CellWorldAction> af = (s) =>
            {
                // All actions can be performed in each cell (except terminal states)
                if (terminals.Contains(s))
                {
                    return new HashSet<CellWorldAction>();
                }
                return CellWorldAction.Actions();
            };
            return af;
        }

        /**
         * Figure 17.1 (b) Illustration of the transition model of the environment:
         * the 'intended' outcome occurs with probability 0.8, but with probability
         * 0.2 the agent moves at right angles to the intended direction. A
         * collision with a wall results in no movement.
         * 
         * @param cw
         *            the cell world from figure 17.1.
         * @return the transition probability function as described in figure 17.1.
         */
        public static mdp.TransitionProbabilityFunction<Cell<double>, CellWorldAction> createTransitionProbabilityFunctionForFigure17_1(CellWorld<double> cw)
        {
            TransitionProbabilityFunction<Cell<double>, CellWorldAction> tf = (sDelta, s, a) =>
            {
                Func<Cell<double>, CellWorldAction, IList<Cell<double>>> possibleOutcomes = (c, ab) =>
                  {
                      // There can be three possible outcomes for the planned action
                      IList<Cell<double>> o = new List<Cell<double>>();

                      o.Add(cw.Result(c, ab));
                      o.Add(cw.Result(c, ab.GetFirstRightAngledAction()));
                      o.Add(cw.Result(c, ab.GetSecondRightAngledAction()));

                      return o;
                  };

                double[] distribution = new double[] { 0.8, 0.1, 0.1 };
                double prob = 0;

                IList<Cell<double>> outcomes = possibleOutcomes(s, a);
                for (int i = 0; i < outcomes.Count; ++i)
                {
                    if (sDelta.Equals(outcomes[i]))
                    {
                        // Note: You have to sum the matches to
                        // sDelta as the different actions
                        // could have the same effect (i.e.
                        // staying in place due to there being
                        // no adjacent cells), which increases
                        // the probability of the transition for
                        // that state.
                        prob += distribution[i];
                    }
                }

                return prob;
            };
            return tf;
        }

        /**
         * 
         * @return the reward function which takes the content of the cell as being
         *         the reward value.
         */
        public static RewardFunction<Cell<double>> createRewardFunctionForFigure17_1()
        {
            RewardFunction<Cell<double>> rf = (s) =>
            {
                return s.getContent();
            };
            return rf;
        }
    }
}
