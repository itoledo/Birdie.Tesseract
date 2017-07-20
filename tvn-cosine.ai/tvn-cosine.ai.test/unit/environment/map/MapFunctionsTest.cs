using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.environment.map;
using tvn.cosine.ai.search.framework.problem;

namespace tvn_cosine.ai.test.unit.environment.map
{
    [TestClass]
    public class MapFunctionsTest
    {
        private ActionsFunction<string, MoveToAction> actionsFn;
        private ResultFunction<string, MoveToAction> resultFn;
        private StepCostFunction<string, MoveToAction> stepCostFn;

        [TestInitialize]
        public void setUp()
        {
            ExtendableMap aMap = new ExtendableMap();
            aMap.addBidirectionalLink("A", "B", 5.0);
            aMap.addBidirectionalLink("A", "C", 6.0);
            aMap.addBidirectionalLink("B", "C", 4.0);
            aMap.addBidirectionalLink("C", "D", 7.0);
            aMap.addUnidirectionalLink("B", "E", 14.0);

            actionsFn = MapFunctions.createActionsFunction(aMap);
            resultFn = MapFunctions.createResultFunction();
            stepCostFn = MapFunctions.createDistanceStepCostFunction(aMap);
        }

        [TestMethod]
        public void testSuccessors()
        {
            IQueue<string> locations = Factory.CreateQueue<string>();

            // A
            locations.Clear();
            locations.Add("B");
            locations.Add("C");
            foreach (MoveToAction a in actionsFn("A"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn("A", a)));
            }

            // B
            locations.Clear();
            locations.Add("A");
            locations.Add("C");
            locations.Add("E");
            foreach (MoveToAction a in actionsFn("B"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn("B", a)));
            }

            // C
            locations.Clear();
            locations.Add("A");
            locations.Add("B");
            locations.Add("D");
            foreach (MoveToAction a in actionsFn("C"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn("C", a)));
            }

            // D
            locations.Clear();
            locations.Add("C");
            foreach (MoveToAction a in actionsFn("D"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn("D", a)));
            }
            // E
            locations.Clear();
            Assert.IsTrue(0 == actionsFn("E").Size());
        }

        [TestMethod]
        public void testCosts()
        {
            Assert.AreEqual(5.0, stepCostFn("A", new MoveToAction("B"), "B"), 0.001);
            Assert.AreEqual(6.0, stepCostFn("A", new MoveToAction("C"), "C"), 0.001);
            Assert.AreEqual(4.0, stepCostFn("B", new MoveToAction("C"), "C"), 0.001);
            Assert.AreEqual(7.0, stepCostFn("C", new MoveToAction("D"), "D"), 0.001);
            Assert.AreEqual(14.0, stepCostFn("B", new MoveToAction("E"), "E"), 0.001);
            //
            Assert.AreEqual(5.0, stepCostFn("B", new MoveToAction("A"), "A"), 0.001);
            Assert.AreEqual(6.0, stepCostFn("C", new MoveToAction("A"), "A"), 0.001);
            Assert.AreEqual(4.0, stepCostFn("C", new MoveToAction("B"), "B"), 0.001);
            Assert.AreEqual(7.0, stepCostFn("D", new MoveToAction("C"), "C"), 0.001);
            //
            Assert.AreEqual(1.0, stepCostFn("X", new MoveToAction("Z"), "Z"), 0.001);
            Assert.AreEqual(1.0, stepCostFn("A", new MoveToAction("Z"), "Z"), 0.001);
            Assert.AreEqual(1.0, stepCostFn("A", new MoveToAction("D"), "D"), 0.001);
            Assert.AreEqual(1.0, stepCostFn("A", new MoveToAction("B"), "E"), 0.001);
        }
    }

}
