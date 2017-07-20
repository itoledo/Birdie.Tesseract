using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.bayes.impl;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.util;

namespace tvn_cosine.ai.test.unit.probability.bayes.impl
{
    [TestClass] public class CPTTest
    {
        public static readonly  double DELTA_THRESHOLD = ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;

        [TestMethod]
        public void test_getConditioningCase()
        {
            RandVar aRV = new RandVar("A", new BooleanDomain());
            RandVar bRV = new RandVar("B", new BooleanDomain());
            RandVar cRV = new RandVar("C", new BooleanDomain());

            CPT cpt = new CPT(cRV, new double[] {
				// A = true, B = true, C = true
				0.1,
				// A = true, B = true, C = false
				0.9,
				// A = true, B = false, C = true
				0.2,
				// A = true, B = false, C = false
				0.8,
				// A = false, B = true, C = true
				0.3,
				// A = false, B = true, C = false
				0.7,
				// A = false, B = false, C = true
				0.4,
				// A = false, B = false, C = false
				0.6 }, aRV, bRV);

            Assert.AreEqual(new double[] { 0.1, 0.9 }, cpt
                    .getConditioningCase(true, true).getValues());

            Assert.AreEqual(new double[] { 0.2, 0.8 }, cpt
                    .getConditioningCase(true, false).getValues());

            Assert.AreEqual(new double[] { 0.3, 0.7 }, cpt
                    .getConditioningCase(false, true).getValues());

            Assert.AreEqual(new double[] { 0.4, 0.6 }, cpt
                    .getConditioningCase(false, false).getValues());

        }
    }

}
