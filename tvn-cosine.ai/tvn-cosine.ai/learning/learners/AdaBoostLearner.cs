using System;
using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.learning.learners
{ 
    public class AdaBoostLearner : Learner
    {
        private IList<Learner> learners;

        private DataSet dataSet;

        private double[] exampleWeights;

        private IDictionary<Learner, double> learnerWeights;

        public AdaBoostLearner(IList<Learner> learners, DataSet ds)
        {
            this.learners = learners;
            this.dataSet = ds;

            initializeExampleWeights(ds.examples.Count);
            initializeHypothesisWeights(learners.Count);
        }

        public void train(DataSet ds)
        {
            initializeExampleWeights(ds.examples.Count);

            foreach (Learner learner in learners)
            {
                learner.train(ds);

                double error = calculateError(ds, learner);
                if (error < 0.0001)
                {
                    break;
                }

                adjustExampleWeights(ds, learner, error);

                double newHypothesisWeight = learnerWeights[learner] * Math.Log((1.0 - error) / error);
                learnerWeights[learner] = newHypothesisWeight;
            }
        }

        public string predict(Example e)
        {
            return weightedMajority(e);
        }

        public int[] test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(predict(e)))
                {
                    results[0] = results[0] + 1;
                }
                else
                {
                    results[1] = results[1] + 1;
                }
            }
            return results;
        }

        //
        // PRIVATE METHODS
        //

        private string weightedMajority(Example e)
        {
            IList<string> targetValues = dataSet.getPossibleAttributeValues(dataSet.getTargetAttributeName());

            Table<string, Learner, double> table = createTargetValueLearnerTable(targetValues, e);
            return getTargetValueWithTheMaximumVotes(targetValues, table);
        }

        private Table<string, Learner, double> createTargetValueLearnerTable(IList<string> targetValues, Example e)
        {
            // create a table with target-attribute values as rows and learners as
            // columns and cells containing the weighted votes of each Learner for a
            // target value
            // Learner1 Learner2 Laerner3 .......
            // Yes 0.83 0.5 0
            // No 0 0 0.6

            Table<string, Learner, double> table = new Table<string, Learner, double>(targetValues, learners);
            // initialize table
            foreach (Learner l in learners)
            {
                foreach (string s in targetValues)
                {
                    table.Set(s, l, 0.0);
                }
            }
            foreach (Learner learner in learners)
            {
                string predictedValue = learner.predict(e);
                foreach (string v in targetValues)
                {
                    if (predictedValue.Equals(v))
                    {
                        table.Set(v, learner, table.Get(v, learner) + learnerWeights[learner] * 1);
                    }
                }
            }
            return table;
        }

        private string getTargetValueWithTheMaximumVotes(IList<string> targetValues, Table<string, Learner, double> table)
        {
            string targetValueWithMaxScore = targetValues[0];
            double score = scoreOfValue(targetValueWithMaxScore, table, learners);
            foreach (string value in targetValues)
            {
                double _scoreOfValue = scoreOfValue(value, table, learners);
                if (_scoreOfValue > score)
                {
                    targetValueWithMaxScore = value;
                    score = _scoreOfValue;
                }
            }
            return targetValueWithMaxScore;
        }

        private void initializeExampleWeights(int size)
        {
            if (size == 0)
            {
                throw new Exception("cannot initialize Ensemble learning with Empty Dataset");
            }
            double value = 1.0 / (1.0 * size);
            exampleWeights = new double[size];
            for (int i = 0; i < size; i++)
            {
                exampleWeights[i] = value;
            }
        }

        private void initializeHypothesisWeights(int size)
        {
            if (size == 0)
            {
                throw new Exception("cannot initialize Ensemble learning with Zero Learners");
            }

            learnerWeights = new Dictionary<Learner, double>();
            foreach (Learner le in learners)
            {
                learnerWeights[le] = 1.0;
            }
        }

        private double calculateError(DataSet ds, Learner l)
        {
            double error = 0.0;
            for (int i = 0; i < ds.examples.Count; i++)
            {
                Example e = ds.getExample(i);
                if (!(l.predict(e).Equals(e.targetValue())))
                {
                    error = error + exampleWeights[i];
                }
            }
            return error;
        }

        private void adjustExampleWeights(DataSet ds, Learner l, double error)
        {
            double epsilon = error / (1.0 - error);
            for (int j = 0; j < ds.examples.Count; j++)
            {
                Example e = ds.getExample(j);
                if ((l.predict(e).Equals(e.targetValue())))
                {
                    exampleWeights[j] = exampleWeights[j] * epsilon;
                }
            }
            exampleWeights = Util.normalize(exampleWeights);
        }

        private double scoreOfValue(string targetValue, Table<string, Learner, double> table, IList<Learner> learners)
        {
            double score = 0.0;
            foreach (Learner l in learners)
            {
                score += table.Get(targetValue, l);
            }
            return score;
        }
    } 
}
