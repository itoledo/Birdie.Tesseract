using System.Text;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.vacuum;
using tvn.cosine.ai.search.nondeterministic;

namespace tvn_cosine.ai.demo.agent
{
    /**
     * Applies AND-OR-GRAPH-SEARCH to a non-deterministic version of the Vacuum World.
     * 
     * 
     * @author Andrew Brown
     */
    public class NondeterministicVacuumEnvironmentDemo
    {
        public static void Main(params string[] args)
        {
            System.Console.WriteLine("NON-DETERMINISTIC-VACUUM-ENVIRONMENT DEMO");
            System.Console.WriteLine("");
            startAndOrSearch();
        }

        private static void startAndOrSearch()
        {
            System.Console.WriteLine("AND-OR-GRAPH-SEARCH");

            NondeterministicVacuumAgent agent = new NondeterministicVacuumAgent(VacuumWorldFunctions.FullyObservableVacuumEnvironmentPerceptToStateFunction);
            // create state: both rooms are dirty and the vacuum is in room A
            VacuumEnvironmentState state = new VacuumEnvironmentState();
            state.setLocationState(VacuumEnvironment.LOCATION_A, VacuumEnvironment.LocationState.Dirty);
            state.setLocationState(VacuumEnvironment.LOCATION_B, VacuumEnvironment.LocationState.Dirty);
            state.setAgentLocation(agent, VacuumEnvironment.LOCATION_A);
            // create problem
            NondeterministicProblem<VacuumEnvironmentState, IAction> problem = new NondeterministicProblem<VacuumEnvironmentState, IAction>(
                    state,
                    VacuumWorldFunctions.getActions,
                    VacuumWorldFunctions.createResultsFunction(agent),
                    VacuumWorldFunctions.testGoal,
                    (s, a, sPrimed) => 1.0);
            // set the problem and agent
         //   agent.setProblem(problem);

            // create world
            NondeterministicVacuumEnvironment world = new NondeterministicVacuumEnvironment(VacuumEnvironment.LocationState.Dirty, VacuumEnvironment.LocationState.Dirty);
            world.addAgent(agent, VacuumEnvironment.LOCATION_A);

            // execute and show plan
            System.Console.WriteLine("Initial Plan: " + agent.getContingencyPlan());
            StringBuilder sb = new StringBuilder();
            world.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(sb));
            world.StepUntilDone();
            System.Console.WriteLine("Remaining Plan: " + agent.getContingencyPlan());
            System.Console.WriteLine("Actions Taken: " + sb);
            System.Console.WriteLine("Final State: " + world.getCurrentState());
        }
    }
}
