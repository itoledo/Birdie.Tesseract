using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
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
        private ICollection<NNExample> dataset;
        /*
         * a copy from which examples are drawn.
         */
        private ICollection<NNExample> presentlyProcessed = CollectionFactory.CreateQueue<NNExample>();

        /*
         * list of mean Values for all components of raw data set
         */
        private ICollection<double> means;

        /*
         * list of stdev Values for all components of raw data set
         */
        private ICollection<double> stdevs;
        /*
         * the normalized data set
         */
        protected ICollection<ICollection<double>> nds;

        /*
         * the column numbers of the "target"
         */

        protected ICollection<int> targetColumnNumbers;

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

            ICollection<ICollection<double>> rds = CollectionFactory.CreateQueue<ICollection<double>>();

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

            ICollection<ICollection<double>> rds = rawExamplesFromDataSet(ds, numerizer);
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
            presentlyProcessed = CollectionFactory.CreateQueue<NNExample>();
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

        public ICollection<ICollection<double>> getNormalizedData()
        {
            return nds;
        }

        public ICollection<double> getMeans()
        {
            return means;
        }

        public ICollection<double> getStdevs()
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
            dataset = CollectionFactory.CreateQueue<NNExample>();
            foreach (ICollection<double> dataLine in nds)
            {
                ICollection<double> input = CollectionFactory.CreateQueue<double>();
                ICollection<double> target = CollectionFactory.CreateQueue<double>();
                for (int i = 0; i < dataLine.Size();++i)
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

        private ICollection<ICollection<double>> normalize(ICollection<ICollection<double>> rds)
        {
            int rawDataLength = rds.Get(0).Size();
            ICollection<ICollection<double>> nds = CollectionFactory.CreateQueue<ICollection<double>>();

            means = CollectionFactory.CreateQueue<double>();
            stdevs = CollectionFactory.CreateQueue<double>();

            ICollection<ICollection<double>> normalizedColumns = CollectionFactory.CreateQueue<ICollection<double>>();
            // clculate means for each coponent of example data
            for (int i = 0; i < rawDataLength;++i)
            {
                ICollection<double> columnValues = CollectionFactory.CreateQueue<double>();
                foreach (ICollection<double> rawDatum in rds)
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
            for (int i = 0; i < columnLength;++i)
            {
                ICollection<double> lst = CollectionFactory.CreateQueue<double>();
                for (int j = 0; j < numberOfColumns; j++)
                {
                    lst.Add(normalizedColumns.Get(j).Get(i));
                }
                nds.Add(lst);
            }
            return nds;
        }

        private ICollection<double> exampleFromString(string line, string separator)
        {
            // assumes all values for inout and target are doubles
            ICollection<double> rexample = CollectionFactory.CreateQueue<double>();
            ICollection<string> attributeValues = CollectionFactory.CreateQueue<string>(Regex.Split(line, separator));
            foreach (string valString in attributeValues)
            {
                rexample.Add(double.Parse(valString, NumberStyles.Any, CultureInfo.InvariantCulture));
            }
            return rexample;
        }

        private ICollection<ICollection<double>> rawExamplesFromDataSet(DataSet ds, Numerizer numerizer)
        {
            // assumes all values for inout and target are doubles
            ICollection<ICollection<double>> rds = CollectionFactory.CreateQueue<ICollection<double>>();
            for (int i = 0; i < ds.size();++i)
            {
                ICollection<double> rexample = CollectionFactory.CreateQueue<double>();
                Example e = ds.getExample(i);
                Pair<ICollection<double>, ICollection<double>> p = numerizer.numerize(e);
                ICollection<double> attributes = p.getFirst();
                foreach (double d in attributes)
                {
                    rexample.Add(d);
                }
                ICollection<double> targets = p.getSecond();
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
