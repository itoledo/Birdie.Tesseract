using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;

namespace tvn_cosine.ai.test.agent.impl
{
    [TestClass]
    public class PerceptSequenceTest
    {
        [TestMethod]
        public void TestToString()
        {
            IList<Percept> ps = new List<Percept>();
            ps.Add(new DynamicPercept("key1", "value1"));

            Assert.AreEqual("[Percept[key1=value1]]", ps.ToString());

            ps.Add(new DynamicPercept("key1", "value1", "key2", "value2"));

            Assert.AreEqual(
                    "[Percept[key1=value1], Percept[key1=value1, key2=value2]]",
                    ps.ToString());
        }

        [TestMethod]
        public void TestEquals()
        {
            IList<Percept> ps1 = new List<Percept>();
            IList<Percept> ps2 = new List<Percept>();

            Assert.AreEqual(ps1, ps2);

            ps1.Add(new DynamicPercept("key1", "value1"));

            Assert.AreNotEqual(ps1, ps2);

            ps2.Add(new DynamicPercept("key1", "value1"));

            Assert.AreEqual(ps1, ps2);
        }
    }
}
