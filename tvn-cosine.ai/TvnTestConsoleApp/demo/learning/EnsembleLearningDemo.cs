using System;
using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;
using tvn.cosine.ai.util;

namespace TvnTestConsoleApp.demo.learning
{
    class EnsembleLearningDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));
            Console.WriteLine("\n\n Ensemble Decision Demo - Weak Learners co operating to give Superior decisions");
            Console.WriteLine();

            ensembleLearningDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void ensembleLearningDemo()
        {
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));

            DataSet ds = DataSetFactory.getRestaurantDataSet();
            IList<DecisionTree> stumps = DecisionTree.getStumpsFor(ds, "Yes", "No");
            IList<Learner> learners = new List<Learner>();

            Console.WriteLine("\nStump Learners vote to decide in this algorithm");
            foreach (DecisionTree stump in stumps)
            { 
                StumpLearner stumpLearner = new StumpLearner(stump, "No");
                learners.Add(stumpLearner);
            }
            AdaBoostLearner learner = new AdaBoostLearner(learners, ds);
            learner.train(ds);
            int[] result = learner.test(ds);
            Console.WriteLine("\nThis Ensemble Learner  classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
            Console.WriteLine("\n");

        }
    }
}
