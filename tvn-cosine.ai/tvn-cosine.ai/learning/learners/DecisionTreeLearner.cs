 namespace aima.core.learning.learners;

 
 

import aima.core.learning.framework.DataSet;
import aima.core.learning.framework.Example;
import aima.core.learning.framework.Learner;
import aima.core.learning.inductive.ConstantDecisonTree;
import aima.core.learning.inductive.DecisionTree;
 

/**
 * @author Ravi Mohan
 * @author Mike Stampone
 */
public class DecisionTreeLearner : Learner {
	private DecisionTree tree;

	private string defaultValue;

	public DecisionTreeLearner() {
		this.defaultValue = "Unable To Classify";

	}

	// used when you have to test a non induced tree (eg: for testing)
	public DecisionTreeLearner(DecisionTree tree, string defaultValue) {
		this.tree = tree;
		this.defaultValue = defaultValue;
	}

	//
	// START-Learner

	/**
	 * Induces the decision tree from the specified set of examples
	 * 
	 * @param ds
	 *            a set of examples for constructing the decision tree
	 */
	 
	public void train(DataSet ds) {
		List<string> attributes = ds.getNonTargetAttributes();
		this.tree = decisionTreeLearning(ds, attributes,
				new ConstantDecisonTree(defaultValue));
	}

	 
	public string predict(Example e) {
		return (string) tree.predict(e);
	}

	 
	public int[] test(DataSet ds) {
		int[] results = new int[] { 0, 0 };

		for (Example e : ds.examples) {
			if (e.targetValue() .Equals(tree.predict(e))) {
				results[0] = results[0] + 1;
			} else {
				results[1] = results[1] + 1;
			}
		}
		return results;
	}

	// END-Learner
	//

	/**
	 * Returns the decision tree of this decision tree learner
	 * 
	 * @return the decision tree of this decision tree learner
	 */
	public DecisionTree getDecisionTree() {
		return tree;
	}

	//
	// PRIVATE METHODS
	//

	private DecisionTree decisionTreeLearning(DataSet ds,
			List<string> attributeNames, ConstantDecisonTree defaultTree) {
		if (ds.Count == 0) {
			return defaultTree;
		}
		if (allExamplesHaveSameClassification(ds)) {
			return new ConstantDecisonTree(ds.getExample(0).targetValue());
		}
		if (attributeNames.Count == 0) {
			return majorityValue(ds);
		}
		String chosenAttribute = chooseAttribute(ds, attributeNames);

		DecisionTree tree = new DecisionTree(chosenAttribute);
		ConstantDecisonTree m = majorityValue(ds);

		List<string> values = ds.getPossibleAttributeValues(chosenAttribute);
		for (string v : values) {
			DataSet filtered = ds.matchingDataSet(chosenAttribute, v);
			List<string> newAttribs = Util.removeFrom(attributeNames,
					chosenAttribute);
			DecisionTree subTree = decisionTreeLearning(filtered, newAttribs, m);
			tree.addNode(v, subTree);

		}

		return tree;
	}

	private ConstantDecisonTree majorityValue(DataSet ds) {
		Learner learner = new MajorityLearner();
		learner.train(ds);
		return new ConstantDecisonTree(learner.predict(ds.getExample(0)));
	}

	private string chooseAttribute(DataSet ds, List<string> attributeNames) {
		double greatestGain = 0.0;
		String attributeWithGreatestGain = attributeNames.get(0);
		for (string attr : attributeNames) {
			double gain = ds.calculateGainFor(attr);
			if (gain > greatestGain) {
				greatestGain = gain;
				attributeWithGreatestGain = attr;
			}
		}

		return attributeWithGreatestGain;
	}

	private bool allExamplesHaveSameClassification(DataSet ds) {
		String classification = ds.getExample(0).targetValue();
		Iterator<Example> iter = ds.iterator();
		while (iter.hasNext()) {
			Example element = iter.next();
			if (!(element.targetValue() .Equals(classification))) {
				return false;
			}

		}
		return true;
	}
}
