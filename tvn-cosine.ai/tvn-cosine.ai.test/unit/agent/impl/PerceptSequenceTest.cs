using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common.collections;

namespace tvn_cosine.ai.test.unit.agent.impl
{
    [TestClass]
    public class PerceptSequenceTest
    {

        [TestMethod]
        public void testToString()
        {
            IQueue<IPercept> ps = Factory.CreateQueue<IPercept>();
            ps.Add(new DynamicPercept("key1", "value1"));

            Assert.AreEqual("[Percept[key1==value1]]", ps.ToString());

            ps.Add(new DynamicPercept("key1", "value1", "key2", "value2"));

            Assert.AreEqual(
                    "[Percept[key1==value1], Percept[key1==value1, key2==value2]]",
                    ps.ToString());
        }

        [TestMethod]
        public void testEquals()
        {
            IQueue<IPercept> ps1 = Factory.CreateQueue<IPercept>();
            IQueue<IPercept> ps2 = Factory.CreateQueue<IPercept>();

            Assert.AreEqual(ps1, ps2);

            ps1.Add(new DynamicPercept("key1", "value1"));

            Assert.AreNotEqual(ps1, ps2);

            ps2.Add(new DynamicPercept("key1", "value1"));

            Assert.AreEqual(ps1, ps2);
        }
    }

}
