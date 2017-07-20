using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.agent.impl.aprog;
using tvn.cosine.ai.common.collections;

namespace tvn_cosine.ai.test.unit.agent.impl.aprog
{
    [TestClass]
    public class TableDrivenAgentProgramTest
    {

        private static readonly IAction ACTION_1 = new DynamicAction("action1");
        private static readonly IAction ACTION_2 = new DynamicAction("action2");
        private static readonly IAction ACTION_3 = new DynamicAction("action3");

        private AgentBase agent;

        [TestInitialize]
        public void setUp()
        {
            IMap<IQueue<IPercept>, IAction> perceptSequenceActions = Factory.CreateMap<IQueue<IPercept>, IAction>();
            perceptSequenceActions.Put(createPerceptSequence(new DynamicPercept("key1", "value1")), ACTION_1);
            perceptSequenceActions.Put(createPerceptSequence(new DynamicPercept("key1", "value1"),
                            new DynamicPercept("key1", "value2")), ACTION_2);
            perceptSequenceActions.Put(createPerceptSequence(new DynamicPercept("key1", "value1"),
                            new DynamicPercept("key1", "value2"),
                            new DynamicPercept("key1", "value3")), ACTION_3);

            agent = new MockAgent(new TableDrivenAgentProgram(perceptSequenceActions));
        }

        [TestMethod]
        public void testExistingSequences()
        {
            Assert.AreEqual(ACTION_1,
                    agent.Execute(new DynamicPercept("key1", "value1")));
            Assert.AreEqual(ACTION_2,
                    agent.Execute(new DynamicPercept("key1", "value2")));
            Assert.AreEqual(ACTION_3,
                    agent.Execute(new DynamicPercept("key1", "value3")));
        }

        [TestMethod]
        public void testNonExistingSequence()
        {
            Assert.AreEqual(ACTION_1,
                    agent.Execute(new DynamicPercept("key1", "value1")));
            Assert.AreEqual(NoOpAction.NO_OP,
                    agent.Execute(new DynamicPercept("key1", "value3")));
        }

        private static IQueue<IPercept> createPerceptSequence(params IPercept[] percepts)
        {
            IQueue<IPercept> perceptSequence = Factory.CreateQueue<IPercept>();

            foreach (IPercept p in percepts)
            {
                perceptSequence.Add(p);
            }

            return perceptSequence;
        }
    }

}
