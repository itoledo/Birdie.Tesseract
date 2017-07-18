﻿namespace tvn_cosine.ai.test.unit.probability.bayes.approx
{
    public class GibbsAskTest
    {
        public static final double DELTA_THRESHOLD = 0.1;

        /** Mock randomizer - A very skewed distribution results from the choice of 
         * MockRandomizer that always favours one type of sample over others
         */
        @Test
        public void testGibbsAsk_mock()
        {
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(
                ExampleRV.SPRINKLER_RV, Boolean.TRUE) };
            MockRandomizer r = new MockRandomizer(new double[] { 0.5, 0.5, 0.5,
                0.5, 0.5, 0.5, 0.6, 0.5, 0.5, 0.6, 0.5, 0.5 });

            GibbsAsk ga = new GibbsAsk(r);

            double[] estimate = ga.gibbsAsk(
                    new RandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000)
                    .getValues();

            Assert.assertArrayEquals(new double[] { 0, 1 }, estimate, DELTA_THRESHOLD);
        }

        /** Same test as above but with JavaRandomizer
         * <p>
         * Expected result : <br/>
         * P(Rain = true | Sprinkler = true) = 0.3 <br/>
         * P(Rain = false | Sprinkler = true) = 0.7 <br/>
         */
        @Test
        public void testGibbsAsk_basic()
        {
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(
                ExampleRV.SPRINKLER_RV, Boolean.TRUE) };

            GibbsAsk ga = new GibbsAsk();

            double[] estimate = ga.gibbsAsk(
                    new RandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000)
                    .getValues();

            Assert.assertArrayEquals(new double[] { 0.3, 0.7 }, estimate, DELTA_THRESHOLD);
        }

        @Test
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
            AssignmentProposition[] propE = new AssignmentProposition[] { new AssignmentProposition(rvChild, Boolean.TRUE) };

            // sample with LikelihoodWeighting
            CategoricalDistribution samplesLW = new LikelihoodWeighting().ask(rvX, propE, net, 1000);
            assertEquals(0.9, samplesLW.getValue(Boolean.TRUE), DELTA_THRESHOLD);

            // sample with RejectionSampling
            CategoricalDistribution samplesRS = new RejectionSampling().ask(rvX, propE, net, 1000);
            assertEquals(0.9, samplesRS.getValue(Boolean.TRUE), DELTA_THRESHOLD);

            // sample with GibbsAsk
            CategoricalDistribution samplesGibbs = new GibbsAsk().ask(rvX, propE, net, 1000);
            assertEquals(0.9, samplesGibbs.getValue(Boolean.TRUE), DELTA_THRESHOLD);
        }
    }

}
