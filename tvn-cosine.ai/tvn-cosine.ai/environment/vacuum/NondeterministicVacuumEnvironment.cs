using System;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.vacuum
{
    /// <summary>
    /// Create the erratic vacuum world from page 134, AIMA3e. In the erratic vacuum
    /// world, the Suck action works as follows: 1) when applied to a dirty square
    /// the action cleans the square and sometimes cleans up dirt in an adjacent
    /// square too; 2) when applied to a clean square the action sometimes deposits
    /// dirt on the carpet.
    /// </summary>
    public class NondeterministicVacuumEnvironment : VacuumEnvironment
    {
        private readonly Random random = new Random();
        /// <summary>
        /// Construct a vacuum environment with two locations, in which dirt is placed at random.
        /// </summary>
        public NondeterministicVacuumEnvironment()
            : base()
        { }

        /// <summary>
        /// Construct a vacuum environment with two locations, in which dirt is placed as specified.
        /// </summary>
        /// <param name="locAState">the initial state of location A, which is either Clean or Dirty.</param>
        /// <param name="locBState">the initial state of location A, which is either Clean or Dirty.</param>
        public NondeterministicVacuumEnvironment(LocationState locAState, LocationState locBState)
            : base(locAState, locBState)
        {

        }

        /// <summary>
        /// Execute the agent action
        /// </summary>
        /// <param name="a"></param>
        /// <param name="agentAction"></param>
        public override void executeAction(IAgent a, IAction agentAction)
        {
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
