using System;
using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.inductive
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DLTestFactory
    {

        public List<DLTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            if (i != 1)
            {
                throw new Exception("For now DLTests with only 1 attribute can be craeted , not" + i);
            }
            List<string> nonTargetAttributes = ds.getNonTargetAttributes();
            List<DLTest> tests = new List<DLTest>();
            foreach (string ntAttribute in nonTargetAttributes)
            {
                List<string> ntaValues = ds.getPossibleAttributeValues(ntAttribute);
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
