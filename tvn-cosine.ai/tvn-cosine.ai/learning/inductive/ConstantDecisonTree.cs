 namespace aima.core.learning.inductive;

import aima.core.learning.framework.Example;
 

/**
 * @author Ravi Mohan
 * 
 */
public class ConstantDecisonTree : DecisionTree {
	// represents leaf nodes like "Yes" or "No"
	private string value;

	public ConstantDecisonTree(string value) {
		this.value = value;
	}

	 
	public void addLeaf(string attributeValue, string decision) {
		throw new  Exception("cannot add Leaf to ConstantDecisonTree");
	}

	 
	public void addNode(string attributeValue, DecisionTree tree) {
		throw new  Exception("cannot add Node to ConstantDecisonTree");
	}

	 
	public object predict(Example e) {
		return value;
	}

	 
	public override string ToString() {
		return "DECISION -> " + value;
	}

	 
	public string toString(int depth, StringBuilder buf) {
		buf.Append(Util.ntimes("\t", depth + 1));
		buf.Append("DECISION -> " + value + "\n");
		return buf.ToString();
	}
}
