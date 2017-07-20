using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.hmm.exact;
using tvn_cosine.ai.test.unit.probability.temporal;

namespace tvn_cosine.ai.test.unit.probability.hmm.exact
{
    [TestClass]
    public class HMMForwardBackwardTest : CommonForwardBackwardTest
    {

        //
        private HMMForwardBackward uw = null;

        [TestInitialize]
        public void setUp()
        {
            uw = new HMMForwardBackward(HMMExampleFactory.getUmbrellaWorldModel());
        }

        [TestMethod]
        public void testForwardStep_UmbrellaWorld()
        {
            base.testForwardStep_UmbrellaWorld(uw);
        }

        [TestMethod]
        public void testBackwardStep_UmbrellaWorld()
        {
            base.testBackwardStep_UmbrellaWorld(uw);
        }

        [TestMethod]
        public void testForwardBackward_UmbrellaWorld()
        {
            base.testForwardBackward_UmbrellaWorld(uw);
        }
    }

}
