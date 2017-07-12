using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.learners
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class MajorityLearner : Learner
    {
        private string result;

        public void train(DataSet ds)
        {
            IList<string> targets = new List<string>();
            foreach (Example e in ds.examples)
            {
                targets.Add(e.targetValue());
            }
            result = Util.mode(targets);
        }

        public string predict(Example e)
        {
            return result;
        }

        public int[] test(DataSet ds)
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
