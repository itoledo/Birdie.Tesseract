using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.neural
{
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
        private IQueue<NNExample> dataset;
        /*
         * a copy from which examples are drawn.
         */
        private IQueue<NNExample> presentlyProcessed = Factory.CreateQueue<NNExample>();

        /*
         * list of mean Values for all components of raw data set
         */
        private IQueue<double> means;

        /*
         * list of stdev Values for all components of raw data set
         */
        private IQueue<double> stdevs;
        /*
         * the normalized data set
         */
        protected IQueue<IQueue<double>> nds;

        /*
         * the column numbers of the "target"
         */

        protected IQueue<int> targetColumnNumbers;

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

            IQueue<IQueue<double>> rds = Factory.CreateQueue<IQueue<double>>();

            // create raw data set
            using (StreamReader reader = new StreamReader(filename + ".csv"))
            {

                string line = string.Empty;

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

            IQueue<IQueue<double>> rds = rawExamplesFromDataSet(ds, numerizer);
            // normalize raw dataset
            nds = normalize(rds);
        }

        /*
         * Gets (and removes) a random example from the 'presentlyProcessed'
         */
        public NNExample getExampleAtRandom()
        {
            int i = Util.randomNumberBetween(0, (presentlyProcessed.Size() - 1));
            return getExample(i);
        }

        /*
         * Gets (and removes) a random example from the 'presentlyProcessed'
         */
        public NNExample getExample(int index)
        {
            NNExample obj = presentlyProcessed.Get(index);
            presentlyProcessed.Remove(obj);
            return obj;
        }

        /*
         * check if any more examples remain to be processed
         */
        public bool hasMoreExamples()
        {
            return presentlyProcessed.Size() > 0;
        }

        /*
         * check how many examples remain to be processed
         */
        public int howManyExamplesLeft()
        {
            return presentlyProcessed.Size();
        }

        /*
         * refreshes the presentlyProcessed dataset so it can be used for a new
         * epoch of training.
         */
        public void refreshDataset()
        {
            presentlyProcessed = Factory.CreateQueue<NNExample>();
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

        public IQueue<IQueue<double>> getNormalizedData()
        {
            return nds;
        }

        public IQueue<double> getMeans()
        {
            return means;
        }

        public IQueue<double> getStdevs()
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
            dataset = Factory.CreateQueue<NNExample>();
            foreach (IQueue<double> dataLine in nds)
            {
                IQueue<double> input = Factory.CreateQueue<double>();
                IQueue<double> target = Factory.CreateQueue<double>();
                for (int i = 0; i < dataLine.Size(); i++)
                {
                    if (targetColumnNumbers.Contains(i))
                    {
                        target.Add(dataLine.Get(i));
                    }
                    else
                    {
                        input.Add(dataLine.Get(i));
                    }
                }
                dataset.Add(new NNExample(input, target));
            }
            refreshDataset();// to populate the preentlyProcessed dataset
        }

        private IQueue<IQueue<double>> normalize(IQueue<IQueue<double>> rds)
        {
            int rawDataLength = rds.Get(0).Size();
            IQueue<IQueue<double>> nds = Factory.CreateQueue<IQueue<double>>();

            means = Factory.CreateQueue<double>();
            stdevs = Factory.CreateQueue<double>();

            IQueue<IQueue<double>> normalizedColumns = Factory.CreateQueue<IQueue<double>>();
            // clculate means for each coponent of example data
            for (int i = 0; i < rawDataLength; i++)
            {
                IQueue<double> columnValues = Factory.CreateQueue<double>();
                foreach (IQueue<double> rawDatum in rds)
                {
                    columnValues.Add(rawDatum.Get(i));
                }
                double mean = Util.calculateMean(columnValues);
                means.Add(mean);

                double stdev = Util.calculateStDev(columnValues, mean);
                stdevs.Add(stdev);

                normalizedColumns.Add(Util.normalizeFromMeanAndStdev(columnValues, mean, stdev));

            }
            // re arrange data from columns
            // TODO Assert normalized columns have same size etc

            int columnLength = normalizedColumns.Get(0).Size();
            int numberOfColumns = normalizedColumns.Size();
            for (int i = 0; i < columnLength; i++)
            {
                IQueue<double> lst = Factory.CreateQueue<double>();
                for (int j = 0; j < numberOfColumns; j++)
                {
                    lst.Add(normalizedColumns.Get(j).Get(i));
                }
                nds.Add(lst);
            }
            return nds;
        }

        private IQueue<double> exampleFromString(string line, string separator)
        {
            // assumes all values for inout and target are doubles
            IQueue<double> rexample = Factory.CreateQueue<double>();
            IQueue<string> attributeValues = Factory.CreateQueue<string>(Regex.Split(line, separator));
            foreach (string valString in attributeValues)
            {
                rexample.Add(double.Parse(valString, NumberStyles.Any, CultureInfo.InvariantCulture));
            }
            return rexample;
        }

        private IQueue<IQueue<double>> rawExamplesFromDataSet(DataSet ds, Numerizer numerizer)
        {
            // assumes all values for inout and target are doubles
            IQueue<IQueue<double>> rds = Factory.CreateQueue<IQueue<double>>();
            for (int i = 0; i < ds.size(); i++)
            {
                IQueue<double> rexample = Factory.CreateQueue<double>();
                Example e = ds.getExample(i);
                Pair<IQueue<double>, IQueue<double>> p = numerizer.numerize(e);
                IQueue<double> attributes = p.getFirst();
                foreach (double d in attributes)
                {
                    rexample.Add(d);
                }
                IQueue<double> targets = p.getSecond();
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
