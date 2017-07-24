﻿using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;
using tvn.cosine.ai.util;
 
namespace tvn_cosine.ai.demo.learning.chapter18
{
    public class EnsembleLearningDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n Ensemble Decision Demo - Weak Learners co operating to give Superior decisions ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            ensembleLearningDemo();
        }

        static void ensembleLearningDemo()
        {
            try
            {
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                IQueue<DecisionTree> stumps = DecisionTree.getStumpsFor(ds, "Yes", "No");
                IQueue<Learner> learners = Factory.CreateQueue<Learner>();

                System.Console.WriteLine("\nStump Learners vote to decide in this algorithm");
                foreach (object stump in stumps)
                {
                    DecisionTree sl = (DecisionTree)stump;
                    StumpLearner stumpLearner = new StumpLearner(sl, "No");
                    learners.Add(stumpLearner);
                }
                AdaBoostLearner learner = new AdaBoostLearner(learners, ds);
                learner.train(ds);
                int[] result = learner.test(ds);
                System.Console.WriteLine("\nThis Ensemble Learner  classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
                System.Console.WriteLine("\n");

            }
            catch (Exception e)
            {
                throw e; 
            }
        }
    }
}