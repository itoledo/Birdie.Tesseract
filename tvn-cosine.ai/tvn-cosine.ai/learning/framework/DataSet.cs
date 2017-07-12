using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DataSet : IEnumerable<Example>
    {
        protected DataSet()
        { }

        public IList<Example> examples;

        public DataSetSpecification specification;

        public DataSet(DataSetSpecification spec)
        {
            examples = new List<Example>();
            this.specification = spec;
        }

        public void add(Example e)
        {
            examples.Add(e);
        }

        public int size()
        {
            return examples.Count;
        }

        public Example getExample(int number)
        {
            return examples[number];
        }

        public DataSet removeExample(Example e)
        {
            DataSet ds = new DataSet(specification);
            foreach (Example eg in examples)
            {
                if (!(e.Equals(eg)))
                {
                    ds.add(eg);
                }
            }
            return ds;
        }

        public double getInformationFor()
        {
            string attributeName = specification.getTarget();
            IDictionary<string, int> counts = new Dictionary<string, int>();
            foreach (Example e in examples)
            {
                string val = e.getAttributeValueAsString(attributeName);
                if (counts.ContainsKey(val))
                {
                    counts[val] = counts[val] + 1;
                }
                else
                {
                    counts.Add(val, 1);
                }
            }

            double[] data = counts.Values.Select(item => (double)item).ToArray();

            data = Util.normalize(data);

            return Util.information(data);
        }

        public IDictionary<string, DataSet> splitByAttribute(string attributeName)
        {
            IDictionary<string, DataSet> results = new Dictionary<string, DataSet>();
            foreach (Example e in examples)
            {
                string val = e.getAttributeValueAsString(attributeName);
                if (results.ContainsKey(val))
                {
                    results[val].add(e);
                }
                else
                {
                    DataSet ds = new DataSet(specification);
                    ds.add(e);
                    results.Add(val, ds);
                }
            }
            return results;
        }

        public double calculateGainFor(string parameterName)
        {
            IDictionary<string, DataSet> hash = splitByAttribute(parameterName);
            double totalSize = examples.Count;
            double remainder = 0.0;
            foreach (string parameterValue in hash.Keys)
            {
                double reducedDataSetSize = hash[parameterValue].examples.Count;
                remainder += (reducedDataSetSize / totalSize)
                        * hash[parameterValue].getInformationFor();
            }
            return getInformationFor() - remainder;
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if ((o == null) || (GetType() != o.GetType()))
            {
                return false;
            }
            DataSet other = (DataSet)o;
            return examples.Equals(other.examples);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public IEnumerator<Example> iterator()
        {
            return examples.GetEnumerator();
        }

        public DataSet copy()
        {
            DataSet ds = new DataSet(specification);
            foreach (Example e in examples)
            {
                ds.add(e);
            }
            return ds;
        }

        public IList<string> getAttributeNames()
        {
            return specification.getAttributeNames();
        }

        public string getTargetAttributeName()
        {
            return specification.getTarget();
        }

        public DataSet emptyDataSet()
        {
            return new DataSet(specification);
        }

        /**
         * @param specification
         *            The specification to set. USE SPARINGLY for testing etc ..
         *            makes no semantic sense
         */
        public void setSpecification(DataSetSpecification specification)
        {
            this.specification = specification;
        }

        public IList<string> getPossibleAttributeValues(string attributeName)
        {
            return specification.getPossibleAttributeValues(attributeName);
        }

        public DataSet matchingDataSet(string attributeName, string attributeValue)
        {
            DataSet ds = new DataSet(specification);
            foreach (Example e in examples)
            {
                if (e.getAttributeValueAsString(attributeName).Equals(attributeValue))
                {
                    ds.add(e);
                }
            }
            return ds;
        }

        public IList<string> getNonTargetAttributes()
        {
            return Util.removeFrom(getAttributeNames(), getTargetAttributeName());
        }

        public IEnumerator<Example> GetEnumerator()
        {
            return examples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return examples.GetEnumerator();
        }
    }
}
