namespace tvn_cosine.ai.test.unit.probability.bayes.approx
{
    public class PriorSampleTest
    {

        @Test
        public void testPriorSample_basic()
        {
            // AIMA3e pg. 530
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            MockRandomizer r = new MockRandomizer(
                    new double[] { 0.5, 0.5, 0.5, 0.5 });

            PriorSample ps = new PriorSample(r);
            Map < RandomVariable, Object > event = ps.priorSample(bn);

        Assert.assertEquals(4, event.keySet().size());
        Assert.assertEquals(Boolean.TRUE, event.get(ExampleRV.CLOUDY_RV));
        Assert.assertEquals(Boolean.FALSE, event.get(ExampleRV.SPRINKLER_RV));
        Assert.assertEquals(Boolean.TRUE, event.get(ExampleRV.RAIN_RV));
        Assert.assertEquals(Boolean.TRUE, event.get(ExampleRV.WET_GRASS_RV));
        }
    }

}
