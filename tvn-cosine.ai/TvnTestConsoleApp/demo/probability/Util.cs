using System;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.proposition;

namespace TvnTestConsoleApp.demo.probability
{
    public class Util
    {
        public const int NUM_SAMPLES = 1000;

        public static void demoToothacheCavityCatchModel(FiniteProbabilityModel<bool> model)
        {
            Console.WriteLine("Toothache, Cavity, and Catch Model");
            Console.WriteLine("----------------------------------");
            AssignmentProposition<bool> atoothache = new AssignmentProposition<bool>(ExampleRV.TOOTHACHE_RV, true);
            AssignmentProposition<bool> acavity = new AssignmentProposition<bool>(ExampleRV.CAVITY_RV, true);
            AssignmentProposition<bool> anotcavity = new AssignmentProposition<bool>(ExampleRV.CAVITY_RV, false);
            AssignmentProposition<bool> acatch = new AssignmentProposition<bool>(ExampleRV.CATCH_RV, true);

            // AIMA3e pg. 485
            Console.WriteLine("P(cavity) = " + model.prior(acavity));
            Console.WriteLine("P(cavity | toothache) = "
                    + model.posterior(acavity, atoothache));

            // AIMA3e pg. 492
            DisjunctiveProposition<bool> cavityOrToothache = new DisjunctiveProposition<bool>(acavity, atoothache);
            Console.WriteLine("P(cavity OR toothache) = " + model.prior(cavityOrToothache));

            // AIMA3e pg. 493
            Console.WriteLine("P(~cavity | toothache) = " + model.posterior(anotcavity, atoothache));

            // AIMA3e pg. 493
            // P<>(Cavity | toothache) = <0.6, 0.4>
            Console.WriteLine("P<>(Cavity | toothache) = " + model.posteriorDistribution(ExampleRV.CAVITY_RV, atoothache));

            // AIMA3e pg. 497
            // P<>(Cavity | toothache AND catch) = <0.871, 0.129>
            Console.WriteLine("P<>(Cavity | toothache AND catch) = " + model.posteriorDistribution(ExampleRV.CAVITY_RV, atoothache, acatch));
        }

        public static void demoBurglaryAlarmModel(FiniteProbabilityModel<bool> model)
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("Burglary Alarm Model");
            Console.WriteLine("--------------------");

            AssignmentProposition<bool> aburglary = new AssignmentProposition<bool>(ExampleRV.BURGLARY_RV, true);
            AssignmentProposition<bool> anotburglary = new AssignmentProposition<bool>(ExampleRV.BURGLARY_RV, false);
            AssignmentProposition<bool> anotearthquake = new AssignmentProposition<bool>(ExampleRV.EARTHQUAKE_RV, false);
            AssignmentProposition<bool> aalarm = new AssignmentProposition<bool>(ExampleRV.ALARM_RV, true);
            AssignmentProposition<bool> anotalarm = new AssignmentProposition<bool>(ExampleRV.ALARM_RV, false);
            AssignmentProposition<bool> ajohnCalls = new AssignmentProposition<bool>(ExampleRV.JOHN_CALLS_RV, true);
            AssignmentProposition<bool> amaryCalls = new AssignmentProposition<bool>(ExampleRV.MARY_CALLS_RV, true);

            // AIMA3e pg. 514
            Console.WriteLine("P(j,m,a,~b,~e) = "
                    + model.prior(ajohnCalls, amaryCalls, aalarm, anotburglary,
                            anotearthquake));
            Console.WriteLine("P(j,m,~a,~b,~e) = "
                    + model.prior(ajohnCalls, amaryCalls, anotalarm, anotburglary,
                            anotearthquake));

            // AIMA3e. pg. 514
            // P<>(Alarm | JohnCalls = true, MaryCalls = true, Burglary = false,
            // Earthquake = false)
            // = <0.558, 0.442>
            Console.WriteLine("P<>(Alarm | JohnCalls = true, MaryCalls = true, Burglary = false, Earthquake = false) = "
                        + model.posteriorDistribution(ExampleRV.ALARM_RV,
                                ajohnCalls, amaryCalls, anotburglary,
                                anotearthquake));

            // AIMA3e pg. 523
            // P<>(Burglary | JohnCalls = true, MaryCalls = true) = <0.284, 0.716>
            Console.WriteLine("P<>(Burglary | JohnCalls = true, MaryCalls = true) = "
                        + model.posteriorDistribution(ExampleRV.BURGLARY_RV,
                                ajohnCalls, amaryCalls));

            // AIMA3e pg. 528
            // P<>(JohnCalls | Burglary = true)
            Console.WriteLine("P<>(JohnCalls | Burglary = true) = "
                    + model.posteriorDistribution(ExampleRV.JOHN_CALLS_RV,
                            aburglary));
        }
    }
}
