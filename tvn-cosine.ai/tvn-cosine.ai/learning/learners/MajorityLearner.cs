using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.framework.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.learners
{
    public class MajorityLearner : ILearner
    {
        private string result;

        public void Train(DataSet ds)
        {
            ICollection<string> targets = CollectionFactory.CreateQueue<string>();
            foreach (Example e in ds.examples)
            {
                targets.Add(e.targetValue());
            }
            result = Util.mode(targets);
        }

        public string Predict(Example e)
        {
            return result;
        }

        public int[] Test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(result))
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
