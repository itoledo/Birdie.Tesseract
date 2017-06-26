 namespace aima.core.learning.neural;

 
import java.util.Arrays;
 

import aima.core.learning.framework.Example;
import aima.core.util.datastructure.Pair;

/**
 * @author Ravi Mohan
 * 
 */
public class IrisDataSetNumerizer : Numerizer {

	public Pair<List<double>, List<double>> numerize(Example e) {
		List<double> input = new List<double>();
		List<double> desiredOutput = new List<double>();

		double sepal_length = e.getAttributeValueAsDouble("sepal_length");
		double sepal_width = e.getAttributeValueAsDouble("sepal_width");
		double petal_length = e.getAttributeValueAsDouble("petal_length");
		double petal_width = e.getAttributeValueAsDouble("petal_width");

		input.Add(sepal_length);
		input.Add(sepal_width);
		input.Add(petal_length);
		input.Add(petal_width);

		String plant_category_string = e
				.getAttributeValueAsString("plant_category");

		desiredOutput = convertCategoryToListOfDoubles(plant_category_string);

		Pair<List<double>, List<double>> io = new Pair<List<double>, List<double>>(
				input, desiredOutput);

		return io;
	}

	public string denumerize(List<double> outputValue) {
		List<double> rounded = new List<double>();
		for (double d : outputValue) {
			rounded.Add(round(d));
		}
		if (rounded .Equals(Arrays.asList(0.0, 0.0, 1.0))) {
			return "setosa";
		} else if (rounded .Equals(Arrays.asList(0.0, 1.0, 0.0))) {
			return "versicolor";
		} else if (rounded .Equals(Arrays.asList(1.0, 0.0, 0.0))) {
			return "virginica";
		} else {
			return "unknown";
		}
	}

	//
	// PRIVATE METHODS
	//
	private double round(double d) {
		if (d < 0) {
			return 0.0;
		}
		if (d > 1) {
			return 1.0;
		} else {
			return Math.round(d);
		}
	}

	private List<double> convertCategoryToListOfDoubles(
			String plant_category_string) {
		if (plant_category_string .Equals("setosa")) {
			return Arrays.asList(0.0, 0.0, 1.0);
		} else if (plant_category_string .Equals("versicolor")) {
			return Arrays.asList(0.0, 1.0, 0.0);
		} else if (plant_category_string .Equals("virginica")) {
			return Arrays.asList(1.0, 0.0, 0.0);
		} else {
			throw new  Exception("invalid plant category");
		}
	}
}
