using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.learning.neural
{
    /**
     * @author Ravi Mohan
     * 
     */
    public abstract class NNDataSet
    {
        /*
         * This class represents a source of examples to the rest of the nn
         * framework. Assumes only one function approximator works on an instance at
         * a given point in time
         */
        /*
         * the parsed and preprocessed form of the dataset.
         */
        private IList<NNExample> dataset;
        /*
         * a copy from which examples are drawn.
         */
        private IList<NNExample> presentlyProcessed = new List<NNExample>();

        /*
         * list of mean Values for all components of raw data set
         */
        private IList<double> means;

        /*
         * list of stdev Values for all components of raw data set
         */
        private IList<double> stdevs;
        /*
         * the normalized data set
         */
        protected IList<IList<double>> nds;

        /*
         * the column numbers of the "target"
         */

        protected IList<int> targetColumnNumbers;

        /*
         * population delegated to subclass because only subclass knows which
         * column(s) is target
         */
        public abstract void setTargetColumns();

        /*
         * create a normalized data "table" from the data in the file. At this
         * stage, the data isnot split into input pattern and tragets
         */
        public void createNormalizedDataFromFile(string filename)
        {
            IList<IList<double>> rds = new List<IList<double>>();

            // create raw data set
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
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
        {
            IList<IList<double>> rds = rawExamplesFromDataSet(ds, numerizer);
            // normalize raw dataset
            nds = normalize(rds);
        }

        /*
         * Gets (and removes) a random example from the 'presentlyProcessed'
         */
        public NNExample getExampleAtRandom()
        {
            int i = Util.randomNumberBetween(0, (presentlyProcessed.Count - 1));
            var obj = presentlyProcessed[i];
            presentlyProcessed.RemoveAt(i);
            return obj;
        }

        /*
         * Gets (and removes) a random example from the 'presentlyProcessed'
         */
        public NNExample getExample(int index)
        {
            NNExample obj = presentlyProcessed[index];
            presentlyProcessed.RemoveAt(index);
            return obj;
        }

        /*
         * check if any more examples remain to be processed
         */
        public bool hasMoreExamples()
        {
            return presentlyProcessed.Count > 0;
        }

        /*
         * check how many examples remain to be processed
         */
        public int howManyExamplesLeft()
        {
            return presentlyProcessed.Count;
        }

        /*
         * refreshes the presentlyProcessed dataset so it can be used for a new
         * epoch of training.
         */
        public void refreshDataset()
        {
            presentlyProcessed = new List<NNExample>();
            foreach (NNExample e in dataset)
            {
                presentlyProcessed.Add(e.copyExample());
            }
        }

        /*
         * method called by clients to set up data set and make it ready for
         * processing
         */
        public void createExamplesFromFile(string filename)
        {
            createNormalizedDataFromFile(filename);
            setTargetColumns();
            createExamples();

        }

        /*
         * method called by clients to set up data set and make it ready for
         * processing
         */
        public void createExamplesFromDataSet(DataSet ds, Numerizer numerizer)
        {
            createNormalizedDataFromDataSet(ds, numerizer);
            setTargetColumns();
            createExamples();

        }

        public IList<IList<double>> getNormalizedData()
        {
            return nds;
        }

        public IList<double> getMeans()
        {
            return means;
        }

        public IList<double> getStdevs()
        {
            return stdevs;
        }

        //
        // PRIVATE METHODS
        //

        /*
         * create Example instances from a normalized data "table".
         */
        private void createExamples()
        {
            dataset = new List<NNExample>();
            foreach (IList<double> dataLine in nds)
            {
                IList<double> input = new List<double>();
                IList<double> target = new List<double>();
                for (int i = 0; i < dataLine.Count; i++)
                {
                    if (targetColumnNumbers.Contains(i))
                    {
                        target.Add(dataLine[i]);
                    }
                    else
                    {
                        input.Add(dataLine[i]);
                    }
                }
                dataset.Add(new NNExample(input, target));
            }
            refreshDataset();// to populate the preentlyProcessed dataset
        }

        private IList<IList<double>> normalize(IList<IList<double>> rds)
        {
            int rawDataLength = rds[0].Count;
            IList<IList<double>> nds = new List<IList<double>>();

            means = new List<double>();
            stdevs = new List<double>();

            IList<IList<double>> normalizedColumns = new List<IList<double>>();
            // clculate means for each coponent of example data
            for (int i = 0; i < rawDataLength; i++)
            {
                IList<double> columnValues = new List<double>();
                foreach (IList<double> rawDatum in rds)
                {
                    columnValues.Add(rawDatum[i]);
                }
                double mean = Util.calculateMean(columnValues);
                means.Add(mean);

                double stdev = Util.calculateStDev(columnValues, mean);
                stdevs.Add(stdev);

                normalizedColumns.Add(Util.normalizeFromMeanAndStdev(columnValues, mean, stdev));

            }
            // re arrange data from columns
            // TODO Assert normalized columns have same size etc

            int columnLength = normalizedColumns[0].Count;
            int numberOfColumns = normalizedColumns.Count;
            for (int i = 0; i < columnLength; i++)
            {
                IList<double> lst = new List<double>();
                for (int j = 0; j < numberOfColumns; j++)
                {
                    lst.Add(normalizedColumns[j][i]);
                }
                nds.Add(lst);
            }
            return nds;
        }

        private IList<double> exampleFromString(string line, string separator)
        {
            // assumes all values for inout and target are doubles
            IList<double> rexample = new List<double>();

            IList<string> attributeValues = new Regex(separator).Split(line).ToList();
            foreach (string valString in attributeValues)
            {
                rexample.Add(double.Parse(valString));
            }
            return rexample;
        }

        private IList<IList<double>> rawExamplesFromDataSet(DataSet ds, Numerizer numerizer)
        {
            // assumes all values for inout and target are doubles
            IList<IList<double>> rds = new List<IList<double>>();
            for (int i = 0; i < ds.size(); i++)
            {
                IList<double> rexample = new List<double>();
                Example e = ds.getExample(i);
                Pair<IList<double>, IList<double>> p = numerizer.numerize(e);
                IList<double> attributes = p.First;
                foreach (double d in attributes)
                {
                    rexample.Add(d);
                }
                IList<double> targets = p.Second;
                foreach (double d in targets)
                {
                    rexample.Add(d);
                }
                rds.Add(rexample);
            }
            return rds;
        }
    }
}
