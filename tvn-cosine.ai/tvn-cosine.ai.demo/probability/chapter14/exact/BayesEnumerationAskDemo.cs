using tvn.cosine.ai.probability.bayes.exact;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;

namespace tvn_cosine.ai.demo.probability.chapter14.exact
{
    public class BayesEnumerationAskDemo : ProbabilityDemoBase
    {
        public static void Main(params string[] args)
        {
            bayesEnumerationAskDemo();
        }

        static void bayesEnumerationAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Enumeration Ask");
            System.Console.WriteLine("===========================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new EnumerationAsk()));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new EnumerationAsk()));
            System.Console.WriteLine("===========================");
        }
    }
}
