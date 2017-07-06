using System;
using System.Collections.Generic;
using System.Text;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.inductive
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DLTest
    { 
        // represents a single test in the Decision List
        private IDictionary<string, string> attrValues;

        public DLTest()
        {
            attrValues = new Dictionary<String, String>();
        }

        public void add(String nta, String ntaValue)
        {
            attrValues.Add(nta, ntaValue);

        }

        public bool matches(Example e)
        {
            foreach (string key in attrValues.Keys)
            {
                if (!(attrValues[key].Equals(e.getAttributeValueAsString(key))))
                {
                    return false;
                }
            }
            return true;
            // return e.targetValue().equals(targetValue);
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
            foreach (string key in attrValues.Keys)
            {
                buf.Append(key + " = ");
                buf.Append(attrValues[key] + " ");
            }
            buf.Append(" DECISION ");
            return buf.ToString();
        }
    } 
}
