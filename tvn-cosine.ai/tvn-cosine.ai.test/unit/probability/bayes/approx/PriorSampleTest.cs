using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.test.unit.probability.bayes.approx
{
    [TestClass]
    public class PriorSampleTest
    {
        [TestMethod]
        public void testPriorSample_basic()
        {
            // AIMA3e pg. 530
            BayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            IRandom r = new MockRandomizer(
                    new double[] { 0.5, 0.5, 0.5, 0.5 });

            PriorSample ps = new PriorSample(r);
            IMap<RandomVariable, object> even = ps.priorSample(bn);

            Assert.AreEqual(4, even.GetKeys().Size());
            Assert.AreEqual(true, even.Get(ExampleRV.CLOUDY_RV));
            Assert.AreEqual(false, even.Get(ExampleRV.SPRINKLER_RV));
            Assert.AreEqual(true, even.Get(ExampleRV.RAIN_RV));
            Assert.AreEqual(true, even.Get(ExampleRV.WET_GRASS_RV));
        }
    }

}
