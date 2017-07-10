using System.Collections.Generic;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.temporal;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.probability.hmm.exact
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 576.<br>
     * <br>
     * 
     * <pre>
     * function FORWARD-BACKWARD(ev, prior) returns a vector of probability distributions
     *   inputs: ev, a vector of evidence values for steps 1,...,t
     *           prior, the prior distribution on the initial state, <b>P</b>(X<sub>0</sub>)
     *   local variables: fv, a vector of forward messages for steps 0,...,t
     *                    b, a representation of the backward message, initially all 1s
     *                    sv, a vector of smoothed estimates for steps 1,...,t
     *                    
     *   fv[0] <- prior
     *   for i = 1 to t do
     *       fv[i] <- FORWARD(fv[i-1], ev[i])
     *   for i = t downto 1 do
     *       sv[i] <- NORMALIZE(fv[i] * b)
     *       b <- BACKWARD(b, ev[i])
     *   return sv
     * </pre>
     * 
     * Figure 15.4 The forward-backward algorithm for smoothing: computing posterior
     * probabilities of a sequence of states given a sequence of observations. The
     * FORWARD and BACKWARD operators are defined by Equations (15.5) and (15.9),
     * respectively.<br>
     * <br>
     * <b>Note:</b> An implementation of the FORWARD-BACKWARD algorithm using a
     * Hidden Markov Model as the underlying model implementation.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class HMMForwardBackward<T> : ForwardBackwardInference<T>
    {
        protected HiddenMarkovModel<T> hmm = null;

        public HMMForwardBackward(HiddenMarkovModel<T> hmm)
        {
            this.hmm = hmm;
        }

        //
        // START-ForwardBackwardInference 
        public virtual IList<CategoricalDistribution<T>> forwardBackward(IList<IList<AssignmentProposition<T>>> ev, CategoricalDistribution<T> prior)
        {
            // local variables: fv, a vector of forward messages for steps 0,...,t
            IList<Matrix> fv = new List<Matrix>(ev.Count + 1);
            // b, a representation of the backward message, initially all 1s
            Matrix b = hmm.createUnitMessage();
            // sv, a vector of smoothed estimates for steps 1,...,t
            IList<Matrix> sv = new List<Matrix>(ev.Count);

            // fv[0] <- prior
            fv.Add(hmm.convert(prior));
            // for i = 1 to t do
            for (int i = 0; i < ev.Count; i++)
            {
                // fv[i] <- FORWARD(fv[i-1], ev[i])
                fv.Add(forward(fv[i], hmm.getEvidence(ev[i])));
            }
            // for i = t downto 1 do
            for (int i = ev.Count - 1; i >= 0; i--)
            {
                // sv[i] <- NORMALIZE(fv[i] * b)
                sv.Insert(0, hmm.normalize(fv[i + 1].arrayTimes(b)));
                // b <- BACKWARD(b, ev[i])
                b = backward(b, hmm.getEvidence(ev[i]));
            }

            // return sv
            return hmm.convert(sv);
        }

        public virtual CategoricalDistribution<T> forward(CategoricalDistribution<T> f1_t, IList<AssignmentProposition<T>> e_tp1)
        {
            return hmm.convert(forward(hmm.convert(f1_t), hmm.getEvidence(e_tp1)));
        }

        public virtual CategoricalDistribution<T> backward(CategoricalDistribution<T> b_kp2t, IList<AssignmentProposition<T>> e_kp1)
        {
            return hmm.convert(backward(hmm.convert(b_kp2t), hmm.getEvidence(e_kp1)));
        }

        // END-ForwardBackwardInference
        //

        /**
         * The forward equation (15.5) in Matrix form becomes (15.12):<br>
         * 
         * <pre>
         * <b>f</b><sub>1:t+1</sub> = &alpha;<b>O</b><sub>t+1</sub><b>T</b><sup>T</sup><b>f</b><sub>1:t</sub>
         * </pre>
         * 
         * @param f1_t
         *            <b>f</b><sub>1:t</sub>
         * @param O_tp1
         *            <b>O</b><sub>t+1</sub>
         * @return <b>f</b><sub>1:t+1</sub>
         */
        public Matrix forward(Matrix f1_t, Matrix O_tp1)
        {
            return hmm.normalize(O_tp1.times(hmm.getTransitionModel().transpose() .times(f1_t)));
        }

        /**
         * The backward equation (15.9) in Matrix form becomes (15.13):<br>
         * 
         * <pre>
         * <b>b</b><sub>k+1:t</sub> = <b>T</b><b>O</b><sub>k+1</sub><b>b</b><sub>k+2:t</sub>
         * </pre>
         * 
         * @param b_kp2t
         *            <b>b</b><sub>k+2:t</sub>
         * @param O_kp1
         *            <b>O</b><sub>k+1</sub>
         * @return <b>b</b><sub>k+1:t</sub>
         */
        public Matrix backward(Matrix b_kp2t, Matrix O_kp1)
        {
            return hmm.getTransitionModel().times(O_kp1).times(b_kp2t);
        }
    } 
}
