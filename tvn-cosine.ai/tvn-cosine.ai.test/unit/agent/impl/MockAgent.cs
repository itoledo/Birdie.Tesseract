using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.agent;

namespace tvn_cosine.ai.test.unit.agent.impl
{
    /**
     * @author Ravi Mohan
     * 
     */
    [TestClass]
    public class MockAgent : AgentBase
    {

        public MockAgent()
        {
        }

        public MockAgent(IAgentProgram agent)
            : base(agent)
        {

        }
    }
}
