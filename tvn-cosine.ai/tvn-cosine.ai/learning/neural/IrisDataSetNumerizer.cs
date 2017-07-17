using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.neural
{
    public class IrisDataSetNumerizer : Numerizer
    {
        public Pair<IQueue<double>, IQueue<double>> numerize(Example e)
        {
            IQueue<double> input = Factory.CreateQueue<double>();
            IQueue<double> desiredOutput = Factory.CreateQueue<double>();

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

            Pair<IQueue<double>, IQueue<double>> io = new Pair<IQueue<double>, IQueue<double>>(input, desiredOutput);

            return io;
        }

        public string denumerize(IQueue<double> outputValue)
        {
            IQueue<double> rounded = Factory.CreateQueue<double>();
            foreach (double d in outputValue)
            {
                rounded.Add(round(d));
            }
            if (rounded.Equals(Factory.CreateQueue<double>(new[] { 0.0, 0.0, 1.0 })))
            {
                return "setosa";
            }
            else if (rounded.Equals(Factory.CreateQueue<double>(new[] { 0.0, 1.0, 0.0 })))
            {
                return "versicolor";
            }
            else if (rounded.Equals(Factory.CreateQueue<double>(new[] { 1.0, 0.0, 0.0 })))
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
                return System.Math.Round(d);
            }
        }

        private IQueue<double> convertCategoryToListOfDoubles(string plant_category_string)
        {
            if (plant_category_string.Equals("setosa"))
            {
                return Factory.CreateQueue<double>(new[] { 0.0, 0.0, 1.0 });
            }
            else if (plant_category_string.Equals("versicolor"))
            {
                return Factory.CreateQueue<double>(new[] { 0.0, 1.0, 0.0 });
            }
            else if (plant_category_string.Equals("virginica"))
            {
                return Factory.CreateQueue<double>(new[] { 1.0, 0.0, 0.0 });
            }
            else
            {
                throw new RuntimeException("invalid plant category");
            }
        }
    }

}
