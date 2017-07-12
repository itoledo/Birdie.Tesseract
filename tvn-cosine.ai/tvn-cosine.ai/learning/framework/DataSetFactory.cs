using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DataSetFactory
    {

        public static DataSet FromFile(string filename, DataSetSpecification spec, string separator)
        {
            // assumed file in data directory and ends in .csv
            DataSet ds = new DataSet(spec);
            if (!System.IO.File.Exists(filename))
            {
                throw new FileNotFoundException(filename);
            }

            using (var reader = new StreamReader(filename))
            {
                string line;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    ds.add(exampleFromString(line, spec, separator));
                }
            }

            return ds;
        }

        public static Example exampleFromString(string data, DataSetSpecification dataSetSpec, string separator)
        {
            IDictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();
            var attributeValues = new Regex(separator).Split(data.Trim()).ToList();
            if (dataSetSpec.isValid(attributeValues))
            {
                IList<string> names = dataSetSpec.getAttributeNames();
                int min = Math.Min(names.Count, attributeValues.Count);
                for (int i = 0; i < min; ++i)
                {
                    string name = names[i];
                    AttributeSpecification attributeSpec = dataSetSpec.getAttributeSpecFor(name);
                    Attribute attribute = attributeSpec.createAttribute(attributeValues[i]);
                    attributes.Add(name, attribute);
                }

                string targetAttributeName = dataSetSpec.getTarget();
                return new Example(attributes, attributes[targetAttributeName]);
            }
            else
            {
                throw new Exception("Unable to construct Example from " + data);
            }
        }

        public static DataSet getRestaurantDataSet()
        {
            DataSetSpecification spec = createRestaurantDataSetSpec();
            return DataSetFactory.FromFile("data\\restaurant.csv", spec, "\\s+");
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
            // last attribute is the target attribute unless the target is
            // explicitly reset with dss.setTarget(name)

            return dss;
        }

        public static DataSet getIrisDataSet()
        {
            DataSetSpecification spec = createIrisDataSetSpec();
            return DataSetFactory.FromFile("iris", spec, ",");
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