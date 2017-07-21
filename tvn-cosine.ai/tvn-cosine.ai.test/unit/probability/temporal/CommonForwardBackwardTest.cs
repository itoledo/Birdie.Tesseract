using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.temporal;
using tvn.cosine.ai.probability.util;

namespace tvn_cosine.ai.test.unit.probability.temporal
{
    public abstract class CommonForwardBackwardTest
    {
        public static readonly double DELTA_THRESHOLD = 1e-3;

        protected static void assertArrayEquals(double[] arr1, double[] arr2, double delta)
        {
            if (arr1.Length != arr2.Length)
            {
                Assert.Fail("Two arrays not same length");
            }

            for (int i = 0; i < arr1.Length; ++i)
            {
                Assert.AreEqual(arr1[i], arr2[i], delta);
            }
        }
        //
        // PROTECTED METHODS
        //
        protected void testForwardStep_UmbrellaWorld(ForwardStepInference uw)
        {
            // AIMA3e pg. 572
            // Day 0, no observations only the security guards prior beliefs
            // P(R<sub>0</sub>) = <0.5, 0.5>
            CategoricalDistribution prior = new ProbabilityTable(new double[] { 0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1, the umbrella appears, so U<sub>1</sub> = true.
            // &asymp; <0.818, 0.182>
            IQueue<AssignmentProposition> e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            CategoricalDistribution f1 = uw.forward(prior, e1);
            assertArrayEquals(new double[] { 0.818, 0.182 }, f1.getValues(), DELTA_THRESHOLD);

            // Day 2, the umbrella appears, so U<sub>2</sub> = true.
            // &asymp; <0.883, 0.117>
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            CategoricalDistribution f2 = uw.forward(f1, e2);
            assertArrayEquals(new double[] { 0.883, 0.117 }, f2.getValues(),
                    DELTA_THRESHOLD);
        }

        protected void testBackwardStep_UmbrellaWorld(BackwardStepInference uw)
        {
            // AIMA3e pg. 575
            CategoricalDistribution b_kp2t = new ProbabilityTable(new double[] { 1.0, 1.0 }, ExampleRV.RAIN_t_RV);
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            CategoricalDistribution b1 = uw.backward(b_kp2t, e2);
            assertArrayEquals(new double[] { 0.69, 0.41 }, b1.getValues(), DELTA_THRESHOLD);
        }

        protected void testForwardBackward_UmbrellaWorld(ForwardBackwardInference uw)
        {
            // AIMA3e pg. 572
            // Day 0, no observations only the security guards prior beliefs
            // P(R<sub>0</sub>) = <0.5, 0.5>
            CategoricalDistribution prior = new ProbabilityTable(new double[] { 0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1
            IQueue<IQueue<AssignmentProposition>> evidence = Factory.CreateQueue<IQueue<AssignmentProposition>>();
            IQueue<AssignmentProposition> e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e1);

            IQueue<CategoricalDistribution> smoothed = uw.forwardBackward(evidence, prior);

            Assert.AreEqual(1, smoothed.Size());
            assertArrayEquals(new double[] { 0.818, 0.182 }, smoothed.Get(0).getValues(), DELTA_THRESHOLD);

            // Day 2
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e2);

            smoothed = uw.forwardBackward(evidence, prior);

            Assert.AreEqual(2, smoothed.Size());
            assertArrayEquals(new double[] { 0.883, 0.117 }, smoothed.Get(0).getValues(), DELTA_THRESHOLD);
            assertArrayEquals(new double[] { 0.883, 0.117 }, smoothed.Get(1).getValues(), DELTA_THRESHOLD);

            // Day 3
            IQueue<AssignmentProposition> e3 = Factory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false));
            evidence.Add(e3);

            smoothed = uw.forwardBackward(evidence, prior);

            Assert.AreEqual(3, smoothed.Size());
            assertArrayEquals(new double[] { 0.861, 0.138 }, smoothed.Get(0).getValues(), DELTA_THRESHOLD);
            assertArrayEquals(new double[] { 0.799, 0.201 }, smoothed.Get(1).getValues(), DELTA_THRESHOLD);
            assertArrayEquals(new double[] { 0.190, 0.810 }, smoothed.Get(2).getValues(), DELTA_THRESHOLD);
        }
    }

}
