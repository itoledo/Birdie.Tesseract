using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.agent.impl.aprog;
using tvn.cosine.ai.common.collections;

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
            IMap<IQueue<Percept>, Action> perceptSequenceActions = Factory.CreateMap<IQueue<Percept>, Action>();
            perceptSequenceActions.Put(
                createPerceptSequence(
                    new DynamicPercept("key1", "value1")), 
                ACTION_1);
            perceptSequenceActions.Put(
                createPerceptSequence(
                    new DynamicPercept("key1", "value1"),
                    new DynamicPercept("key1", "value2")), 
                ACTION_2);
            perceptSequenceActions.Put(
                createPerceptSequence(
                    new DynamicPercept("key1", "value1"),
                    new DynamicPercept("key1", "value2"),
                    new DynamicPercept("key1", "value3")), 
                ACTION_3);

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
            Assert.AreEqual(NoOpAction.NO_OP,
                    agent.execute(new DynamicPercept("key1", "value3")));
        }

        private static IQueue<Percept> createPerceptSequence(params Percept[] percepts)
        {
            IQueue<Percept> perceptSequence = Factory.CreateQueue<Percept>();

            foreach (Percept p in percepts)
            {
                perceptSequence.Add(p);
            }

            return perceptSequence;
        }
    }
}
