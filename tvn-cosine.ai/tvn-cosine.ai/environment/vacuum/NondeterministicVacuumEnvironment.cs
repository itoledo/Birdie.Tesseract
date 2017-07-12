using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.vacuum
{
    /**
     * Create the erratic vacuum world from page 134, AIMA3e. In the erratic vacuum
     * world, the Suck action works as follows: 1) when applied to a dirty square
     * the action cleans the square and sometimes cleans up dirt in an adjacent
     * square too; 2) when applied to a clean square the action sometimes deposits
     * dirt on the carpet.
     *
     * @author Andrew Brown
     */
    public class NondeterministicVacuumEnvironment : VacuumEnvironment
    {

        /**
         * Construct a vacuum environment with two locations, in which dirt is
         * placed at random.
         */
        public NondeterministicVacuumEnvironment()
                : base()
        { }

        /**
         * Construct a vacuum environment with two locations, in which dirt is
         * placed as specified.
         *
         * @param locAState the initial state of location A, which is either
         * <em>Clean</em> or <em>Dirty</em>.
         * @param locBState the initial state of location B, which is either
         * <em>Clean</em> or <em>Dirty</em>.
         */
        public NondeterministicVacuumEnvironment(LocationState locAState, LocationState locBState)
            : base(locAState, locBState)
        {

        }

        /**
         * Execute the agent action
         */
        public override void executeAction(IAgent a, IAction agentAction)
        {
            System.Random random = new System.Random();
            if (ACTION_MOVE_RIGHT == agentAction)
            {
                envState.setAgentLocation(a, LOCATION_B);
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_MOVE_LEFT == agentAction)
            {
                envState.setAgentLocation(a, LOCATION_A);
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_SUCK == agentAction)
            {
                // case: square is dirty
                if (VacuumEnvironment.LocationState.Dirty == envState.getLocationState(envState.getAgentLocation(a)))
                {
                    string current_location = envState.getAgentLocation(a);
                    string adjacent_location = (current_location.Equals("A")) ? "B" : "A";
                    // always clean current square
                    envState.setLocationState(current_location, VacuumEnvironment.LocationState.Clean);
                    // possibly clean adjacent square
                    if (random.NextDouble() > 0.5)
                    {
                        envState.setLocationState(adjacent_location, VacuumEnvironment.LocationState.Clean);
                    }
                } // case: square is clean
                else if (VacuumEnvironment.LocationState.Clean == envState.getLocationState(envState.getAgentLocation(a)))
                {
                    // possibly dirty current square
                    if (random.NextDouble() > 0.5)
                    {
                        envState.setLocationState(envState.getAgentLocation(a), VacuumEnvironment.LocationState.Dirty);
                    }
                }
            }
            else if (agentAction.IsNoOp())
            {
                _isDone = true;
            }
        }
    }

}
