using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.knowledge;

namespace tvn.cosine.ai.learning.learners
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class CurrentBestLearner : Learner
    {
        private string trueGoalValue = null;
        private FOLDataSetDomain folDSDomain = null;
        private FOLKnowledgeBase kb = null;
        private Hypothesis currentBestHypothesis = null;

        //
        // PUBLIC METHODS
        //
        public CurrentBestLearner(string trueGoalValue)
        {
            this.trueGoalValue = trueGoalValue;
        }

        //
        // START-Learner
        public void train(DataSet ds)
        {
            folDSDomain = new FOLDataSetDomain(ds.specification, trueGoalValue);
            IQueue<FOLExample> folExamples = Factory.CreateQueue<FOLExample>();
            int egNo = 1;
            foreach (Example e in ds.examples)
            {
                folExamples.Add(new FOLExample(folDSDomain, e, egNo));
                egNo++;
            }

            // Setup a KB to be used for learning
            kb = new FOLKnowledgeBase(folDSDomain, new FOLOTTERLikeTheoremProver(1000, false));

            CurrentBestLearning cbl = new CurrentBestLearning(folDSDomain, kb);

            currentBestHypothesis = cbl.currentBestLearning(folExamples);
        }

        public string predict(Example e)
        {
            string prediction = "~" + e.targetValue();
            if (null != currentBestHypothesis)
            {
                FOLExample etp = new FOLExample(folDSDomain, e, 0);
                kb.clear();
                kb.tell(etp.getDescription());
                kb.tell(currentBestHypothesis.getHypothesis());
                InferenceResult ir = kb.ask(etp.getClassification());
                if (ir.isTrue())
                {
                    if (trueGoalValue.Equals(e.targetValue()))
                    {
                        prediction = e.targetValue();
                    }
                }
                else if (ir.isPossiblyFalse() || ir.isUnknownDueToTimeout())
                {
                    if (!trueGoalValue.Equals(e.targetValue()))
                    {
                        prediction = e.targetValue();
                    }
                }
            }

            return prediction;
        }

        public int[] test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(predict(e)))
                {
                    results[0] = results[0] + 1;
                }
                else
                {
                    results[1] = results[1] + 1;
                }
            }
            return results;
        }
        // END-Learner
        //
    } 
}
