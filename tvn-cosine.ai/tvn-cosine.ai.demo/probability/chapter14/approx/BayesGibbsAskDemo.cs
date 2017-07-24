using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;

namespace tvn_cosine.ai.demo.probability.chapter14.approx
{
    class BayesGibbsAskDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            bayesGibbsAskDemo();
        }

        static void bayesGibbsAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Gibbs Ask N = " + NUM_SAMPLES);
            System.Console.WriteLine("=====================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new BayesInferenceApproxAdapter(new GibbsAsk(), NUM_SAMPLES)));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new BayesInferenceApproxAdapter(new GibbsAsk(), NUM_SAMPLES)));
            System.Console.WriteLine("=====================");
        } 
    }
}
