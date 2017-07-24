using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.map;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.uninformed;

namespace tvn_cosine.ai.test.unit.environment.map
{
    [TestClass]
    // [Ignore]
    public class MapAgentTest
    { 
        private ExtendableMap aMap; 
        private StringBuilder envChanges;

        [TestInitialize]
        public void setUp()
        {
            aMap = new ExtendableMap();
            aMap.addBidirectionalLink("A", "B", 5.0);
            aMap.addBidirectionalLink("A", "C", 6.0);
            aMap.addBidirectionalLink("B", "C", 4.0);
            aMap.addBidirectionalLink("C", "D", 7.0);
            aMap.addUnidirectionalLink("B", "E", 14.0);

            envChanges = new StringBuilder();
        }

        [TestMethod] 
        public void testAlreadyAtGoal()
        {
            MapEnvironment me = new MapEnvironment(aMap);
            SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<string, MoveToAction>(), new string[] { "A" });
            me.addAgent(ma, "A");
            me.AddEnvironmentView(new TestEnvironmentView(envChanges));
            me.StepUntilDone();

            Assert.AreEqual(
                    "CurrentLocation=In(A), Goal=In(A):Action[name==NoOp]:METRIC[nodesExpanded]=0:METRIC[queueSize]=0:METRIC[maxQueueSize]=1:METRIC[pathCost]=0:Action[name==NoOp]:",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testNormalSearch()
        {
            MapEnvironment me = new MapEnvironment(aMap);
            SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<string, MoveToAction>(), new string[] { "D" });
            me.addAgent(ma, "A");
            me.AddEnvironmentView(new TestEnvironmentView(envChanges));
            me.StepUntilDone();

            Assert.AreEqual(
                    "CurrentLocation=In(A), Goal=In(D):Action[name==moveTo, location==C]:Action[name==moveTo, location==D]:METRIC[nodesExpanded]=3:METRIC[queueSize]=1:METRIC[maxQueueSize]=3:METRIC[pathCost]=13:Action[name==NoOp]:",
                    envChanges.ToString());
        }

        [TestMethod] 
        public void testNormalSearchGraphSearchMinFrontier()
        {
            MapEnvironment me = new MapEnvironment(aMap);
            UniformCostSearch<string, MoveToAction> ucSearch = new UniformCostSearch<string, MoveToAction>(new GraphSearchReducedFrontier<string, MoveToAction>());

            SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, ucSearch, new string[] { "D" });

            me.addAgent(ma, "A");
            me.AddEnvironmentView(new TestEnvironmentView(envChanges));
            me.StepUntilDone();

            Assert.AreEqual(
                    "CurrentLocation=In(A), Goal=In(D):Action[name==moveTo, location==C]:Action[name==moveTo, location==D]:METRIC[nodesExpanded]=3:METRIC[queueSize]=1:METRIC[maxQueueSize]=2:METRIC[pathCost]=13:Action[name==NoOp]:",
                    envChanges.ToString());
        }

        [TestMethod] 
        public void testNoPath()
        {
            MapEnvironment me = new MapEnvironment(aMap);
            SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<string, MoveToAction>(), new string[] { "A" });
            me.addAgent(ma, "E");
            me.AddEnvironmentView(new TestEnvironmentView(envChanges));
            me.StepUntilDone();

            Assert.AreEqual(
                    "CurrentLocation=In(E), Goal=In(A):Action[name==NoOp]:METRIC[nodesExpanded]=1:METRIC[queueSize]=0:METRIC[maxQueueSize]=1:METRIC[pathCost]=0:Action[name==NoOp]:",
                    envChanges.ToString());
        }

        private class TestEnvironmentView : IEnvironmentView
        {
            private StringBuilder envChanges;

            public TestEnvironmentView(StringBuilder envChanges)
            {
                this.envChanges = envChanges;
            }

            public void Notify(string msg)
            {
                envChanges.Append(msg).Append(":");
            }

            public void AgentAdded(IAgent agent, IEnvironment source)
            {
                // Nothing
            }

            public void AgentActed(IAgent agent, IPercept percept, IAction action, IEnvironment source)
            {
                envChanges.Append(action).Append(":");
            }
        }
    }

}
