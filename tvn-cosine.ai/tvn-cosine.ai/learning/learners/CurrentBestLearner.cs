using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.framework.api;
using tvn.cosine.ai.learning.knowledge;
using tvn.cosine.ai.logic.fol.inference;
using tvn.cosine.ai.logic.fol.kb;

namespace tvn.cosine.ai.learning.learners
{ 
    public class CurrentBestLearner : ILearner
    {
        private string trueGoalValue = null;
        private FOLDataSetDomain folDSDomain = null;
        private FOLKnowledgeBase kb = null;
        private Hypothesis currentBestHypothesis = null;
         
        public CurrentBestLearner(string trueGoalValue)
        {
            this.trueGoalValue = trueGoalValue;
        }
         
        public void train(DataSet ds)
        {
            folDSDomain = new FOLDataSetDomain(ds.specification, trueGoalValue);
            ICollection<FOLExample> folExamples = CollectionFactory.CreateQueue<FOLExample>();
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

        public string Predict(Example e)
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

        public int[] Test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(Predict(e)))
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
    } 
}
