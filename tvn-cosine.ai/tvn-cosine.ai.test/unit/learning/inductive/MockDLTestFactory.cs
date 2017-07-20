using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;

namespace tvn_cosine.ai.test.unit.learning.inductive
{
    [TestClass]
    public class MockDLTestFactory : DLTestFactory
    {
        private IQueue<DLTest> tests;

        public MockDLTestFactory(IQueue<DLTest> tests)
        {
            this.tests = tests;
        }

        public override IQueue<DLTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            return tests;
        }
    } 
}
