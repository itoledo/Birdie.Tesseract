using System;
using tvn.cosine.ai.probability.bayes.approx;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.util;

namespace TvnTestConsoleApp.demo.probability
{
    class ParticleFilterinfDemo
    {
        public static void Main(params string[] args)
        {
            particleFilterinfDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void particleFilterinfDemo()
        {
            Console.WriteLine("DEMO: Particle-Filtering");
            Console.WriteLine("========================");
            Console.WriteLine("Figure 15.18");
            Console.WriteLine("------------");

            MockRandomizer mr = new MockRandomizer(new double[] {
				// Prior Sample:
				// 8 with Rain_t-1=true from prior distribution
				0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5,
				// 2 with Rain_t-1=false from prior distribution
				0.6, 0.6,
				// (a) Propagate 6 samples Rain_t=true
				0.7, 0.7, 0.7, 0.7, 0.7, 0.7,
				// 4 samples Rain_t=false
				0.71, 0.71, 0.31, 0.31,
				// (b) Weight should be for first 6 samples:
				// Rain_t-1=true, Rain_t=true, Umbrella_t=false = 0.1
				// Next 2 samples:
				// Rain_t-1=true, Rain_t=false, Umbrealla_t=false= 0.8
				// Final 2 samples:
				// Rain_t-1=false, Rain_t=false, Umbrella_t=false = 0.8
				// gives W[] =
				// [0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.8, 0.8, 0.8, 0.8]
				// normalized =
				// [0.026, ...., 0.211, ....] is approx. 0.156 = true
				// the remainder is false
				// (c) Resample 2 Rain_t=true, 8 Rain_t=false
				0.15, 0.15, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2,
				//
				// Next Sample:
				// (a) Propagate 1 samples Rain_t=true
				0.7,
				// 9 samples Rain_t=false
				0.71, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31,
				// (c) resample 1 Rain_t=true, 9 Rain_t=false
				0.0001, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2 });

            int N = 10;
            ParticleFiltering<bool> pf = new ParticleFiltering<bool>(N, DynamicBayesNetExampleFactory.getUmbrellaWorldNetwork(), new Random());

            AssignmentProposition<bool>[] e = new AssignmentProposition<bool>[] { new AssignmentProposition<bool>(ExampleRV.UMBREALLA_t_RV, false) };

            Console.WriteLine("First Sample Set:");
            AssignmentProposition<bool>[][] S = pf.particleFiltering(e);
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine("Sample " + (i + 1) + " = " + S[i][0]);
            }
            Console.WriteLine("Second Sample Set:");
            S = pf.particleFiltering(e);
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine("Sample " + (i + 1) + " = " + S[i][0]);
            }

            Console.WriteLine("========================");
        }
    }
}
