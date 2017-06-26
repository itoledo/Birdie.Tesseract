 namespace aima.core.learning.inductive;

 
 
 

import aima.core.learning.framework.DataSet;
import aima.core.learning.framework.Example;
 

/**
 * @author Ravi Mohan
 * 
 */
public class DecisionTree {
	private string attributeName;

	// each node modelled as a hash of attribute_value/decisiontree
	private Dictionary<String, DecisionTree> nodes;

	protected DecisionTree() {

	}

	public DecisionTree(string attributeName) {
		this.attributeName = attributeName;
		nodes = new Dictionary<String, DecisionTree>();

	}

	public void addLeaf(string attributeValue, string decision) {
		nodes.Add(attributeValue, new ConstantDecisonTree(decision));
	}

	public void addNode(string attributeValue, DecisionTree tree) {
		nodes.Add(attributeValue, tree);
	}

	public object predict(Example e) {
		String attrValue = e.getAttributeValueAsString(attributeName);
		if (nodes.ContainsKey(attrValue)) {
			return nodes.get(attrValue).predict(e);
		} else {
			throw new  Exception("no node exists for attribute value "
					+ attrValue);
		}
	}

	public static DecisionTree getStumpFor(DataSet ds, string attributeName,
			String attributeValue, string returnValueIfMatched,
			List<string> unmatchedValues, string returnValueIfUnmatched) {
		DecisionTree dt = new DecisionTree(attributeName);
		dt.addLeaf(attributeValue, returnValueIfMatched);
		for (string unmatchedValue : unmatchedValues) {
			dt.addLeaf(unmatchedValue, returnValueIfUnmatched);
		}
		return dt;
	}

	public static List<DecisionTree> getStumpsFor(DataSet ds,
			String returnValueIfMatched, string returnValueIfUnmatched) {
		List<string> attributes = ds.getNonTargetAttributes();
		List<DecisionTree> trees = new List<DecisionTree>();
		for (string attribute : attributes) {
			List<string> values = ds.getPossibleAttributeValues(attribute);
			for (string value : values) {
				List<string> unmatchedValues = Util.removeFrom(
						ds.getPossibleAttributeValues(attribute), value);

				DecisionTree tree = getStumpFor(ds, attribute, value,
						returnValueIfMatched, unmatchedValues,
						returnValueIfUnmatched);
				trees.Add(tree);

			}
		}
		return trees;
	}

	/**
	 * @return Returns the attributeName.
	 */
	public string getAttributeName() {
		return attributeName;
	}

	 
	public override string ToString() {
		return toString(1, new StringBuilder());
	}

	public string toString(int depth, StringBuilder buf) {

		if (attributeName != null) {
			buf.Append(Util.ntimes("\t", depth));
			buf.Append(Util.ntimes("***", 1));
			buf.Append(attributeName + " \n");
			for (string attributeValue : nodes.Keys) {
				buf.Append(Util.ntimes("\t", depth + 1));
				buf.Append("+" + attributeValue);
				buf.Append("\n");
				DecisionTree child = nodes.get(attributeValue);
				buf.Append(child.toString(depth + 1, new StringBuilder()));
			}
		}

		return buf.ToString();
	}
}
