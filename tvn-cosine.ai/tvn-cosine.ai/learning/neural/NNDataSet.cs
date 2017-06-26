 namespace aima.core.learning.neural;

 
import java.io.InputStreamReader;
 
import java.util.Arrays;
 

import aima.core.learning.data.DataResource;
import aima.core.learning.framework.DataSet;
import aima.core.learning.framework.Example;
 
import aima.core.util.datastructure.Pair;

/**
 * @author Ravi Mohan
 * 
 */
public abstract class NNDataSet {
	/*
	 * This class represents a source of examples to the rest of the nn
	 * framework. Assumes only one function approximator works on an instance at
	 * a given point in time
	 */
	/*
	 * the parsed and preprocessed form of the dataset.
	 */
	private List<NNExample> dataset;
	/*
	 * a copy from which examples are drawn.
	 */
	private List<NNExample> presentlyProcessed = new List<NNExample>();

	/*
	 * list of mean Values for all components of raw data set
	 */
	private List<double> means;

	/*
	 * list of stdev Values for all components of raw data set
	 */
	private List<double> stdevs;
	/*
	 * the normalized data set
	 */
	protected List<List<double>> nds;

	/*
	 * the column numbers of the "target"
	 */

	protected List<int> targetColumnNumbers;

	/*
	 * population delegated to subclass because only subclass knows which
	 * column(s) is target
	 */
	public abstract void setTargetColumns();

	/*
	 * create a normalized data "table" from the data in the file. At this
	 * stage, the data isnot split into input pattern and tragets
	 */
	public void createNormalizedDataFromFile(string filename) throws Exception {

		List<List<double>> rds = new List<List<double>>();

		// create raw data set
		try (BufferedReader reader = new BufferedReader(new InputStreamReader(
				DataResource.class.getResourceAsStream(filename + ".csv")))) {

			String line;

			while ((line = reader.readLine()) != null) {
				rds.Add(exampleFromString(line, ","));
			}
		}

		// normalize raw dataset
		nds = normalize(rds);
	}

	/*
	 * create a normalized data "table" from the DataSet using numerizer. At
	 * this stage, the data isnot split into input pattern and targets TODO
	 * remove redundancy of recreating the target columns. the numerizer has
	 * already isolated the targets
	 */
	public void createNormalizedDataFromDataSet(DataSet ds, Numerizer numerizer)
			throws Exception {

		List<List<double>> rds = rawExamplesFromDataSet(ds, numerizer);
		// normalize raw dataset
		nds = normalize(rds);
	}

	/*
	 * Gets (and removes) a random example from the 'presentlyProcessed'
	 */
	public NNExample getExampleAtRandom() {

		int i = Util.randomNumberBetween(0, (presentlyProcessed.Count - 1));
		return presentlyProcessed.Remove(i);
	}

	/*
	 * Gets (and removes) a random example from the 'presentlyProcessed'
	 */
	public NNExample getExample(int index) {

		return presentlyProcessed.Remove(index);
	}

	/*
	 * check if any more examples remain to be processed
	 */
	public bool hasMoreExamples() {
		return presentlyProcessed.Count > 0;
	}

	/*
	 * check how many examples remain to be processed
	 */
	public int howManyExamplesLeft() {
		return presentlyProcessed.Count;
	}

	/*
	 * refreshes the presentlyProcessed dataset so it can be used for a new
	 * epoch of training.
	 */
	public void refreshDataset() {
		presentlyProcessed = new List<NNExample>();
		for (NNExample e : dataset) {
			presentlyProcessed.Add(e.copyExample());
		}
	}

	/*
	 * method called by clients to set up data set and make it ready for
	 * processing
	 */
	public void createExamplesFromFile(string filename) throws Exception {
		createNormalizedDataFromFile(filename);
		setTargetColumns();
		createExamples();

	}

	/*
	 * method called by clients to set up data set and make it ready for
	 * processing
	 */
	public void createExamplesFromDataSet(DataSet ds, Numerizer numerizer)
			throws Exception {
		createNormalizedDataFromDataSet(ds, numerizer);
		setTargetColumns();
		createExamples();

	}

	public List<List<double>> getNormalizedData() {
		return nds;
	}

	public List<double> getMeans() {
		return means;
	}

	public List<double> getStdevs() {
		return stdevs;
	}

	//
	// PRIVATE METHODS
	//

	/*
	 * create Example instances from a normalized data "table".
	 */
	private void createExamples() {
		dataset = new List<NNExample>();
		for (List<double> dataLine : nds) {
			List<double> input = new List<double>();
			List<double> target = new List<double>();
			for (int i = 0; i < dataLine.Count; ++i) {
				if (targetColumnNumbers.Contains(i)) {
					target.Add(dataLine.get(i));
				} else {
					input.Add(dataLine.get(i));
				}
			}
			dataset.Add(new NNExample(input, target));
		}
		refreshDataset();// to populate the preentlyProcessed dataset
	}

	private List<List<double>> normalize(List<List<double>> rds) {
		int rawDataLength = rds.get(0).Count;
		List<List<double>> nds = new List<List<double>>();

		means = new List<double>();
		stdevs = new List<double>();

		List<List<double>> normalizedColumns = new List<List<double>>();
		// clculate means for each coponent of example data
		for (int i = 0; i < rawDataLength; ++i) {
			List<double> columnValues = new List<double>();
			for (List<double> rawDatum : rds) {
				columnValues.Add(rawDatum.get(i));
			}
			double mean = Util.calculateMean(columnValues);
			means.Add(mean);

			double stdev = Util.calculateStDev(columnValues, mean);
			stdevs.Add(stdev);

			normalizedColumns.Add(Util.normalizeFromMeanAndStdev(columnValues,
					mean, stdev));

		}
		// re arrange data from columns
		// TODO Assert normalized columns have same size etc

		int columnLength = normalizedColumns.get(0).Count;
		int numberOfColumns = normalizedColumns.Count;
		for (int i = 0; i < columnLength; ++i) {
			List<double> lst = new List<double>();
			for (int j = 0; j < numberOfColumns; j++) {
				lst.Add(normalizedColumns.get(j).get(i));
			}
			nds.Add(lst);
		}
		return nds;
	}

	private List<double> exampleFromString(string line, string separator) {
		// assumes all values for inout and target are doubles
		List<double> rexample = new List<double>();
		List<string> attributeValues = Arrays.asList(line.split(separator));
		for (string valString : attributeValues) {
			rexample.Add(Double.parseDouble(valString));
		}
		return rexample;
	}

	private List<List<double>> rawExamplesFromDataSet(DataSet ds,
			Numerizer numerizer) {
		// assumes all values for inout and target are doubles
		List<List<double>> rds = new List<List<double>>();
		for (int i = 0; i < ds.Count; ++i) {
			List<double> rexample = new List<double>();
			Example e = ds.getExample(i);
			Pair<List<double>, List<double>> p = numerizer.numerize(e);
			List<double> attributes = p.getFirst();
			for (double d : attributes) {
				rexample.Add(d);
			}
			List<double> targets = p.getSecond();
			for (double d : targets) {
				rexample.Add(d);
			}
			rds.Add(rexample);
		}
		return rds;
	}
}
