﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.agent;
using tvn.cosine.collections.api;
using tvn.cosine.collections;

namespace tvn_cosine.ai.test.unit.agent
{
    [TestClass]
    public class PerceptSequenceTest
    { 
        [TestMethod]
        public void testToString()
        {
            ICollection<IPercept> ps = CollectionFactory.CreateQueue<IPercept>();
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
            ICollection<IPercept> ps1 = CollectionFactory.CreateQueue<IPercept>();
            ICollection<IPercept> ps2 = CollectionFactory.CreateQueue<IPercept>();

            Assert.AreEqual(ps1, ps2);

            ps1.Add(new DynamicPercept("key1", "value1"));

            Assert.AreNotEqual(ps1, ps2);

            ps2.Add(new DynamicPercept("key1", "value1"));

            Assert.AreEqual(ps1, ps2);
        }
    }

}
