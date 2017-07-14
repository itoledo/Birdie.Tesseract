using System;
using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.probability
{
    class BayesLikelihoodWeightingDemo
    {
        public static void Main(params string[] args)
        {
            bayesLikelihoodWeightingDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void bayesLikelihoodWeightingDemo()
        {
            Console.WriteLine("DEMO: Bayes Likelihood Weighting N = " + Util.NUM_SAMPLES);
            Console.WriteLine("================================");
            Util.demoToothacheCavityCatchModel(new FiniteBayesModel<bool>(
                     BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                     new BayesInferenceApproxAdapter<bool>(new LikelihoodWeighting<bool>(),
                            Util.NUM_SAMPLES)));
            Util.demoBurglaryAlarmModel(new FiniteBayesModel<bool>(
                     BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                     new BayesInferenceApproxAdapter<bool>(new LikelihoodWeighting<bool>(),
                            Util.NUM_SAMPLES)));
            Console.WriteLine("================================");
        }
    }
}
