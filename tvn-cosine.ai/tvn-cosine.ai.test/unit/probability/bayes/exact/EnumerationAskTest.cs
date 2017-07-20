using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.probability.bayes.exact;

namespace tvn_cosine.ai.test.unit.probability.bayes.exact
{
    [TestClass]
    public class EnumerationAskTest : BayesianInferenceTest
    {
        [TestInitialize]

        public override void setUp()
        {
            bayesInference = new EnumerationAsk();
        }
    }
}
