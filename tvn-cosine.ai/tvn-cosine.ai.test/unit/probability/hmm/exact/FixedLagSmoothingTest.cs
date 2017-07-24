using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.hmm.exact;
using tvn.cosine.ai.probability.proposition;

namespace tvn_cosine.ai.test.unit.probability.hmm.exact
{
    [TestClass]
    public class FixedLagSmoothingTest
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

        [TestMethod]
        public void testFixedLagSmoothing_lag_1_UmbrellaWorld()
        {
            FixedLagSmoothing uw = new FixedLagSmoothing(HMMExampleFactory.getUmbrellaWorldModel(), 1);

            // Day 1 - Lag 1
            IQueue<AssignmentProposition> e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            CategoricalDistribution smoothed = uw.fixedLagSmoothing(e1);
            Assert.IsNull(smoothed);

            // Day 2 - Lag 1
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e2);

            // Day 1 smoothed probabilities based on 2 days of evidence
            Assert.IsNotNull(smoothed);
            assertArrayEquals(new double[] { 0.883, 0.117 }, smoothed.getValues(), DELTA_THRESHOLD);

            // Day 3 - Lag 1
            IQueue<AssignmentProposition> e3 = Factory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false));

            smoothed = uw.fixedLagSmoothing(e3);

            // Day 2 smoothed probabilities based on 3 days of evidence
            Assert.IsNotNull(smoothed);
            assertArrayEquals(new double[] { 0.799, 0.201 }, smoothed.getValues(), DELTA_THRESHOLD);
        }

        [TestMethod]
        public void testFixedLagSmoothing_lag_2_UmbrellaWorld()
        {
            FixedLagSmoothing uw = new FixedLagSmoothing(HMMExampleFactory.getUmbrellaWorldModel(), 2);

            // Day 1 - Lag 2
            IQueue<AssignmentProposition> e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            CategoricalDistribution smoothed = uw.fixedLagSmoothing(e1);
            Assert.IsNull(smoothed);

            // Day 2 - Lag 2
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e2);
            Assert.IsNull(smoothed);

            // Day 3 - Lag 2
            IQueue<AssignmentProposition> e3 = Factory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false));

            smoothed = uw.fixedLagSmoothing(e3);

            Assert.IsNotNull(smoothed);
            assertArrayEquals(new double[] { 0.861, 0.138 }, smoothed.getValues(), DELTA_THRESHOLD);
        }
    }
}
