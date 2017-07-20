using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.inductive
{
    public class DLTestFactory
    {
        public virtual IQueue<DLTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            if (i != 1)
            {
                throw new RuntimeException("For now DLTests with only 1 attribute can be craeted , not" + i);
            }
            IQueue<string> nonTargetAttributes = ds.getNonTargetAttributes();
            IQueue<DLTest> tests = Factory.CreateQueue<DLTest>();
            foreach (string ntAttribute in nonTargetAttributes)
            {
                IQueue<string> ntaValues = ds.getPossibleAttributeValues(ntAttribute);
                foreach (string ntaValue in ntaValues)
                {
                    DLTest test = new DLTest();
                    test.add(ntAttribute, ntaValue);
                    tests.Add(test);
                }
            }
            return tests;
        }
    }
}
