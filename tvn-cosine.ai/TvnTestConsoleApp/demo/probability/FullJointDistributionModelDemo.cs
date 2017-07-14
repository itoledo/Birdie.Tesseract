using System;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.probability
{
    class FullJointDistributionModelDemo
    {
        public static void Main(params string[] args)
        {
            fullJointDistributionModelDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void fullJointDistributionModelDemo()
        {
            Console.WriteLine("DEMO: Full Joint Distribution Model");
            Console.WriteLine("===================================");
            Util.demoToothacheCavityCatchModel(new FullJointDistributionToothacheCavityCatchModel());
            Util.demoBurglaryAlarmModel(new FullJointDistributionBurglaryAlarmModel());
            Console.WriteLine("===================================");
        }
    }
}
