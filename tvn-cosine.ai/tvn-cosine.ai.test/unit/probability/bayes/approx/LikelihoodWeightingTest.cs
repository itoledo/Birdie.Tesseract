using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.test.unit.probability.bayes.approx
{
    [TestClass]
    public class LikelihoodWeightingTest
    { 
        public static readonly double DELTA_THRESHOLD = ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;

        [TestMethod]
        public void testLikelihoodWeighting_basic()
        {
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(
                ExampleRV.SPRINKLER_RV, true) };
            IRandom r = new MockRandomizer(
                    new double[] { 0.5, 0.5, 0.5, 0.5 });

            LikelihoodWeighting lw = new LikelihoodWeighting(r);

            double[] estimate = lw.likelihoodWeighting(
                    new RandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000)
                    .getValues();

            Assert.AreEqual(new double[] { 1.0, 0.0 }, estimate );
        }

        [TestMethod]
        public void testLikelihoodWeighting_AIMA3e_pg533()
        {
            // AIMA3e pg. 533
            // <b>P</b>(Rain | Cloudy = true, WetGrass = true)
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] {
                new AssignmentProposition(ExampleRV.CLOUDY_RV, true),
                new AssignmentProposition(ExampleRV.WET_GRASS_RV, true) };
            // sample P(Sprinkler | Cloudy = true) = <0.1, 0.9>; suppose
            // Sprinkler=false
            // sample P(Rain | Cloudy = true) = <0.8, 0.2>; suppose Rain=true
            IRandom r = new MockRandomizer(new double[] { 0.5, 0.5 });

            LikelihoodWeighting lw = new LikelihoodWeighting(r);
            double[] estimate = lw.likelihoodWeighting(
                    new RandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1)
                    .getValues();

            // Here the even [true,false,true,true] should have weight 0.45,
            // and this is tallied under Rain = true, which when normalized
            // should be <1.0, 0.0>;
            Assert.AreEqual(new double[] { 1.0, 0.0 }, estimate );
        }
    }

}
