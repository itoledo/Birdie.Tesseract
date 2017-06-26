 namespace aima.core.learning.framework;

 
 
import java.util.LinkedList;
 

 

/**
 * @author Ravi Mohan
 * 
 */
public class DataSet {
	protected DataSet() {

	}

	public List<Example> examples;

	public DataSetSpecification specification;

	public DataSet(DataSetSpecification spec) {
		examples = new LinkedList<Example>();
		this.specification = spec;
	}

	public void add(Example e) {
		examples.Add(e);
	}

	public int size() {
		return examples.Count;
	}

	public Example getExample(int number) {
		return examples.get(number);
	}

	public DataSet removeExample(Example e) {
		DataSet ds = new DataSet(specification);
		for (Example eg : examples) {
			if (!(e .Equals(eg))) {
				ds.Add(eg);
			}
		}
		return ds;
	}

	public double getInformationFor() {
		String attributeName = specification.getTarget();
		Dictionary<String, int> counts = new Dictionary<String, int>();
		for (Example e : examples) {

			String val = e.getAttributeValueAsString(attributeName);
			if (counts.ContainsKey(val)) {
				counts.Add(val, counts.get(val) + 1);
			} else {
				counts.Add(val, 1);
			}
		}

		double[] data = new double[counts.Keys.Count];
		Iterator<int> iter = counts.values().iterator();
		for (int i = 0; i < data.Length; ++i) {
			data[i] = iter.next();
		}
		data = Util.normalize(data);

		return Util.information(data);
	}

	public Dictionary<String, DataSet> splitByAttribute(string attributeName) {
		Dictionary<String, DataSet> results = new Dictionary<String, DataSet>();
		for (Example e : examples) {
			String val = e.getAttributeValueAsString(attributeName);
			if (results.ContainsKey(val)) {
				results.get(val).Add(e);
			} else {
				DataSet ds = new DataSet(specification);
				ds.Add(e);
				results.Add(val, ds);
			}
		}
		return results;
	}

	public double calculateGainFor(string parameterName) {
		Dictionary<String, DataSet> hash = splitByAttribute(parameterName);
		double totalSize = examples.Count;
		double remainder = 0.0;
		for (string parameterValue : hash.Keys) {
			double reducedDataSetSize = hash.get(parameterValue).examples
					.Count;
			remainder += (reducedDataSetSize / totalSize)
					* hash.get(parameterValue).getInformationFor();
		}
		return getInformationFor() - remainder;
	}

	 
	public override bool Equals(object o) {
		if (this == o) {
			return true;
		}
		if ((o == null) || (this .GetType() != o .GetType())) {
			return false;
		}
		DataSet other = (DataSet) o;
		return examples .Equals(other.examples);
	}

	 
	public override int GetHashCode() {
		return 0;
	}

	public Iterator<Example> iterator() {
		return examples.iterator();
	}

	public DataSet copy() {
		DataSet ds = new DataSet(specification);
		for (Example e : examples) {
			ds.Add(e);
		}
		return ds;
	}

	public List<string> getAttributeNames() {
		return specification.getAttributeNames();
	}

	public string getTargetAttributeName() {
		return specification.getTarget();
	}

	public DataSet emptyDataSet() {
		return new DataSet(specification);
	}

	/**
	 * @param specification
	 *            The specification to set. USE SPARINGLY for testing etc ..
	 *            makes no semantic sense
	 */
	public void setSpecification(DataSetSpecification specification) {
		this.specification = specification;
	}

	public List<string> getPossibleAttributeValues(string attributeName) {
		return specification.getPossibleAttributeValues(attributeName);
	}

	public DataSet matchingDataSet(string attributeName, string attributeValue) {
		DataSet ds = new DataSet(specification);
		for (Example e : examples) {
			if (e.getAttributeValueAsString(attributeName) .Equals(
					attributeValue)) {
				ds.Add(e);
			}
		}
		return ds;
	}

	public List<string> getNonTargetAttributes() {
		return Util.removeFrom(getAttributeNames(), getTargetAttributeName());
	}
}
