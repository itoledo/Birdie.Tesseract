﻿using tvn.cosine.ai.common.collections; 
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.temporal.generic
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
     * <b>Note:</b> An implementation of the FORWARD-BACKWARD algorithm using the
     * general purpose probability APIs, i.e. the underlying model implementation is
     * abstracted away.
     * 
     * @author Ciaran O'Reilly
     */
    public class ForwardBackward : ForwardBackwardInference
    {
        private FiniteProbabilityModel transitionModel = null;
        private IMap<RandomVariable, RandomVariable> tToTm1StateVarMap = Factory.CreateMap<RandomVariable, RandomVariable>();
        private FiniteProbabilityModel sensorModel = null;

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
        public ForwardBackward(FiniteProbabilityModel transitionModel,
                IMap<RandomVariable, RandomVariable> tToTm1StateVarMap,
                FiniteProbabilityModel sensorModel)
        {
            this.transitionModel = transitionModel;
            this.tToTm1StateVarMap.AddAll(tToTm1StateVarMap);
            this.sensorModel = sensorModel;
        }

        //
        // START-ForwardBackwardInference

        // function FORWARD-BACKWARD(ev, prior) returns a vector of probability
        // distributions

        public IQueue<CategoricalDistribution> forwardBackward(
               IQueue<IQueue<AssignmentProposition>> ev, CategoricalDistribution prior)
        {
            // local variables: fv, a vector of forward messages for steps 0,...,t
            IQueue<CategoricalDistribution> fv = Factory.CreateQueue<CategoricalDistribution>();
            // b, a representation of the backward message, initially all 1s
            CategoricalDistribution b = initBackwardMessage();
            // sv, a vector of smoothed estimates for steps 1,...,t
            IQueue<CategoricalDistribution> sv = Factory.CreateQueue<CategoricalDistribution>();

            // fv[0] <- prior
            fv.Add(prior);
            // for i = 1 to t do
            for (int i = 0; i < ev.Size(); i++)
            {
                // fv[i] <- FORWARD(fv[i-1], ev[i])
                fv.Add(forward(fv.Get(i), ev.Get(i)));
            }
            // for i = t downto 1 do
            for (int i = ev.Size() - 1; i >= 0; i--)
            {
                // sv[i] <- NORMALIZE(fv[i] * b)
                sv.Insert(0, fv.Get(i + 1).multiplyBy(b).normalize());
                // b <- BACKWARD(b, ev[i])
                b = backward(b, ev.Get(i));
            }

            // return sv
            return sv;
        }

        class CategoricalDistributionIteratorImpl : CategoricalDistributionIterator
        {
            private CategoricalDistribution s1;
            private FiniteProbabilityModel transitionModel;
            private AssignmentProposition[] xt;
            private Proposition xtp1;
            private IMap<RandomVariable, AssignmentProposition> xtVarAssignMap;

            public CategoricalDistributionIteratorImpl(FiniteProbabilityModel transitionModel, IMap<RandomVariable, AssignmentProposition> xtVarAssignMap, CategoricalDistribution s1, Proposition xtp1, AssignmentProposition[] xt)
            {
                this.transitionModel = transitionModel;
                this.xtVarAssignMap = xtVarAssignMap;
                this.s1 = s1;
                this.xtp1 = xtp1;
                this.xt = xt;
            }

            public void iterate(IMap<RandomVariable, object> possibleWorld, double probability)
            {
                // <b>P</b>(X<sub>t+1</sub> | x<sub>t</sub>)*
                // P(x<sub>t</sub> | e<sub>1:t</sub>)
                foreach (var av in possibleWorld)
                {
                    xtVarAssignMap.Get(av.GetKey()).setValue(av.GetValue());
                }
                int i = 0;
                foreach (double tp in transitionModel.posteriorDistribution(xtp1, xt).getValues())
                {
                    s1.setValue(i, s1.getValues()[i] + (tp * probability));
                    i++;
                }
            }
        }

        public CategoricalDistribution forward(CategoricalDistribution f1_t, IQueue<AssignmentProposition> e_tp1)
        {
            CategoricalDistribution s1 = new ProbabilityTable(f1_t.getFor());
            // Set up required working variables
            Proposition[] props = new Proposition[s1.getFor().Size()];
            int i = 0;
            foreach (RandomVariable rv in s1.getFor())
            {
                props[i] = new RandVar(rv.getName(), rv.getDomain());
                i++;
            }
            Proposition Xtp1 = ProbUtil.constructConjunction(props);
            AssignmentProposition[] xt = new AssignmentProposition[tToTm1StateVarMap.Size()];
            IMap<RandomVariable, AssignmentProposition> xtVarAssignMap = Factory.CreateMap<RandomVariable, AssignmentProposition>();
            i = 0;
            foreach (RandomVariable rv in tToTm1StateVarMap.GetKeys())
            {
                xt[i] = new AssignmentProposition(tToTm1StateVarMap.Get(rv), "<Dummy Value>");
                xtVarAssignMap.Put(rv, xt[i]);
                i++;
            }

            // Step 1: Calculate the 1 time step prediction
            // &sum;<sub>x<sub>t</sub></sub>
            CategoricalDistributionIterator if1_t = new CategoricalDistributionIteratorImpl(transitionModel,
                xtVarAssignMap, s1, Xtp1, xt);
            f1_t.iterateOver(if1_t);

            // Step 2: multiply by the probability of the evidence
            // and normalize
            // <b>P</b>(e<sub>t+1</sub> | X<sub>t+1</sub>)
            CategoricalDistribution s2 = sensorModel.posteriorDistribution(ProbUtil
                    .constructConjunction(e_tp1.ToArray()), Xtp1);

            return s2.multiplyBy(s1).normalize();
        }

        class CategoricalDistributionIteratorImpl2 : CategoricalDistributionIterator
        {
            private CategoricalDistribution b_kp1t;
            private Proposition pe_kp1;
            private FiniteProbabilityModel sensorModel;
            private FiniteProbabilityModel transitionModel;
            private Proposition xk;
            private Proposition x_kp1;
            private IMap<RandomVariable, AssignmentProposition> x_kp1VarAssignMap;

            public CategoricalDistributionIteratorImpl2(IMap<RandomVariable, AssignmentProposition> x_kp1VarAssignMap, FiniteProbabilityModel sensorModel, FiniteProbabilityModel transitionModel, CategoricalDistribution b_kp1t, Proposition pe_kp1, Proposition xk, Proposition x_kp1)
            {
                this.x_kp1VarAssignMap = x_kp1VarAssignMap;
                this.sensorModel = sensorModel;
                this.transitionModel = transitionModel;
                this.b_kp1t = b_kp1t;
                this.pe_kp1 = pe_kp1;
                this.xk = xk;
                this.x_kp1 = x_kp1;
            }

            public void iterate(IMap<RandomVariable, object> possibleWorld, double probability)
            {
                // Assign current values for x<sub>k+1</sub>
                foreach (var av in possibleWorld)
                {
                    x_kp1VarAssignMap.Get(av.GetKey()).setValue(av.GetValue());
                }

                // P(e<sub>k+1</sub> | x<sub>k+1</sub>)
                // P(e<sub>k+2:t</sub> | x<sub>k+1</sub>)
                double p = sensorModel.posterior(pe_kp1, x_kp1) * probability;

                // <b>P</b>(x<sub>k+1</sub> | X<sub>k</sub>)
                int i = 0;
                foreach (double tp in transitionModel.posteriorDistribution(x_kp1, xk).getValues())
                {
                    b_kp1t.setValue(i, b_kp1t.getValues()[i] + (tp * p));
                    i++;
                }
            }
        }

        public CategoricalDistribution backward(CategoricalDistribution b_kp2t, IQueue<AssignmentProposition> e_kp1)
        {
            CategoricalDistribution b_kp1t = new ProbabilityTable(b_kp2t.getFor());
            // Set up required working variables
            Proposition[] props = new Proposition[b_kp1t.getFor().Size()];
            int i = 0;
            foreach (RandomVariable rv in b_kp1t.getFor())
            {
                RandomVariable prv = tToTm1StateVarMap.Get(rv);
                props[i] = new RandVar(prv.getName(), prv.getDomain());
                i++;
            }
            Proposition Xk = ProbUtil.constructConjunction(props);
            AssignmentProposition[] ax_kp1 = new AssignmentProposition[tToTm1StateVarMap.Size()];
            IMap<RandomVariable, AssignmentProposition> x_kp1VarAssignMap = Factory.CreateMap<RandomVariable, AssignmentProposition>();
            i = 0;
            foreach (RandomVariable rv in b_kp1t.getFor())
            {
                ax_kp1[i] = new AssignmentProposition(rv, "<Dummy Value>");
                x_kp1VarAssignMap.Put(rv, ax_kp1[i]);
                i++;
            }
            Proposition x_kp1 = ProbUtil.constructConjunction(ax_kp1);
            props = e_kp1.ToArray();
            Proposition pe_kp1 = ProbUtil.constructConjunction(props);

            // &sum;<sub>x<sub>k+1</sub></sub>
            CategoricalDistributionIterator ib_kp2t = new CategoricalDistributionIteratorImpl2(x_kp1VarAssignMap, 
                sensorModel, transitionModel, b_kp1t, pe_kp1, Xk, x_kp1);
            b_kp2t.iterateOver(ib_kp2t);

            return b_kp1t;
        }

        // END-ForwardBackwardInference
        //

        //
        // PRIVATE METHODS
        //
        private CategoricalDistribution initBackwardMessage()
        {
            ProbabilityTable b = new ProbabilityTable(tToTm1StateVarMap.GetKeys());

            for (int i = 0; i < b.size(); i++)
            {
                b.setValue(i, 1.0);
            }

            return b;
        }
    }
}
