using System;
using tvn.cosine.ai.probability.bayes.exact;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.probability
{
    class BayesEliminationAskDemo
    {
        public static void Main(params string[] args)
        {
            bayesEliminationAskDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void bayesEliminationAskDemo()
        {
            Console.WriteLine("DEMO: Bayes Elimination Ask");
            Console.WriteLine("===========================");
            Util.demoToothacheCavityCatchModel(new FiniteBayesModel<bool>(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new EliminationAsk<bool>()));
            Util.demoBurglaryAlarmModel(new FiniteBayesModel<bool>(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new EliminationAsk<bool>()));
            Console.WriteLine("===========================");
        }
    }
}
