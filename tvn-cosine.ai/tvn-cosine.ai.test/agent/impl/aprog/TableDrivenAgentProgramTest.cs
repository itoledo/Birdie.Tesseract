using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.agent.impl.aprog;

namespace tvn_cosine.ai.test.agent.impl.aprog
{
    [TestClass]
    public class TableDrivenAgentProgramTest
    {
        private static readonly Action ACTION_1 = new DynamicAction("action1");
        private static readonly Action ACTION_2 = new DynamicAction("action2");
        private static readonly Action ACTION_3 = new DynamicAction("action3");

        private AbstractAgent agent;

        [TestInitialize]
        public void setUp()
        {
            IDictionary<IList<Percept>, Action> perceptSequenceActions = new Dictionary<IList<Percept>, Action>();
            perceptSequenceActions[createPerceptSequence(new DynamicPercept("key1", "value1"))] 
                = ACTION_1;
            perceptSequenceActions[createPerceptSequence(new DynamicPercept("key1", "value1"),
                new DynamicPercept("key1", "value2"))] = ACTION_2;
            perceptSequenceActions[createPerceptSequence(new DynamicPercept("key1", "value1"),
                new DynamicPercept("key1", "value2"),
                new DynamicPercept("key1", "value3"))] = ACTION_3;

            agent = new MockAgent(new TableDrivenAgentProgram(perceptSequenceActions));
        }

        [TestMethod]
        public void testExistingSequences()
        {
            Assert.AreEqual(ACTION_1,
                    agent.execute(new DynamicPercept("key1", "value1")));
            Assert.AreEqual(ACTION_2,
                    agent.execute(new DynamicPercept("key1", "value2")));
            Assert.AreEqual(ACTION_3,
                    agent.execute(new DynamicPercept("key1", "value3")));
        }

        [TestMethod]
        public void testNonExistingSequence()
        {
            Assert.AreEqual(ACTION_1,
                    agent.execute(new DynamicPercept("key1", "value1")));
            Assert.AreEqual(DynamicAction.NO_OP,
                    agent.execute(new DynamicPercept("key1", "value3")));
        }

        private static IList<Percept> createPerceptSequence(params Percept[] percepts)
        {
            IList<Percept> perceptSequence = new List<Percept>();

            foreach (Percept p in percepts)
            {
                perceptSequence.Add(p);
            }

            return perceptSequence;
        }
    }
}
