using System;
using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.probability
{
    class BayesRejectionSamplingDemo
    {
        public static void Main(params string[] args)
        {
            bayesRejectionSamplingDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void bayesRejectionSamplingDemo()
        {
            Console.WriteLine("DEMO: Bayes Rejection Sampling N = " + Util.NUM_SAMPLES);
            Console.WriteLine("==============================");
            Util.demoToothacheCavityCatchModel(new FiniteBayesModel<bool>(
                     BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                     new BayesInferenceApproxAdapter<bool>(new RejectionSampling<bool>(),
                            Util.NUM_SAMPLES)));
            Util.demoBurglaryAlarmModel(new FiniteBayesModel<bool>(
                     BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                     new BayesInferenceApproxAdapter<bool>(new RejectionSampling<bool>(),
                            Util.NUM_SAMPLES)));
            Console.WriteLine("==============================");
        }
    }
}
