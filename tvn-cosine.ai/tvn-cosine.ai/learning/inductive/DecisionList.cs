using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.inductive
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DecisionList
    {
        private string positive, negative;

        private List<DLTest> tests;

        private IDictionary<DLTest, string> testOutcomes;

        public DecisionList(string positive, string negative)
        {
            this.positive = positive;
            this.negative = negative;
            this.tests = new List<DLTest>();
            testOutcomes = new Dictionary<DLTest, string>();
        }

        public string predict(Example example)
        {
            if (tests.Count == 0)
            {
                return negative;
            }
            foreach (DLTest test in tests)
            {
                if (test.matches(example))
                {
                    return testOutcomes[test];
                }
            }
            return negative;
        }

        public void add(DLTest test, string outcome)
        {
            tests.Add(test);
            testOutcomes.Add(test, outcome);
        }

        public DecisionList mergeWith(DecisionList dlist2)
        {
            DecisionList merged = new DecisionList(positive, negative);
            foreach (DLTest test in tests)
            {
                merged.add(test, testOutcomes[test]);
            }
            foreach (DLTest test in dlist2.tests)
            {
                merged.add(test, dlist2.testOutcomes[test]);
            }
            return merged;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (DLTest test in tests)
            {
                buf.Append(test.ToString() + " => " + testOutcomes[test] + " ELSE \n");
            }
            buf.Append("END");
            return buf.ToString();
        }
    }

}
