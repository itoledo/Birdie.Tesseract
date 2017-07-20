using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.bayes.exact;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.hmm.exact;
using tvn.cosine.ai.probability.mdp;
using tvn.cosine.ai.probability.mdp.impl;
using tvn.cosine.ai.probability.mdp.search;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.temporal.generic;
using tvn.cosine.ai.probability.util;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.probability
{
    public class ProbabilityDemo
    {
        // Note: You should increase this to 1000000+
        // in order to get answers from the approximate
        // algorithms (i.e. Rejection, Likelihood and Gibbs)
        // that look close to their exact inference
        // counterparts.
        public static readonly int NUM_SAMPLES = 1000;

        public static void Main(params string[] args)
        {
            // Chapter 13
            fullJointDistributionModelDemo();

            // Chapter 14 - Exact
            bayesEnumerationAskDemo();
            bayesEliminationAskDemo();
            // Chapter 14 - Approx
            bayesRejectionSamplingDemo();
            bayesLikelihoodWeightingDemo();
            bayesGibbsAskDemo();

            // Chapter 15
            forwardBackWardDemo();
            fixedLagSmoothingDemo();
            particleFilterinfDemo();

            // Chapter 17
            valueIterationDemo();
            policyIterationDemo();
        }

        public static void fullJointDistributionModelDemo()
        {
            System.Console.WriteLine("DEMO: Full Joint Distribution Model");
            System.Console.WriteLine("===================================");
            demoToothacheCavityCatchModel(new FullJointDistributionToothacheCavityCatchModel());
            demoBurglaryAlarmModel(new FullJointDistributionBurglaryAlarmModel());
            System.Console.WriteLine("===================================");
        }

        public static void bayesEnumerationAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Enumeration Ask");
            System.Console.WriteLine("===========================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new EnumerationAsk()));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new EnumerationAsk()));
            System.Console.WriteLine("===========================");
        }

        public static void bayesEliminationAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Elimination Ask");
            System.Console.WriteLine("===========================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new EliminationAsk()));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new EliminationAsk()));
            System.Console.WriteLine("===========================");
        }

        public static void bayesRejectionSamplingDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Rejection Sampling N = " + NUM_SAMPLES);
            System.Console.WriteLine("==============================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new BayesInferenceApproxAdapter(new RejectionSampling(),
                            NUM_SAMPLES)));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new BayesInferenceApproxAdapter(new RejectionSampling(),
                            NUM_SAMPLES)));
            System.Console.WriteLine("==============================");
        }

        public static void bayesLikelihoodWeightingDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Likelihood Weighting N = "
                    + NUM_SAMPLES);
            System.Console.WriteLine("================================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new BayesInferenceApproxAdapter(new LikelihoodWeighting(),
                            NUM_SAMPLES)));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new BayesInferenceApproxAdapter(new LikelihoodWeighting(),
                            NUM_SAMPLES)));
            System.Console.WriteLine("================================");
        }

        public static void bayesGibbsAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Gibbs Ask N = " + NUM_SAMPLES);
            System.Console.WriteLine("=====================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new BayesInferenceApproxAdapter(new GibbsAsk(), NUM_SAMPLES)));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new BayesInferenceApproxAdapter(new GibbsAsk(), NUM_SAMPLES)));
            System.Console.WriteLine("=====================");
        }

        public static void forwardBackWardDemo()
        {

            System.Console.WriteLine("DEMO: Forward-BackWard");
            System.Console.WriteLine("======================");

            System.Console.WriteLine("Umbrella World");
            System.Console.WriteLine("--------------");
            ForwardBackward uw = new ForwardBackward(
                    GenericTemporalModelFactory.getUmbrellaWorldTransitionModel(),
                    GenericTemporalModelFactory.getUmbrellaWorld_Xt_to_Xtm1_Map(),
                    GenericTemporalModelFactory.getUmbrellaWorldSensorModel());

            CategoricalDistribution prior = new ProbabilityTable(new double[] {
                0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1
            IQueue<IQueue<AssignmentProposition>> evidence = Factory.CreateQueue<IQueue<AssignmentProposition>>();
            IQueue<AssignmentProposition> e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e1);

            IQueue<CategoricalDistribution> smoothed = uw.forwardBackward(evidence,
                    prior);

            System.Console.WriteLine("Day 1 (Umbrealla_t=true) smoothed:\nday 1 = "
                    + smoothed.Get(0));

            // Day 2
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e2);

            smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 2 (Umbrealla_t=true) smoothed:\nday 1 = "
                    + smoothed.Get(0) + "\nday 2 = " + smoothed.Get(1));

            // Day 3
            IQueue<AssignmentProposition> e3 = Factory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV,
                    false));
            evidence.Add(e3);

            smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 3 (Umbrealla_t=false) smoothed:\nday 1 = "
                    + smoothed.Get(0) + "\nday 2 = " + smoothed.Get(1)
                    + "\nday 3 = " + smoothed.Get(2));

            System.Console.WriteLine("======================");
        }

        public static void fixedLagSmoothingDemo()
        {
            System.Console.WriteLine("DEMO: Fixed-Lag-Smoothing");
            System.Console.WriteLine("=========================");
            System.Console.WriteLine("Lag = 1");
            System.Console.WriteLine("-------");
            FixedLagSmoothing uw = new FixedLagSmoothing(
                    HMMExampleFactory.getUmbrellaWorldModel(), 1);

            // Day 1 - Lag 1
            IQueue<AssignmentProposition> e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            CategoricalDistribution smoothed = uw.fixedLagSmoothing(e1);

            System.Console.WriteLine("Day 1 (Umbrella_t=true) smoothed:\nday 1="
                    + smoothed);

            // Day 2 - Lag 1
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e2);

            System.Console.WriteLine("Day 2 (Umbrella_t=true) smoothed:\nday 1="
                    + smoothed);

            // Day 3 - Lag 1
            IQueue<AssignmentProposition> e3 = Factory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV,
                    false));

            smoothed = uw.fixedLagSmoothing(e3);

            System.Console.WriteLine("Day 3 (Umbrella_t=false) smoothed:\nday 2="
                    + smoothed);

            System.Console.WriteLine("-------");
            System.Console.WriteLine("Lag = 2");
            System.Console.WriteLine("-------");

            uw = new FixedLagSmoothing(HMMExampleFactory.getUmbrellaWorldModel(), 2);

            // Day 1 - Lag 2
            e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e1);

            System.Console.WriteLine("Day 1 (Umbrella_t=true) smoothed:\nday 1="
                    + smoothed);

            // Day 2 - Lag 2
            e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e2);

            System.Console.WriteLine("Day 2 (Umbrella_t=true) smoothed:\nday 1="
                    + smoothed);

            // Day 3 - Lag 2
            e3 = Factory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV,
                    false));

            smoothed = uw.fixedLagSmoothing(e3);

            System.Console.WriteLine("Day 3 (Umbrella_t=false) smoothed:\nday 1="
                    + smoothed);

            System.Console.WriteLine("=========================");
        }

        public static void particleFilterinfDemo()
        {
            System.Console.WriteLine("DEMO: Particle-Filtering");
            System.Console.WriteLine("========================");
            System.Console.WriteLine("Figure 15.18");
            System.Console.WriteLine("------------");

            MockRandomizer mr = new MockRandomizer(new double[] {
				// Prior Sample:
				// 8 with Rain_t-1=true from prior distribution
				0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5,
				// 2 with Rain_t-1=false from prior distribution
				0.6, 0.6,
				// (a) Propagate 6 samples Rain_t=true
				0.7, 0.7, 0.7, 0.7, 0.7, 0.7,
				// 4 samples Rain_t=false
				0.71, 0.71, 0.31, 0.31,
				// (b) Weight should be for first 6 samples:
				// Rain_t-1=true, Rain_t=true, Umbrella_t=false = 0.1
				// Next 2 samples:
				// Rain_t-1=true, Rain_t=false, Umbrealla_t=false= 0.8
				// Final 2 samples:
				// Rain_t-1=false, Rain_t=false, Umbrella_t=false = 0.8
				// gives W[] =
				// [0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.8, 0.8, 0.8, 0.8]
				// normalized =
				// [0.026, ...., 0.211, ....] is approx. 0.156 = true
				// the remainder is false
				// (c) Resample 2 Rain_t=true, 8 Rain_t=false
				0.15, 0.15, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2,
				//
				// Next Sample:
				// (a) Propagate 1 samples Rain_t=true
				0.7,
				// 9 samples Rain_t=false
				0.71, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31,
				// (c) resample 1 Rain_t=true, 9 Rain_t=false
				0.0001, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2 });

            int N = 10;
            ParticleFiltering pf = new ParticleFiltering(N,
                    DynamicBayesNetExampleFactory.getUmbrellaWorldNetwork(), mr);

            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(
                ExampleRV.UMBREALLA_t_RV, false) };

            System.Console.WriteLine("First Sample Set:");
            AssignmentProposition[][] S = pf.particleFiltering(e);
            for (int i = 0; i < N;++i)
            {
                System.Console.WriteLine("Sample " + (i + 1) + " = " + S[i][0]);
            }
            System.Console.WriteLine("Second Sample Set:");
            S = pf.particleFiltering(e);
            for (int i = 0; i < N;++i)
            {
                System.Console.WriteLine("Sample " + (i + 1) + " = " + S[i][0]);
            }

            System.Console.WriteLine("========================");
        }

        public static void valueIterationDemo()
        {

            System.Console.WriteLine("DEMO: Value Iteration");
            System.Console.WriteLine("=====================");
            System.Console.WriteLine("Figure 17.3");
            System.Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
            MarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = MDPFactory
                    .createMDPForFigure17_3(cw);
            ValueIteration<Cell<double>, CellWorldAction> vi = new ValueIteration<Cell<double>, CellWorldAction>(
                    1.0);

            IMap<Cell<double>, double> U = vi.valueIteration(mdp, 0.0001);

            System.Console.WriteLine("(1,1) = " + U.Get(cw.getCellAt(1, 1)));
            System.Console.WriteLine("(1,2) = " + U.Get(cw.getCellAt(1, 2)));
            System.Console.WriteLine("(1,3) = " + U.Get(cw.getCellAt(1, 3)));

            System.Console.WriteLine("(2,1) = " + U.Get(cw.getCellAt(2, 1)));
            System.Console.WriteLine("(2,3) = " + U.Get(cw.getCellAt(2, 3)));

            System.Console.WriteLine("(3,1) = " + U.Get(cw.getCellAt(3, 1)));
            System.Console.WriteLine("(3,2) = " + U.Get(cw.getCellAt(3, 2)));
            System.Console.WriteLine("(3,3) = " + U.Get(cw.getCellAt(3, 3)));

            System.Console.WriteLine("(4,1) = " + U.Get(cw.getCellAt(4, 1)));
            System.Console.WriteLine("(4,2) = " + U.Get(cw.getCellAt(4, 2)));
            System.Console.WriteLine("(4,3) = " + U.Get(cw.getCellAt(4, 3)));

            System.Console.WriteLine("=========================");
        }

        public static void policyIterationDemo()
        {

            System.Console.WriteLine("DEMO: Policy Iteration");
            System.Console.WriteLine("======================");
            System.Console.WriteLine("Figure 17.3");
            System.Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
            MarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = MDPFactory
                    .createMDPForFigure17_3(cw);
            PolicyIteration<Cell<double>, CellWorldAction> pi = new PolicyIteration<Cell<double>, CellWorldAction>(
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(50,
                            1.0));

            Policy<Cell<double>, CellWorldAction> policy = pi.policyIteration(mdp);

            System.Console.WriteLine("(1,1) = " + policy.action(cw.getCellAt(1, 1)));
            System.Console.WriteLine("(1,2) = " + policy.action(cw.getCellAt(1, 2)));
            System.Console.WriteLine("(1,3) = " + policy.action(cw.getCellAt(1, 3)));

            System.Console.WriteLine("(2,1) = " + policy.action(cw.getCellAt(2, 1)));
            System.Console.WriteLine("(2,3) = " + policy.action(cw.getCellAt(2, 3)));

            System.Console.WriteLine("(3,1) = " + policy.action(cw.getCellAt(3, 1)));
            System.Console.WriteLine("(3,2) = " + policy.action(cw.getCellAt(3, 2)));
            System.Console.WriteLine("(3,3) = " + policy.action(cw.getCellAt(3, 3)));

            System.Console.WriteLine("(4,1) = " + policy.action(cw.getCellAt(4, 1)));
            System.Console.WriteLine("(4,2) = " + policy.action(cw.getCellAt(4, 2)));
            System.Console.WriteLine("(4,3) = " + policy.action(cw.getCellAt(4, 3)));

            System.Console.WriteLine("=========================");
        }

        //
        // PRIVATE METHODS
        //
        private static void demoToothacheCavityCatchModel(
                FiniteProbabilityModel model)
        {
            System.Console.WriteLine("Toothache, Cavity, and Catch Model");
            System.Console.WriteLine("----------------------------------");
            AssignmentProposition atoothache = new AssignmentProposition(
                    ExampleRV.TOOTHACHE_RV, true);
            AssignmentProposition acavity = new AssignmentProposition(
                    ExampleRV.CAVITY_RV, true);
            AssignmentProposition anotcavity = new AssignmentProposition(
                    ExampleRV.CAVITY_RV, false);
            AssignmentProposition acatch = new AssignmentProposition(
                    ExampleRV.CATCH_RV, true);

            // AIMA3e pg. 485
            System.Console.WriteLine("P(cavity) = " + model.prior(acavity));
            System.Console.WriteLine("P(cavity | toothache) = "
                    + model.posterior(acavity, atoothache));

            // AIMA3e pg. 492
            DisjunctiveProposition cavityOrToothache = new DisjunctiveProposition(
                    acavity, atoothache);
            System.Console.WriteLine("P(cavity OR toothache) = "
                    + model.prior(cavityOrToothache));

            // AIMA3e pg. 493
            System.Console.WriteLine("P(~cavity | toothache) = "
                    + model.posterior(anotcavity, atoothache));

            // AIMA3e pg. 493
            // P<>(Cavity | toothache) = <0.6, 0.4>
            System.Console.WriteLine("P<>(Cavity | toothache) = "
                    + model.posteriorDistribution(ExampleRV.CAVITY_RV, atoothache));

            // AIMA3e pg. 497
            // P<>(Cavity | toothache AND catch) = <0.871, 0.129>
            System.Console.WriteLine("P<>(Cavity | toothache AND catch) = "
                    + model.posteriorDistribution(ExampleRV.CAVITY_RV, atoothache,
                            acatch));
        }

        private static void demoBurglaryAlarmModel(FiniteProbabilityModel model)
        {
            System.Console.WriteLine("--------------------");
            System.Console.WriteLine("Burglary Alarm Model");
            System.Console.WriteLine("--------------------");

            AssignmentProposition aburglary = new AssignmentProposition(
                    ExampleRV.BURGLARY_RV, true);
            AssignmentProposition anotburglary = new AssignmentProposition(
                    ExampleRV.BURGLARY_RV, false);
            AssignmentProposition anotearthquake = new AssignmentProposition(
                    ExampleRV.EARTHQUAKE_RV, false);
            AssignmentProposition aalarm = new AssignmentProposition(
                    ExampleRV.ALARM_RV, true);
            AssignmentProposition anotalarm = new AssignmentProposition(
                    ExampleRV.ALARM_RV, false);
            AssignmentProposition ajohnCalls = new AssignmentProposition(
                    ExampleRV.JOHN_CALLS_RV, true);
            AssignmentProposition amaryCalls = new AssignmentProposition(
                    ExampleRV.MARY_CALLS_RV, true);

            // AIMA3e pg. 514
            System.Console.WriteLine("P(j,m,a,~b,~e) = "
                    + model.prior(ajohnCalls, amaryCalls, aalarm, anotburglary,
                            anotearthquake));
            System.Console.WriteLine("P(j,m,~a,~b,~e) = "
                    + model.prior(ajohnCalls, amaryCalls, anotalarm, anotburglary,
                            anotearthquake));

            // AIMA3e. pg. 514
            // P<>(Alarm | JohnCalls = true, MaryCalls = true, Burglary = false,
            // Earthquake = false)
            // = <0.558, 0.442>
            System.Console
				.WriteLine("P<>(Alarm | JohnCalls = true, MaryCalls = true, Burglary = false, Earthquake = false) = "
                        + model.posteriorDistribution(ExampleRV.ALARM_RV,
                                ajohnCalls, amaryCalls, anotburglary,
                                anotearthquake));

            // AIMA3e pg. 523
            // P<>(Burglary | JohnCalls = true, MaryCalls = true) = <0.284, 0.716>
            System.Console
				.WriteLine("P<>(Burglary | JohnCalls = true, MaryCalls = true) = "
                        + model.posteriorDistribution(ExampleRV.BURGLARY_RV,
                                ajohnCalls, amaryCalls));

            // AIMA3e pg. 528
            // P<>(JohnCalls | Burglary = true)
            System.Console.WriteLine("P<>(JohnCalls | Burglary = true) = "
                    + model.posteriorDistribution(ExampleRV.JOHN_CALLS_RV,
                            aburglary));
        }
    }

}
