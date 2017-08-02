using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.exceptions;
using tvn.cosine.text;
using tvn.cosine.text.api;
using tvn.cosine.ai.learning.framework.api;
using tvn.cosine.ai.util;
using tvn.cosine.ai.learning.learners.svm;

namespace tvn.cosine.ai.learning.framework
{
    public class DataSetFactory
    {
        public static svm_problem LoadSVMFile(string filename, svm_parameter param)
        {
            System.IO.StreamReader fp = new System.IO.StreamReader(filename);
            List<double> vy = new List<double>();
            List<svm_node[]> vx = new List<svm_node[]>();
            int max_index = 0;

            while (true)
            {
                string line = fp.ReadLine();
                if (line == null) break;

                var st = line.Split(new[] { ' ', '\t', '\n', '\r', '\f', ':' });
                int counter = 0;

                vy.Add(TextFactory.ParseDouble(st[counter++]));
                int m = (st.Length - 1) / 2;
                svm_node[] x = new svm_node[m];
                for (int j = 0; j < m; j++)
                {
                    x[j] = new svm_node();
                    x[j].index = TextFactory.ParseInt(st[counter++]);
                    x[j].value = TextFactory.ParseDouble(st[counter++]);
                }
                if (m > 0) max_index = System.Math.Max(max_index, x[m - 1].index);
                vx.Add(x);
            }

            svm_problem prob = new svm_problem(); 
            prob.l = vy.Size();
            prob.x = new svm_node[prob.l][];
            for (int i = 0; i < prob.l; i++)
                prob.x[i] = vx.Get(i);
            prob.y = new double[prob.l];
            for (int i = 0; i < prob.l; i++)
                prob.y[i] = vy.Get(i);

            if (param.gamma == 0 && max_index > 0)
                param.gamma = 1.0 / max_index;

            if (param.kernel_type == svm_parameter.PRECOMPUTED)
                for (int i = 0; i < prob.l; i++)
                {
                    if (prob.x[i][0].index != 0)
                    {
                        throw new Exception("Wrong kernel matrix: first column must be 0:sample_serial_number\n");
                    }
                    if ((int)prob.x[i][0].value <= 0 || (int)prob.x[i][0].value > max_index)
                    {
                        throw new Exception("Wrong input format: sample_serial_number out of range\n");
                    }
                }

            fp.Close();
            return prob;
        }


        public DataSet fromFile(string filename, DataSetSpecification spec, string separator)
        {
            // assumed file in data directory and ends in .csv
            DataSet ds = new DataSet(spec);

            if (!System.IO.File.Exists(filename + ".csv"))
            {
                throw new FileNotFoundException(filename + ".csv" + " does not exist.");
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
            IRegularExpression splitter = TextFactory.CreateRegularExpression(separator);
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
            dss.defineStringAttribute("alternate", Util.YesNo());
            dss.defineStringAttribute("bar", Util.YesNo());
            dss.defineStringAttribute("fri/sat", Util.YesNo());
            dss.defineStringAttribute("hungry", Util.YesNo());
            dss.defineStringAttribute("patrons", new string[] { "None", "Some", "Full" });
            dss.defineStringAttribute("price", new string[] { "$", "$$", "$$$" });
            dss.defineStringAttribute("raining", Util.YesNo());
            dss.defineStringAttribute("reservation", Util.YesNo());
            dss.defineStringAttribute("type", new string[] { "French", "Italian", "Thai", "Burger" });
            dss.defineStringAttribute("wait_estimate", new string[] { "0-10", "10-30", "30-60", ">60" });
            dss.defineStringAttribute("will_wait", Util.YesNo());
            // last attribute is the target attribute unless the target is explicitly reset with dss.setTarget(name)

            return dss;
        }

        public static DataSet getIrisDataSet()
        {
            DataSetSpecification spec = createIrisDataSetSpec();
            return new DataSetFactory().fromFile("iris", spec, ",");
        }

        public static DataSetSpecification createCSVDataSetSpec()
        {
            DataSetSpecification dss = new DataSetSpecification();

            for (int i = 0; i < 13; ++i)
            {
                dss.defineNumericAttribute(i.ToString());
            }

            return dss;
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
