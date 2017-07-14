using System;
using System.Collections.Generic;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.temporal.generic;
using tvn.cosine.ai.probability.util;

namespace TvnTestConsoleApp.demo.probability
{
    class ForwardBackWardDemo
    {
        public static void Main(params string[] args)
        {
            forwardBackWardDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void forwardBackWardDemo()
        {

            Console.WriteLine("DEMO: Forward-BackWard");
            Console.WriteLine("======================");

            Console.WriteLine("Umbrella World");
            Console.WriteLine("--------------");
            ForwardBackward<bool> uw = new ForwardBackward<bool>(
                    GenericTemporalModelFactory.getUmbrellaWorldTransitionModel(),
                    GenericTemporalModelFactory.getUmbrellaWorld_Xt_to_Xtm1_Map(),
                    GenericTemporalModelFactory.getUmbrellaWorldSensorModel());

            CategoricalDistribution<bool> prior = new ProbabilityTable<bool>(new double[] { 0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1
            IList<IList<AssignmentProposition<bool>>> evidence = new List<IList<AssignmentProposition<bool>>>();
            IList<AssignmentProposition<bool>> e1 = new List<AssignmentProposition<bool>>();
            e1.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e1);

            IList<CategoricalDistribution<bool>> smoothed = uw.forwardBackward(evidence, prior);

            Console.WriteLine("Day 1 (Umbrealla_t=true) smoothed:\nday 1 = " + smoothed[0]);

            // Day 2
            IList<AssignmentProposition<bool>> e2 = new List<AssignmentProposition<bool>>();
            e2.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e2);

            smoothed = uw.forwardBackward(evidence, prior);

            Console.WriteLine("Day 2 (Umbrealla_t=true) smoothed:\nday 1 = " + smoothed[0] + "\nday 2 = " + smoothed[1]);

            // Day 3
            IList<AssignmentProposition<bool>> e3 = new List<AssignmentProposition<bool>>();
            e3.Add(new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, false));
            evidence.Add(e3);

            smoothed = uw.forwardBackward(evidence, prior);

            Console.WriteLine("Day 3 (Umbrealla_t=false) smoothed:\nday 1 = "
                    + smoothed[0] + "\nday 2 = " + smoothed[1]
                    + "\nday 3 = " + smoothed[2]);

            Console.WriteLine("======================");
        }
    }
}
