using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
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
            IQueue<Percept> ps = Factory.CreateQueue<Percept>();
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
            IQueue<Percept> ps1 = Factory.CreateQueue<Percept>();
            IQueue<Percept> ps2 = Factory.CreateQueue<Percept>();

            Assert.AreEqual(ps1, ps2);

            ps1.Add(new DynamicPercept("key1", "value1"));

            Assert.AreNotEqual(ps1, ps2);

            ps2.Add(new DynamicPercept("key1", "value1"));

            Assert.AreEqual(ps1, ps2);
        }
    }

}
