using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.api;

namespace tvn_cosine.ai.test.unit.agent
{
    [TestClass]
    public class DynamicAgentTest
    {
        [TestMethod]
        public void TestNullAgentProgram()
        {
            DynamicAgent agent = new DynamicAgent();

            Assert.AreEqual(DynamicAction.NO_OP, agent.Execute(null));
            Assert.IsTrue(agent.IsAlive());
            agent.SetAlive(false);
            Assert.IsFalse(agent.IsAlive());
            Assert.IsInstanceOfType(agent, typeof(IAgent));
        }
    }
}
