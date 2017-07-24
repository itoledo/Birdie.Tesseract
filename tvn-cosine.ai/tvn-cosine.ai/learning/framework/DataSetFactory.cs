using System.Text.RegularExpressions;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.framework
{ 
    public class DataSetFactory
    { 
        public DataSet fromFile(string filename, DataSetSpecification spec, string separator)
        {
            // assumed file in data directory and ends in .csv
            DataSet ds = new DataSet(spec);

            if (!System.IO.File.Exists(filename + ".csv"))
            {
                throw new FileNotFoundException(filename + ".csv" + "does not exist.");
            }
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename + ".csv"))
            {
                string line = string.Empty;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(line))
                    {
                        ds.add(exampleFromString(line, spec, separator));
                    }
                }

            }

            return ds;
        }

        public static Example exampleFromString(string data, DataSetSpecification dataSetSpec, string separator)
        {
            Regex splitter = new Regex(separator);
            IMap<string, IAttribute> attributes = CollectionFactory.CreateInsertionOrderedMap<string, IAttribute>();
            ICollection<string> attributeValues = CollectionFactory.CreateQueue<string>(splitter.Split(data));
            if (dataSetSpec.isValid(attributeValues))
            {
                ICollection<string> names = dataSetSpec.getAttributeNames();
                int min = names.Size() > attributes.Size() ? names.Size() : attributes.Size();

                for (int i = 0; i < min; ++i)
                {
                    string name = names.Get(i);
                    IAttributeSpecification attributeSpec = dataSetSpec.getAttributeSpecFor(name);
                    IAttribute attribute = attributeSpec.CreateAttribute(attributeValues.Get(i));
                    attributes.Put(name, attribute);
                }
                string targetAttributeName = dataSetSpec.getTarget();
                return new Example(attributes, attributes.Get(targetAttributeName));
            }
            else
            {
                throw new RuntimeException("Unable to construct Example from " + data);
            }
        }

        public static DataSet getRestaurantDataSet()
        {
            DataSetSpecification spec = createRestaurantDataSetSpec();
            return new DataSetFactory().fromFile("restaurant", spec, "\\s+");
        }

        public static DataSetSpecification createRestaurantDataSetSpec()
        {
            DataSetSpecification dss = new DataSetSpecification();
            dss.defineStringAttribute("alternate", Util.yesno());
            dss.defineStringAttribute("bar", Util.yesno());
            dss.defineStringAttribute("fri/sat", Util.yesno());
            dss.defineStringAttribute("hungry", Util.yesno());
            dss.defineStringAttribute("patrons", new string[] { "None", "Some", "Full" });
            dss.defineStringAttribute("price", new string[] { "$", "$$", "$$$" });
            dss.defineStringAttribute("raining", Util.yesno());
            dss.defineStringAttribute("reservation", Util.yesno());
            dss.defineStringAttribute("type", new string[] { "French", "Italian", "Thai", "Burger" });
            dss.defineStringAttribute("wait_estimate", new string[] { "0-10", "10-30", "30-60", ">60" });
            dss.defineStringAttribute("will_wait", Util.yesno());
            // last attribute is the target attribute unless the target is explicitly reset with dss.setTarget(name)

            return dss;
        }

        public static DataSet getIrisDataSet()
        {
            DataSetSpecification spec = createIrisDataSetSpec();
            return new DataSetFactory().fromFile("iris", spec, ",");
        }

        public static DataSetSpecification createIrisDataSetSpec()
        {
            DataSetSpecification dss = new DataSetSpecification();
            dss.defineNumericAttribute("sepal_length");
            dss.defineNumericAttribute("sepal_width");
            dss.defineNumericAttribute("petal_length");
            dss.defineNumericAttribute("petal_width");
            dss.defineStringAttribute("plant_category", new string[] { "setosa", "versicolor", "virginica" });
            return dss;
        }
    }
}
