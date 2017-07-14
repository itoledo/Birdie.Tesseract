using System;
using tvn.cosine.ai.probability.bayes.exact;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.probability
{
    class BayesEnumerationAskDemo
    {
        public static void Main(params string[] args)
        {
            bayesEnumerationAskDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void bayesEnumerationAskDemo()
        {
            Console.WriteLine("DEMO: Bayes Enumeration Ask");
            Console.WriteLine("===========================");
            Util.demoToothacheCavityCatchModel(new FiniteBayesModel<bool>(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new EnumerationAsk<bool>()));
            Util.demoBurglaryAlarmModel(new FiniteBayesModel<bool>(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new EnumerationAsk<bool>()));
            Console.WriteLine("===========================");
        }
    }
}
