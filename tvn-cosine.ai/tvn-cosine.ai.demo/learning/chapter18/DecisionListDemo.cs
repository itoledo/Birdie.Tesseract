using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.inductive;
using tvn.cosine.ai.learning.learners;
using tvn.cosine.ai.util;


namespace tvn_cosine.ai.demo.learning.chapter18
{
    public class DecisionListDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("DecisionList Demo - Inducing a DecisionList from the Restaurant DataSet\n ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            decisionListDemo();
        }

        static void decisionListDemo()
        {
            try
            {
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                DecisionListLearner learner = new DecisionListLearner("Yes", "No",
                        new DLTestFactory());
                learner.train(ds);
                System.Console.WriteLine("The Induced DecisionList is");
                System.Console.WriteLine(learner.getDecisionList());
                int[] result = learner.test(ds);

                System.Console.WriteLine("\nThis Decision List classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
                System.Console.WriteLine("\n");

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Decision ListDemo Failed");
                throw e;
            }
        }
    }
}
