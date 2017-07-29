using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.api;

namespace tvn_cosine.ai.test.unit.agent
{
    [TestClass]
    public class DynamicEnvironmentStateTest
    {
        [TestMethod]
        public void TestInitialisation()
        {
            DynamicEnvironmentState state = new DynamicEnvironmentState();
            Assert.IsInstanceOfType(state, typeof(IEnvironmentState));
            Assert.AreEqual(DynamicEnvironmentState.TYPE, state.DescribeType());
        }
    }
}
