using System.Text;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.inductive
{
    public class DLTest : IStringable
    {
        // represents a single test in the Decision List
        private IMap<string, string> attrValues;

        public DLTest()
        {
            attrValues = Factory.CreateInsertionOrderedMap<string, string>();
        }

        public void add(string nta, string ntaValue)
        {
            attrValues.Put(nta, ntaValue);

        }

        public bool matches(Example e)
        {
            foreach (string key in attrValues.GetKeys())
            {
                if (!(attrValues.Get(key).Equals(e.getAttributeValueAsString(key))))
                {
                    return false;
                }
            }
            return true;
            // return e.targetValue().Equals(targetValue);
        }

        public DataSet matchedExamples(DataSet ds)
        {
            DataSet matched = ds.emptyDataSet();
            foreach (Example e in ds.examples)
            {
                if (matches(e))
                {
                    matched.add(e);
                }
            }
            return matched;
        }

        public DataSet unmatchedExamples(DataSet ds)
        {
            DataSet unmatched = ds.emptyDataSet();
            foreach (Example e in ds.examples)
            {
                if (!(matches(e)))
                {
                    unmatched.add(e);
                }
            }
            return unmatched;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("IF  ");
            foreach (string key in attrValues.GetKeys())
            {
                buf.Append(key + " = ");
                buf.Append(attrValues.Get(key) + " ");
            }
            buf.Append(" DECISION ");
            return buf.ToString();
        }
    } 
}
