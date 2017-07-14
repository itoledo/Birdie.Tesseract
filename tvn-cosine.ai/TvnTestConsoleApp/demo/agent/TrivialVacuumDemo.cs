using System;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.environment.vacuum;

namespace TvnTestConsoleApp.demo.agent
{
    /// <summary>
    /// Demonstrates, how to set up a simple environment, place an agent in it, and run it.The vacuum world is used as a simple example.
    /// </summary>
    public class TrivialVacuumDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("TRIVIAL VACUUM DEMO");
            Console.WriteLine();

            // create environment with random state of cleaning.
            tvn.cosine.ai.agent.Environment env = new VacuumEnvironment();
            EnvironmentView view = new SimpleEnvironmentView();
            env.addEnvironmentView(view);

            Agent a = new ModelBasedReflexVacuumAgent();
            // a = new ReflexVacuumAgent();
            // a = new SimpleReflexVacuumAgent();
            // a = new TableDrivenVacuumAgent();

            env.addAgent(a);
            env.step(16);
            env.notifyViews("Performance=" + env.getPerformanceMeasure(a));

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }
    }
}
