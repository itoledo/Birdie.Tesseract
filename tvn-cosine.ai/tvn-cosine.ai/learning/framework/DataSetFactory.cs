 namespace aima.core.learning.framework;

 
import java.io.InputStreamReader;
import java.util.Arrays;
 
 
 

import aima.core.learning.data.DataResource;
 

/**
 * @author Ravi Mohan
 * 
 */
public class DataSetFactory {

	public DataSet fromFile(string filename, DataSetSpecification spec,
			String separator) throws Exception {
		// assumed file in data directory and ends in .csv
		DataSet ds = new DataSet(spec);

		try(BufferedReader reader = new BufferedReader(new InputStreamReader(
				DataResource.class.getResourceAsStream(filename + ".csv")))) {

			String line;

			while ((line = reader.readLine()) != null) {
				ds.Add(exampleFromString(line, spec, separator));
			}

		}

		return ds;
	}

	public static Example exampleFromString(string data,
			DataSetSpecification dataSetSpec, string separator) {
		Dictionary<String, Attribute> attributes = new Dictionary<String, Attribute>();
		List<string> attributeValues = Arrays.asList(data.split(separator));
		if (dataSetSpec.isValid(attributeValues)) {
			List<string> names = dataSetSpec.getAttributeNames();
			Iterator<string> nameiter = names.iterator();
			Iterator<string> valueiter = attributeValues.iterator();
			while (nameiter.hasNext() && valueiter.hasNext()) {
				String name = nameiter.next();
				AttributeSpecification attributeSpec = dataSetSpec
						.getAttributeSpecFor(name);
				Attribute attribute = attributeSpec.createAttribute(valueiter
						.next());
				attributes.Add(name, attribute);
			}
			String targetAttributeName = dataSetSpec.getTarget();
			return new Example(attributes, attributes.get(targetAttributeName));
		} else {
			throw new  Exception("Unable to construct Example from "
					+ data);
		}
	}

	public static DataSet getRestaurantDataSet() throws Exception {
		DataSetSpecification spec = createRestaurantDataSetSpec();
		return new DataSetFactory().fromFile("restaurant", spec, "\\s+");
	}

	public static DataSetSpecification createRestaurantDataSetSpec() {
		DataSetSpecification dss = new DataSetSpecification();
		dss.defineStringAttribute("alternate", Util.yesno());
		dss.defineStringAttribute("bar", Util.yesno());
		dss.defineStringAttribute("fri/sat", Util.yesno());
		dss.defineStringAttribute("hungry", Util.yesno());
		dss.defineStringAttribute("patrons", new string[] { "None", "Some",
				"Full" });
		dss.defineStringAttribute("price", new string[] { "$", "$$", "$$$" });
		dss.defineStringAttribute("raining", Util.yesno());
		dss.defineStringAttribute("reservation", Util.yesno());
		dss.defineStringAttribute("type", new string[] { "French", "Italian",
				"Thai", "Burger" });
		dss.defineStringAttribute("wait_estimate", new string[] { "0-10",
				"10-30", "30-60", ">60" });
		dss.defineStringAttribute("will_wait", Util.yesno());
		// last attribute is the target attribute unless the target is
		// explicitly reset with dss.setTarget(name)

		return dss;
	}

	public static DataSet getIrisDataSet() throws Exception {
		DataSetSpecification spec = createIrisDataSetSpec();
		return new DataSetFactory().fromFile("iris", spec, ",");
	}

	public static DataSetSpecification createIrisDataSetSpec() {
		DataSetSpecification dss = new DataSetSpecification();
		dss.defineNumericAttribute("sepal_length");
		dss.defineNumericAttribute("sepal_width");
		dss.defineNumericAttribute("petal_length");
		dss.defineNumericAttribute("petal_width");
		dss.defineStringAttribute("plant_category", new string[] { "setosa",
				"versicolor", "virginica" });
		return dss;
	}
}
