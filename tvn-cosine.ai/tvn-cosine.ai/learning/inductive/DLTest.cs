 namespace aima.core.learning.inductive;

 

import aima.core.learning.framework.DataSet;
import aima.core.learning.framework.Example;

/**
 * @author Ravi Mohan
 * 
 */
public class DLTest {

	// represents a single test in the Decision List
	private Dictionary<String, String> attrValues;

	public DLTest() {
		attrValues = new Dictionary<String, String>();
	}

	public void add(string nta, string ntaValue) {
		attrValues.Add(nta, ntaValue);

	}

	public bool matches(Example e) {
		for (string key : attrValues.Keys) {
			if (!(attrValues.get(key) .Equals(e.getAttributeValueAsString(key)))) {
				return false;
			}
		}
		return true;
		// return e.targetValue() .Equals(targetValue);
	}

	public DataSet matchedExamples(DataSet ds) {
		DataSet matched = ds.emptyDataSet();
		for (Example e : ds.examples) {
			if (matches(e)) {
				matched.Add(e);
			}
		}
		return matched;
	}

	public DataSet unmatchedExamples(DataSet ds) {
		DataSet unmatched = ds.emptyDataSet();
		for (Example e : ds.examples) {
			if (!(matches(e))) {
				unmatched.Add(e);
			}
		}
		return unmatched;
	}

	 
	public override string ToString() {
		StringBuilder buf = new StringBuilder();
		buf.Append("IF  ");
		for (string key : attrValues.Keys) {
			buf.Append(key + " = ");
			buf.Append(attrValues.get(key) + " ");
		}
		buf.Append(" DECISION ");
		return buf.ToString();
	}
}
