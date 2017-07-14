using System;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;
using tvn.cosine.ai.util;

namespace TvnTestConsoleApp.demo.learning
{
    class DecisionListDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));
            Console.WriteLine("DecisionList Demo - Inducing a DecisionList from the Restaurant DataSet\n ");
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));
            Console.WriteLine();

            decisionListDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }


        public static void decisionListDemo()
        { 
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                DecisionListLearner learner = new DecisionListLearner("Yes", "No", new DLTestFactory());
                learner.train(ds);
                Console.WriteLine("The Induced DecisionList is");
                Console.WriteLine(learner.getDecisionList());
                int[] result = learner.test(ds);

                Console.WriteLine("\nThis Decision List classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
                Console.WriteLine("\n");
                 
        }
    }
}
