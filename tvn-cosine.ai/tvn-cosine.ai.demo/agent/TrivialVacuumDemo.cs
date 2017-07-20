using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.environment.vacuum;

namespace tvn_cosine.ai.demo.agent
{
    /**
     * Demonstrates, how to set up a simple environment, place an agent in it,
     * and run it. The vacuum world is used as a simple example.
     * 
     * @author Ruediger Lunde
     */
    public class TrivialVacuumDemo
    {
        public static void Main(params string[] args)
        {
            // create environment with random state of cleaning.
            IEnvironment env = new VacuumEnvironment();
            IEnvironmentView view = new SimpleEnvironmentView();
            env.AddEnvironmentView(view);

            IAgent a = null;
            a = new ModelBasedReflexVacuumAgent();
            // a = new ReflexVacuumAgent();
            // a = new SimpleReflexVacuumAgent();
            // a = new TableDrivenVacuumAgent();

            env.AddAgent(a);
            env.Step(16);
            env.NotifyViews("Performance=" + env.GetPerformanceMeasure(a));
        }
    }
}
