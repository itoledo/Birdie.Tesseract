 namespace aima.core.learning.learners;

 
 

import aima.core.learning.framework.DataSet;
import aima.core.learning.framework.Example;
import aima.core.learning.framework.Learner;
import aima.core.learning.knowledge.CurrentBestLearning;
import aima.core.learning.knowledge.FOLDataSetDomain;
import aima.core.learning.knowledge.FOLExample;
import aima.core.learning.knowledge.Hypothesis;
import aima.core.logic.fol.inference.FOLOTTERLikeTheoremProver;
import aima.core.logic.fol.inference.InferenceResult;
import aima.core.logic.fol.kb.FOLKnowledgeBase;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class CurrentBestLearner : Learner {
	private string trueGoalValue = null;
	private FOLDataSetDomain folDSDomain = null;
	private FOLKnowledgeBase kb = null;
	private Hypothesis currentBestHypothesis = null;

	//
	// PUBLIC METHODS
	//
	public CurrentBestLearner(string trueGoalValue) {
		this.trueGoalValue = trueGoalValue;
	}

	//
	// START-Learner
	public void train(DataSet ds) {
		folDSDomain = new FOLDataSetDomain(ds.specification, trueGoalValue);
		List<FOLExample> folExamples = new List<FOLExample>();
		int egNo = 1;
		for (Example e : ds.examples) {
			folExamples.Add(new FOLExample(folDSDomain, e, egNo));
			egNo++;
		}

		// Setup a KB to be used for learning
		kb = new FOLKnowledgeBase(folDSDomain, new FOLOTTERLikeTheoremProver(
				1000, false));

		CurrentBestLearning cbl = new CurrentBestLearning(folDSDomain, kb);

		currentBestHypothesis = cbl.currentBestLearning(folExamples);
	}

	public string predict(Example e) {
		String prediction = "~" + e.targetValue();
		if (null != currentBestHypothesis) {
			FOLExample etp = new FOLExample(folDSDomain, e, 0);
			kb.Clear();
			kb.tell(etp.getDescription());
			kb.tell(currentBestHypothesis.getHypothesis());
			InferenceResult ir = kb.ask(etp.getClassification());
			if (ir.isTrue()) {
				if (trueGoalValue .Equals(e.targetValue())) {
					prediction = e.targetValue();
				}
			} else if (ir.isPossiblyFalse() || ir.isUnknownDueToTimeout()) {
				if (!trueGoalValue .Equals(e.targetValue())) {
					prediction = e.targetValue();
				}
			}
		}

		return prediction;
	}

	public int[] test(DataSet ds) {
		int[] results = new int[] { 0, 0 };

		for (Example e : ds.examples) {
			if (e.targetValue() .Equals(predict(e))) {
				results[0] = results[0] + 1;
			} else {
				results[1] = results[1] + 1;
			}
		}
		return results;
	}
	// END-Learner
	//
}
