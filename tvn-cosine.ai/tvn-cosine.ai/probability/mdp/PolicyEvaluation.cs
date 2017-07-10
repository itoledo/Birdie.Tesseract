using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 656.<br>
     * <br>
     * Given a policy &pi;<sub>i</sub>, calculate
     * U<sub>i</sub>=U<sup>&pi;<sub>i</sub></sup>, the utility of each state if
     * &pi;<sub>i</sub> were to be executed.
     * 
     * @param <S>
     *            the state type.
     * @param <A>
     *            the action type.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public interface PolicyEvaluation<S, A>
        where A : Action
    {
        /**
         * <b>Policy evaluation:</b> given a policy &pi;<sub>i</sub>, calculate
         * U<sub>i</sub>=U<sup>&pi;<sub>i</sub></sup>, the utility of each state if
         * &pi;<sub>i</sub> were to be executed.
         * 
         * @param pi_i
         *            a policy vector indexed by state
         * @param U
         *            a vector of utilities for states in S
         * @param mdp
         *            an MDP with states S, actions A(s), transition model P(s'|s,a)
         * @return U<sub>i</sub>=U<sup>&pi;<sub>i</sub></sup>, the utility of each
         *         state if &pi;<sub>i</sub> were to be executed.
         */
        IDictionary<S, double> evaluate(IDictionary<S, A> pi_i, IDictionary<S, double> U, MarkovDecisionProcess<S, A> mdp);
    }
}
