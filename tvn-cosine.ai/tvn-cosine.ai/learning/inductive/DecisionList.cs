using tvn.cosine.api;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.text;
using tvn.cosine.text.api;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.inductive
{
    public class DecisionList : IStringable
    {
        private string positive, negative;
        private ICollection<DecisionListTest> tests;
        private IMap<DecisionListTest, string> testOutcomes;

        public DecisionList(string positive, string negative)
        {
            this.positive = positive;
            this.negative = negative;
            this.tests = CollectionFactory.CreateQueue<DecisionListTest>();
            testOutcomes = CollectionFactory.CreateInsertionOrderedMap<DecisionListTest, string>();
        }

        public string predict(Example example)
        {
            if (tests.Size() == 0)
            {
                return negative;
            }
            foreach (DecisionListTest test in tests)
            {
                if (test.matches(example))
                {
                    return testOutcomes.Get(test);
                }
            }
            return negative;
        }

        public void add(DecisionListTest test, string outcome)
        {
            tests.Add(test);
            testOutcomes.Put(test, outcome);
        }

        public DecisionList mergeWith(DecisionList dlist2)
        {
            DecisionList merged = new DecisionList(positive, negative);
            foreach (DecisionListTest test in tests)
            {
                merged.add(test, testOutcomes.Get(test));
            }
            foreach (DecisionListTest test in dlist2.tests)
            {
                merged.add(test, dlist2.testOutcomes.Get(test));
            }
            return merged;
        }

        public override string ToString()
        {
            IStringBuilder buf = TextFactory.CreateStringBuilder();
            foreach (DecisionListTest test in tests)
            {
                buf.Append(test.ToString() + " => " + testOutcomes.Get(test) + " ELSE \n");
            }
            buf.Append("END");
            return buf.ToString();
        }
    } 
}
