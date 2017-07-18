using System;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.framework
{
    public class DataSet : IEquatable, IEnumerable<Example>
    {
        protected DataSet()
        { }

        public IQueue<Example> examples;

        public DataSetSpecification specification;

        public DataSet(DataSetSpecification spec)
        {
            examples = Factory.CreateQueue<Example>();
            this.specification = spec;
        }

        public void add(Example e)
        {
            examples.Add(e);
        }

        public int size()
        {
            return examples.Size();
        }

        public Example getExample(int number)
        {
            return examples.Get(number);
        }

        public DataSet removeExample(Example e)
        {
            DataSet ds = new DataSet(specification);
            foreach (Example eg in examples)
            {
                if (!(e.Equals(eg)))
                {
                    ds.Add(eg);
                }
            }
            return ds;
        }

        public double getInformationFor()
        {
            string attributeName = specification.getTarget();
            IMap<string, int> counts = Factory.CreateMap<string, int>();
            foreach (Example e in examples)
            {
                string val = e.getAttributeValueAsString(attributeName);
                if (counts.ContainsKey(val))
                {
                    counts.Put(val, counts.Get(val) + 1);
                }
                else
                {
                    counts.Put(val, 1);
                }
            }

            double[] data = new double[counts.GetKeys().Size()];
            int i = 0;
            foreach (int value in counts.GetValues())
            {
                data[i] = value;
                ++i;
            }
            data = Util.normalize(data);

            return Util.information(data);
        }

        public IMap<string, DataSet> splitByAttribute(string attributeName)
        {
            IMap<string, DataSet> results = Factory.CreateMap<string, DataSet>();
            foreach (Example e in examples)
            {
                string val = e.getAttributeValueAsString(attributeName);
                if (results.ContainsKey(val))
                {
                    results.Get(val).Add(e);
                }
                else
                {
                    DataSet ds = new DataSet(specification);
                    ds.Add(e);
                    results.Put(val, ds);
                }
            }
            return results;
        }

        public double calculateGainFor(string parameterName)
        {
            IMap<string, DataSet> hash = splitByAttribute(parameterName);
            double totalSize = examples.Size();
            double remainder = 0.0;
            foreach (string parameterValue in hash.GetKeys())
            {
                double reducedDataSetSize = hash.Get(parameterValue).examples.Size();
                remainder += (reducedDataSetSize / totalSize)
                        * hash.Get(parameterValue).getInformationFor();
            }
            return getInformationFor() - remainder;
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if ((o == null) || (this.GetType() != o.GetType()))
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
         
        public DataSet copy()
        {
            DataSet ds = new DataSet(specification);
            foreach (Example e in examples)
            {
                ds.Add(e);
            }
            return ds;
        }

        public IQueue<string> getAttributeNames()
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

        public IQueue<string> getPossibleAttributeValues(string attributeName)
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
                    ds.Add(e);
                }
            }
            return ds;
        }

        public IQueue<string> getNonTargetAttributes()
        {
            return Util.removeFrom(getAttributeNames(), getTargetAttributeName());
        }

        public IEnumerator<Example> GetEnumerator()
        {
            return examples.GetEnumerator();
        }
    }
}
