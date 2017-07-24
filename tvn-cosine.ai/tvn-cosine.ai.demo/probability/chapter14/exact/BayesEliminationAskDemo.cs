using tvn.cosine.ai.probability.bayes.exact;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;

namespace tvn_cosine.ai.demo.probability.chapter14.exact
{
    public class BayesEliminationAskDemo : ProbabilityDemoBase
    {
        public static void Main(params string[] args)
        {
            bayesEliminationAskDemo();
        }

        static void bayesEliminationAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Elimination Ask");
            System.Console.WriteLine("===========================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new EliminationAsk()));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new EliminationAsk()));
            System.Console.WriteLine("===========================");
        }
    }
}
