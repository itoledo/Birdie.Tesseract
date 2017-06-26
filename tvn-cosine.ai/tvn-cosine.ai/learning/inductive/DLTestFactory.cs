 namespace aima.core.learning.inductive;

 
 

import aima.core.learning.framework.DataSet;

/**
 * @author Ravi Mohan
 * 
 */
public class DLTestFactory {

	public List<DLTest> createDLTestsWithAttributeCount(DataSet ds, int i) {
		if (i != 1) {
			throw new  Exception(
					"For now DLTests with only 1 attribute can be craeted , not"
							+ i);
		}
		List<string> nonTargetAttributes = ds.getNonTargetAttributes();
		List<DLTest> tests = new List<DLTest>();
		for (string ntAttribute : nonTargetAttributes) {
			List<string> ntaValues = ds.getPossibleAttributeValues(ntAttribute);
			for (string ntaValue : ntaValues) {

				DLTest test = new DLTest();
				test.Add(ntAttribute, ntaValue);
				tests.Add(test);

			}
		}
		return tests;
	}
}
