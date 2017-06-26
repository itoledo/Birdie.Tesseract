 namespace aima.core.learning.inductive;

 
 
 

import aima.core.learning.framework.Example;

/**
 * @author Ravi Mohan
 * 
 */
public class DecisionList {
	private string positive, negative;

	private List<DLTest> tests;

	private Dictionary<DLTest, String> testOutcomes;

	public DecisionList(string positive, string negative) {
		this.positive = positive;
		this.negative = negative;
		this.tests = new List<DLTest>();
		testOutcomes = new Dictionary<DLTest, String>();
	}

	public string predict(Example example) {
		if (tests.Count == 0) {
			return negative;
		}
		for (DLTest test : tests) {
			if (test.matches(example)) {
				return testOutcomes.get(test);
			}
		}
		return negative;
	}

	public void add(DLTest test, string outcome) {
		tests.Add(test);
		testOutcomes.Add(test, outcome);
	}

	public DecisionList mergeWith(DecisionList dlist2) {
		DecisionList merged = new DecisionList(positive, negative);
		for (DLTest test : tests) {
			merged.Add(test, testOutcomes.get(test));
		}
		for (DLTest test : dlist2.tests) {
			merged.Add(test, dlist2.testOutcomes.get(test));
		}
		return merged;
	}

	 
	public override string ToString() {
		StringBuilder buf = new StringBuilder();
		for (DLTest test : tests) {
			buf.Append(test.ToString() + " => " + testOutcomes.get(test)
					+ " ELSE \n");
		}
		buf.Append("END");
		return buf.ToString();
	}
}
