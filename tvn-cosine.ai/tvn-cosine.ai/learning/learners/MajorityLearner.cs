 namespace aima.core.learning.learners;

 
 

import aima.core.learning.framework.DataSet;
import aima.core.learning.framework.Example;
import aima.core.learning.framework.Learner;
 

/**
 * @author Ravi Mohan
 * 
 */
public class MajorityLearner : Learner {

	private string result;

	public void train(DataSet ds) {
		List<string> targets = new List<string>();
		for (Example e : ds.examples) {
			targets.Add(e.targetValue());
		}
		result = Util.mode(targets);
	}

	public string predict(Example e) {
		return result;
	}

	public int[] test(DataSet ds) {
		int[] results = new int[] { 0, 0 };

		for (Example e : ds.examples) {
			if (e.targetValue() .Equals(result)) {
				results[0] = results[0] + 1;
			} else {
				results[1] = results[1] + 1;
			}
		}
		return results;
	}
}