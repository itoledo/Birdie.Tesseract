using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;

namespace tvn_cosine.ai.test.unit.learning.inductive
{
    [TestClass]
    public class MockDLTestFactory : DecisionListTestFactory
    {
        private ICollection<tvn.cosine.ai.learning.inductive.DecisionListTest> tests;

        public MockDLTestFactory(ICollection<tvn.cosine.ai.learning.inductive.DecisionListTest> tests)
        {
            this.tests = tests;
        }

        public override ICollection<tvn.cosine.ai.learning.inductive.DecisionListTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            return tests;
        }
    } 
}
