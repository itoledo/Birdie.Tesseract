using System;
using System.Collections.Generic;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.hmm.exact;
using tvn.cosine.ai.probability.proposition;

namespace TvnTestConsoleApp.demo.probability
{
    class FixedLagSmoothingDemo
    {
        public static void Main(params string[] args)
        {
            fixedLagSmoothingDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void fixedLagSmoothingDemo()
        {
            Console.WriteLine("DEMO: Fixed-Lag-Smoothing");
            Console.WriteLine("=========================");
            Console.WriteLine("Lag = 1");
            Console.WriteLine("-------");
            FixedLagSmoothing<bool> uw = new FixedLagSmoothing<bool>(HMMExampleFactory.getUmbrellaWorldModel(), 1);

            // Day 1 - Lag 1
            IList<AssignmentProposition<bool>> e1 = new List<AssignmentProposition<bool>>();
            e1.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, true));

            CategoricalDistribution<bool> smoothed = uw.fixedLagSmoothing(e1);

            Console.WriteLine("Day 1 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 2 - Lag 1
            IList<AssignmentProposition<bool>> e2 = new List<AssignmentProposition<bool>>();
            e2.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e2);

            Console.WriteLine("Day 2 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 3 - Lag 1
            IList<AssignmentProposition<bool>> e3 = new List<AssignmentProposition<bool>>();
            e3.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, false));

            smoothed = uw.fixedLagSmoothing(e3);

            Console.WriteLine("Day 3 (Umbrella_t=false) smoothed:\nday 2=" + smoothed);

            Console.WriteLine("-------");
            Console.WriteLine("Lag = 2");
            Console.WriteLine("-------");

            uw = new FixedLagSmoothing<bool>(HMMExampleFactory.getUmbrellaWorldModel(), 2);

            // Day 1 - Lag 2
            e1 = new List<AssignmentProposition<bool>>();
            e1.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, true)); 
            smoothed = uw.fixedLagSmoothing(e1); 
            Console.WriteLine("Day 1 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 2 - Lag 2
            e2 = new List<AssignmentProposition<bool>>();
            e2.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, true)); 
            smoothed = uw.fixedLagSmoothing(e2); 
            Console.WriteLine("Day 2 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 3 - Lag 2
            e3 = new List<AssignmentProposition<bool>>();
            e3.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, false)); 
            smoothed = uw.fixedLagSmoothing(e3);

            Console.WriteLine("Day 3 (Umbrella_t=false) smoothed:\nday 1=" + smoothed); 
            Console.WriteLine("=========================");
        }
    }
}
