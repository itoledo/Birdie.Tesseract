using System;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.learners;
using tvn.cosine.ai.util;

namespace TvnTestConsoleApp.demo.learning
{
    class DecisionTreeDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));
            Console.WriteLine("\nDecisionTree Demo - Inducing a DecisionList from the Restaurant DataSet\n ");
            Console.WriteLine();
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));

            decisionTreeDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void decisionTreeDemo()
        { 
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            DecisionTreeLearner learner = new DecisionTreeLearner();
            learner.train(ds);
            Console.WriteLine("The Induced Decision Tree is ");
            Console.WriteLine(learner.getDecisionTree());
            int[] result = learner.test(ds);

            Console.WriteLine("\nThis Decision Tree classifies the data set with "
                       + result[0]
                       + " successes"
                       + " and "
                       + result[1]
                       + " failures");
            System.Console.WriteLine("\n");
        }
    }
}
