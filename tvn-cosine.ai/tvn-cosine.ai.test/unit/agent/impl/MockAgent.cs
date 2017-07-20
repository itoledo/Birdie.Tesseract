using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;

namespace tvn_cosine.ai.test.unit.agent.impl
{
    /**
     * @author Ravi Mohan
     * 
     */
    [TestClass]
    public class MockAgent : AbstractAgent
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
