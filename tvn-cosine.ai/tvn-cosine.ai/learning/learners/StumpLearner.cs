using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;

namespace tvn.cosine.ai.learning.learners
{
    public class StumpLearner : DecisionTreeLearner
    {
        public StumpLearner(DecisionTree sl, string unable_to_classify)
                : base(sl, unable_to_classify)
        { }

        public override void Train(DataSet ds)
        {
            // System.Console.WriteLine("Stump learner training");
            // do nothing the stump is not inferred from the dataset
        }
    }
}
