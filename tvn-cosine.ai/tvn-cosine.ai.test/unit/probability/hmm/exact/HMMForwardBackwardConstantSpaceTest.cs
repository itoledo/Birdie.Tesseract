using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.hmm.exact;
using tvn_cosine.ai.test.unit.probability.temporal;

namespace tvn_cosine.ai.test.unit.probability.hmm.exact
{
    [TestClass]
    public class HMMForwardBackwardConstantSpaceTest : CommonForwardBackwardTest
    {

        //
        private HMMForwardBackwardConstantSpace uw = null;

        [TestInitialize]
        public void setUp()
        {
            uw = new HMMForwardBackwardConstantSpace(
                    HMMExampleFactory.getUmbrellaWorldModel());
        }

        [TestMethod]
        public void testForwardBackward_UmbrellaWorld()
        {
            base.testForwardBackward_UmbrellaWorld(uw);
        }
    }

}
