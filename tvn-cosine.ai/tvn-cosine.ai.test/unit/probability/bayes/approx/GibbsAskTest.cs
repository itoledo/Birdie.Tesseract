using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.bayes.impl;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.test.unit.probability.bayes.approx
{
    [TestClass]
    public class GibbsAskTest
    {
        public static readonly double DELTA_THRESHOLD = 0.1;

        /** Mock randomizer - A very skewed distribution results from the choice of 
         * IRandom that always favours one type of sample over others
         */
        [TestMethod]
        public void testGibbsAsk_mock()
        {
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(
                ExampleRV.SPRINKLER_RV, true) };
            IRandom r = new MockRandomizer(new double[] { 0.5, 0.5, 0.5,
                0.5, 0.5, 0.5, 0.6, 0.5, 0.5, 0.6, 0.5, 0.5 });

            GibbsAsk ga = new GibbsAsk(r);

            double[] estimate = ga.gibbsAsk(
                    new RandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000)
                    .getValues();

            Assert.AreEqual(new double[] { 0, 1 }, estimate);
        }

        /** Same test as above but with JavaRandomizer
         * <p>
         * Expected result : <br/>
         * P(Rain = true | Sprinkler = true) = 0.3 <br/>
         * P(Rain = false | Sprinkler = true) = 0.7 <br/>
         */
        [TestMethod]
        public void testGibbsAsk_basic()
        {
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(
                ExampleRV.SPRINKLER_RV, true) };

            GibbsAsk ga = new GibbsAsk();

            double[] estimate = ga.gibbsAsk(
                    new RandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000)
                    .getValues();

            Assert.AreEqual(new double[] { 0.3, 0.7 }, estimate);
        }

        [TestMethod]
        public void testGibbsAsk_compare()
        {
            // create two nodes: parent and child with an arc from parent to child 
            RandomVariable rvParent = new RandVar("Parent", new BooleanDomain());
            RandomVariable rvChild = new RandVar("Child", new BooleanDomain());
            FullCPTNode nodeParent = new FullCPTNode(rvParent, new double[] { 0.7, 0.3 });
            new FullCPTNode(rvChild, new double[] { 0.8, 0.2, 0.2, 0.8 }, nodeParent);

            // create net
            BayesNet net = new BayesNet(nodeParent);

            // query parent probability
            RandomVariable[] rvX = new RandomVariable[] { rvParent };

            // ...given child evidence (true)
            AssignmentProposition[] propE = new AssignmentProposition[] { new AssignmentProposition(rvChild, true) };

            // sample with LikelihoodWeighting
            CategoricalDistribution samplesLW = new LikelihoodWeighting().ask(rvX, propE, net, 1000);
            Assert.AreEqual(0.9, samplesLW.getValue(true) );

            // sample with RejectionSampling
            CategoricalDistribution samplesRS = new RejectionSampling().ask(rvX, propE, net, 1000);
            Assert.AreEqual(0.9, samplesRS.getValue(true) );

            // sample with GibbsAsk
            CategoricalDistribution samplesGibbs = new GibbsAsk().ask(rvX, propE, net, 1000);
            Assert.AreEqual(0.9, samplesGibbs.getValue(true) );
        }
    }

}
