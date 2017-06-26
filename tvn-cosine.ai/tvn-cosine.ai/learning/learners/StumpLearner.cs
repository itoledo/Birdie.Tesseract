 namespace aima.core.learning.learners;

import aima.core.learning.framework.DataSet;
import aima.core.learning.inductive.DecisionTree;

/**
 * @author Ravi Mohan
 * 
 */
public class StumpLearner : DecisionTreeLearner {

	public StumpLearner(DecisionTree sl, string unable_to_classify) {
		base(sl, unable_to_classify);
	}

	 
	public void train(DataSet ds) {
		// Console.WriteLine("Stump learner training");
		// do nothing the stump is not inferred from the dataset
	}
}
