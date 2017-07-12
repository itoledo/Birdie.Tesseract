using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.temporal.generic
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 576. 
     *  
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
     * respectively. 
     *  
     * <b>Note:</b> An implementation of the FORWARD-BACKWARD algorithm using the
     * general purpose probability APIs, i.e. the underlying model implementation is
     * abstracted away.
     * 
     * @author Ciaran O'Reilly
     */
    public class ForwardBackward<T> : ForwardBackwardInference<T>
    {
        private FiniteProbabilityModel<T> transitionModel = null;
        private IDictionary<RandomVariable, RandomVariable> tToTm1StateVarMap = new Dictionary<RandomVariable, RandomVariable>();
        private FiniteProbabilityModel<T> sensorModel = null;

        /**
         * Instantiate an instance of the Forward Backward algorithm.
         * 
         * @param transitionModel
         *            the transition model.
         * @param tToTm1StateVarMap
         *            a map from the X<sub>t<sub> random variables in the transition
         *            model the to X<sub>t-1</sub> random variables.
         * @param sensorModel
         *            the sensor model.
         */
        public ForwardBackward(FiniteProbabilityModel<T> transitionModel,
                IDictionary<RandomVariable, RandomVariable> tToTm1StateVarMap,
                FiniteProbabilityModel<T> sensorModel)
        {
            this.transitionModel = transitionModel;
            foreach (var v in tToTm1StateVarMap)
                this.tToTm1StateVarMap.Add(v);
            this.sensorModel = sensorModel;
        }

        //
        // START-ForwardBackwardInference

        // function FORWARD-BACKWARD(ev, prior) returns a vector of probability
        // distributions 
        public IList<CategoricalDistribution<T>> forwardBackward(IList<IList<AssignmentProposition<T>>> ev, CategoricalDistribution<T> prior)
        {
            // local variables: fv, a vector of forward messages for steps 0,...,t
            IList<CategoricalDistribution<T>> fv = new List<CategoricalDistribution<T>>(ev.Count + 1);
            // b, a representation of the backward message, initially all 1s
            CategoricalDistribution<T> b = initBackwardMessage();
            // sv, a vector of smoothed estimates for steps 1,...,t
            IList<CategoricalDistribution<T>> sv = new List<CategoricalDistribution<T>>(ev.Count);

            // fv[0] <- prior
            fv.Add(prior);
            // for i = 1 to t do
            for (int i = 0; i < ev.Count; i++)
            {
                // fv[i] <- FORWARD(fv[i-1], ev[i])
                fv.Add(forward(fv[i], ev[i]));
            }
            // for i = t downto 1 do
            for (int i = ev.Count - 1; i >= 0; i--)
            {
                // sv[i] <- NORMALIZE(fv[i] * b)
                sv.Insert(0, fv[i + 1].multiplyBy(b).normalize());
                // b <- BACKWARD(b, ev[i])
                b = backward(b, ev[i]);
            }

            // return sv
            return sv;
        }

        public CategoricalDistribution<T> forward(CategoricalDistribution<T> f1_t, IList<AssignmentProposition<T>> e_tp1)
        {
            CategoricalDistribution<T> s1 = new ProbabilityTable<T>(f1_t.getFor().ToArray());
            // Set up required working variables
            Proposition<T>[] props = new Proposition<T>[s1.getFor().Count];
            int i = 0;
            foreach (RandomVariable rv in s1.getFor())
            {
                props[i] = new RandVar<T>(rv.getName(), rv.getDomain());
                i++;
            }
            Proposition<T> Xtp1 = ProbUtil.constructConjunction(props);
            AssignmentProposition<T>[] xt = new AssignmentProposition<T>[tToTm1StateVarMap.Count];
            IDictionary<RandomVariable, AssignmentProposition<T>> xtVarAssignMap = new Dictionary<RandomVariable, AssignmentProposition<T>>();
            i = 0;
            foreach (RandomVariable rv in tToTm1StateVarMap.Keys)
            {
                xt[i] = new AssignmentProposition<T>(tToTm1StateVarMap[rv], default(T));
                xtVarAssignMap.Add(rv, xt[i]);
                i++;
            }

            // Step 1: Calculate the 1 time step prediction
            // &sum;<sub>x<sub>t</sub></sub>
            Iterator<T> if1_t = new Iterator<T>((possibleWorld, probability) =>
            {
                // <b>P</b>(X<sub>t+1</sub> | x<sub>t</sub>)*
                // P(x<sub>t</sub> | e<sub>1:t</sub>)
                foreach (var av in possibleWorld)
                {
                    xtVarAssignMap[av.Key].setValue(av.Value);
                }
                int b = 0;
                foreach (double tp in transitionModel.posteriorDistribution(Xtp1, xt).getValues())
                {
                    s1.setValue(b, s1.getValues()[i] + (tp * probability));
                    b++;
                }
            });
            f1_t.iterateOver(if1_t);

            // Step 2: multiply by the probability of the evidence
            // and normalize
            // <b>P</b>(e<sub>t+1</sub> | X<sub>t+1</sub>)
            CategoricalDistribution<T> s2 = sensorModel.posteriorDistribution(ProbUtil.constructConjunction(e_tp1.ToArray()), Xtp1);

            return s2.multiplyBy(s1).normalize();
        }

        public CategoricalDistribution<T> backward(CategoricalDistribution<T> b_kp2t, IList<AssignmentProposition<T>> e_kp1)
        {
            CategoricalDistribution<T> b_kp1t = new ProbabilityTable<T>(b_kp2t.getFor().ToArray());
            // Set up required working variables
            Proposition<T>[] props = new Proposition<T>[b_kp1t.getFor().Count];
            int i = 0;
            foreach (RandomVariable rv in b_kp1t.getFor())
            {
                RandomVariable prv = tToTm1StateVarMap[rv];
                props[i] = new RandVar<T>(prv.getName(), prv.getDomain());
                i++;
            }
            Proposition<T> Xk = ProbUtil.constructConjunction(props);
            AssignmentProposition<T>[] ax_kp1 = new AssignmentProposition<T>[tToTm1StateVarMap.Count];
            IDictionary<RandomVariable, AssignmentProposition<T>> x_kp1VarAssignMap = new Dictionary<RandomVariable, AssignmentProposition<T>>();
            i = 0;
            foreach (RandomVariable rv in b_kp1t.getFor())
            {
                ax_kp1[i] = new AssignmentProposition<T>(rv, default(T));
                x_kp1VarAssignMap.Add(rv, ax_kp1[i]);
                i++;
            }
            Proposition<T> x_kp1 = ProbUtil.constructConjunction(ax_kp1);
            props = new Proposition<T>[e_kp1.Count];
            Proposition<T> pe_kp1 = ProbUtil.constructConjunction(e_kp1
                    .ToArray());

            // &sum;<sub>x<sub>k+1</sub></sub>
            Iterator<T> ib_kp2t = new Iterator<T>((possibleWorld, probability) =>
            {
                // Assign current values for x<sub>k+1</sub>
                foreach (var av in possibleWorld)
                {
                    x_kp1VarAssignMap[av.Key].setValue(av.Value);
                }

                // P(e<sub>k+1</sub> | x<sub>k+1</sub>)
                // P(e<sub>k+2:t</sub> | x<sub>k+1</sub>)
                double p = sensorModel.posterior(pe_kp1, x_kp1) * probability;

                // <b>P</b>(x<sub>k+1</sub> | X<sub>k</sub>)
                int b = 0;
                foreach (double tp in transitionModel.posteriorDistribution(x_kp1, Xk).getValues())
                {
                    b_kp1t.setValue(b, b_kp1t.getValues()[i] + (tp * p));
                    b++;
                }
            });
            b_kp2t.iterateOver(ib_kp2t);

            return b_kp1t;
        }

        // END-ForwardBackwardInference
        //

        //
        // PRIVATE METHODS
        //
        private CategoricalDistribution<T> initBackwardMessage()
        {
            ProbabilityTable<T> b = new ProbabilityTable<T>(tToTm1StateVarMap.Keys.ToArray());

            for (int i = 0; i < b.size(); i++)
            {
                b.setValue(i, 1.0);
            }

            return b;
        }
    }
}
