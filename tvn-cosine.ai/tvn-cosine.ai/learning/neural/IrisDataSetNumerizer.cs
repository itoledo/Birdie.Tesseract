using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;

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

            input.add(sepal_length);
            input.add(sepal_width);
            input.add(petal_length);
            input.add(petal_width);

            String plant_category_string = e
                    .getAttributeValueAsString("plant_category");

            desiredOutput = convertCategoryToListOfDoubles(plant_category_string);

            Pair<List<double>, List<double>> io = new Pair<List<double>, List<double>>(
                    input, desiredOutput);

            return io;
        }

        public String denumerize(List<double> outputValue)
        {
            List<double> rounded = new ArrayList<double>();
            for (Double d : outputValue)
            {
                rounded.add(round(d));
            }
            if (rounded.equals(Arrays.asList(0.0, 0.0, 1.0)))
            {
                return "setosa";
            }
            else if (rounded.equals(Arrays.asList(0.0, 1.0, 0.0)))
            {
                return "versicolor";
            }
            else if (rounded.equals(Arrays.asList(1.0, 0.0, 0.0)))
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
        private double round(Double d)
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
                return Math.round(d);
            }
        }

        private List<double> convertCategoryToListOfDoubles(
                String plant_category_string)
        {
            if (plant_category_string.equals("setosa"))
            {
                return Arrays.asList(0.0, 0.0, 1.0);
            }
            else if (plant_category_string.equals("versicolor"))
            {
                return Arrays.asList(0.0, 1.0, 0.0);
            }
            else if (plant_category_string.equals("virginica"))
            {
                return Arrays.asList(1.0, 0.0, 0.0);
            }
            else
            {
                throw new RuntimeException("invalid plant category");
            }
        }
    }

}
