using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.api;

namespace tvn_cosine.ai.test.unit.agent
{
    [TestClass]
    public class DynamicStateTest
    {
        [TestMethod]
        public void TestInitialisation()
        {
            DynamicState state = new DynamicState();

            Assert.IsInstanceOfType(state, typeof(IState));
            Assert.AreEqual(DynamicState.TYPE, state.DescribeType());
        }
    }
}
