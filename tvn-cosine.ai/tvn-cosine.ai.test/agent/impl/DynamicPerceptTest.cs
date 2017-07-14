using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent.impl;

namespace tvn_cosine.ai.test.agent.impl
{
    [TestClass]
    public class DynamicPerceptTest
    {
        [TestMethod]
        public void TestToString()
        {
            DynamicPercept p = new DynamicPercept("key1", "value1");

            Assert.AreEqual("Percept[key1=value1]", p.ToString());

            p = new DynamicPercept("key1", "value1", "key2", "value2");

            Assert.AreEqual("Percept[key1=value1, key2=value2]", p.ToString());
        }

        [TestMethod]
        public void testEquals()
        {
            DynamicPercept p1 = new DynamicPercept();
            DynamicPercept p2 = new DynamicPercept();

            Assert.AreEqual(p1, p2);

            p1 = new DynamicPercept("key1", "value1");

            Assert.AreNotEqual(p1, p2);

            p2 = new DynamicPercept("key1", "value1");

            Assert.AreEqual(p1, p2);
        }
    }
}
