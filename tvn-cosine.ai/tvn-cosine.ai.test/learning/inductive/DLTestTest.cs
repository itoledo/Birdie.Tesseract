using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using System.Collections.Generic;

namespace tvn_cosine.ai.test.learning.inductive
{
    [TestClass]
    public class DLTestTest
    {
        [TestMethod]
        public void testDecisionList()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            IList<DLTest> dlTests = new DLTestFactory()
                    .createDLTestsWithAttributeCount(ds, 1);
            Assert.AreEqual(26, dlTests.Count);
        }

        [TestMethod]
        public void testDLTestMatchSucceedsWithMatchedExample()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Example e = ds.getExample(0);
            DLTest test = new DLTest();
            test.add("type", "French");
            Assert.IsTrue(test.matches(e));
        }

        [TestMethod]
        public void testDLTestMatchFailsOnMismatchedExample()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Example e = ds.getExample(0);
            DLTest test = new DLTest();
            test.add("type", "Thai");
            Assert.IsFalse(test.matches(e));
        }

        [TestMethod]
        public void testDLTestMatchesEvenOnMismatchedTargetAttributeValue()


        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Example e = ds.getExample(0);
            DLTest test = new DLTest();
            test.add("type", "French");
            Assert.IsTrue(test.matches(e));
        }

        [TestMethod]
        public void testDLTestReturnsMatchedAndUnmatchedExamplesCorrectly()


        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DLTest test = new DLTest();
            test.add("type", "Burger");

            DataSet matched = test.matchedExamples(ds);
            Assert.AreEqual(4, matched.Count);

            DataSet unmatched = test.unmatchedExamples(ds);
            Assert.AreEqual(8, unmatched.Count);
        }
    }
}
