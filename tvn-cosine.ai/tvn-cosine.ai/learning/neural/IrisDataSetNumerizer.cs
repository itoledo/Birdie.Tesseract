using System;
using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.learning.neural
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class IrisDataSetNumerizer : Numerizer
    {
        public Pair<IList<double>, IList<double>> numerize(Example e)
        {
            IList<double> input = new List<double>();
            IList<double> desiredOutput = new List<double>();

            double sepal_length = e.getAttributeValueAsDouble("sepal_length");
            double sepal_width = e.getAttributeValueAsDouble("sepal_width");
            double petal_length = e.getAttributeValueAsDouble("petal_length");
            double petal_width = e.getAttributeValueAsDouble("petal_width");

            input.Add(sepal_length);
            input.Add(sepal_width);
            input.Add(petal_length);
            input.Add(petal_width);

            string plant_category_string = e.getAttributeValueAsString("plant_category");

            desiredOutput = convertCategoryToListOfDoubles(plant_category_string);

            Pair<IList<double>, IList<double>> io = new Pair<IList<double>, IList<double>>(input, desiredOutput);

            return io;
        }
         
        public string denumerize(IList<double> outputValue)
        {
            IList<double> rounded = new List<double>();
            foreach (double d in outputValue)
            {
                rounded.Add(round(d));
            }
            if (Util.ValuesEquals(rounded, new List<double>(new double[] { 0.0, 0.0, 1.0 })))
            {
                return "setosa";
            }
            else if (Util.ValuesEquals(rounded, new List<double>(new double[] { 0.0, 1.0, 0.0 })))
            {
                return "versicolor";
            }
            else if (Util.ValuesEquals(rounded, new List<double>(new double[] { 1.0, 0.0, 0.0 })))
            {
                return "virginica";
            }
            else
            {
                return "unknown";
            }
        }

        //
        // PRIVATE METHODS
        //
        private double round(double d)
        {
            if (d < 0)
            {
                return 0.0;
            }
            if (d > 1)
            {
                return 1.0;
            }
            else
            {
                return Math.Round(d);
            }
        }

        private IList<double> convertCategoryToListOfDoubles(string plant_category_string)
        {
            if (plant_category_string.Equals("setosa"))
            {
                return new List<double>(new double[] { 0.0, 0.0, 1.0 });
            }
            else if (plant_category_string.Equals("versicolor"))
            {
                return new List<double>(new double[] { 0.0, 1.0, 0.0 });
            }
            else if (plant_category_string.Equals("virginica"))
            {
                return new List<double>(new double[] { 1.0, 0.0, 0.0 });
            }
            else
            {
                throw new Exception("invalid plant category");
            }
        }
    }
}
