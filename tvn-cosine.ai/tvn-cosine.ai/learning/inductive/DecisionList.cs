using System.Text;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.inductive
{
    public class DecisionList : IToString
    {
        private string positive, negative;
        private IQueue<DLTest> tests;
        private IMap<DLTest, string> testOutcomes;

        public DecisionList(string positive, string negative)
        {
            this.positive = positive;
            this.negative = negative;
            this.tests = Factory.CreateQueue<DLTest>();
            testOutcomes = Factory.CreateMap<DLTest, string>();
        }

        public string predict(Example example)
        {
            if (tests.Size() == 0)
            {
                return negative;
            }
            foreach (DLTest test in tests)
            {
                if (test.matches(example))
                {
                    return testOutcomes.Get(test);
                }
            }
            return negative;
        }

        public void add(DLTest test, string outcome)
        {
            tests.Add(test);
            testOutcomes.Put(test, outcome);
        }

        public DecisionList mergeWith(DecisionList dlist2)
        {
            DecisionList merged = new DecisionList(positive, negative);
            foreach (DLTest test in tests)
            {
                merged.Add(test, testOutcomes.Get(test));
            }
            foreach (DLTest test in dlist2.tests)
            {
                merged.Add(test, dlist2.testOutcomes.Get(test));
            }
            return merged;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (DLTest test in tests)
            {
                buf.Append(test.ToString() + " => " + testOutcomes.Get(test) + " ELSE \n");
            }
            buf.Append("END");
            return buf.ToString();
        }
    }

}
