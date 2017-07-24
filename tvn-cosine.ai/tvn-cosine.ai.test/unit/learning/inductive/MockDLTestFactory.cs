using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;

namespace tvn_cosine.ai.test.unit.learning.inductive
{
    [TestClass]
    public class MockDLTestFactory : DLTestFactory
    {
        private ICollection<DLTest> tests;

        public MockDLTestFactory(ICollection<DLTest> tests)
        {
            this.tests = tests;
        }

        public override ICollection<DLTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            return tests;
        }
    } 
}
