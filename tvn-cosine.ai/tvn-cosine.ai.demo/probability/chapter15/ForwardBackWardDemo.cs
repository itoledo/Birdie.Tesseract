﻿using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.temporal.generic;
using tvn.cosine.ai.probability.util;

namespace tvn_cosine.ai.demo.probability.chapter15
{
    public class ForwardBackWardDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            forwardBackWardDemo();
        }

        static void forwardBackWardDemo()
        {
            System.Console.WriteLine("DEMO: Forward-BackWard");
            System.Console.WriteLine("======================");

            System.Console.WriteLine("Umbrella World");
            System.Console.WriteLine("--------------");
            ForwardBackward uw = new ForwardBackward(
                    GenericTemporalModelFactory.getUmbrellaWorldTransitionModel(),
                    GenericTemporalModelFactory.getUmbrellaWorld_Xt_to_Xtm1_Map(),
                    GenericTemporalModelFactory.getUmbrellaWorldSensorModel());

            CategoricalDistribution prior = new ProbabilityTable(new double[] {
                0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1
            IQueue<IQueue<AssignmentProposition>> evidence = Factory.CreateQueue<IQueue<AssignmentProposition>>();
            IQueue<AssignmentProposition> e1 = Factory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e1);

            IQueue<CategoricalDistribution> smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 1 (Umbrealla_t=true) smoothed:\nday 1 = " + smoothed.Get(0));

            // Day 2
            IQueue<AssignmentProposition> e2 = Factory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e2);

            smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 2 (Umbrealla_t=true) smoothed:\nday 1 = "
                    + smoothed.Get(0) + "\nday 2 = " + smoothed.Get(1));

            // Day 3
            IQueue<AssignmentProposition> e3 = Factory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false));
            evidence.Add(e3);

            smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 3 (Umbrealla_t=false) smoothed:\nday 1 = "
                    + smoothed.Get(0) + "\nday 2 = " + smoothed.Get(1)
                    + "\nday 3 = " + smoothed.Get(2));

            System.Console.WriteLine("======================");
        }
    }
}
