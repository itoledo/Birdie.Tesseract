using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;

namespace tvn_cosine.ai.test.learning.inductive
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class MockDLTestFactory : DLTestFactory
    {
        private IList<DLTest> tests;

        public MockDLTestFactory(IList<DLTest> tests)
        {
            this.tests = tests;
        }

        public override IList<DLTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            return tests;
        }
    }
}
