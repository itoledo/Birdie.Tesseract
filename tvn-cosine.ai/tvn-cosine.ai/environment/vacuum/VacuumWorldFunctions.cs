using System.Collections.Generic;
using System.Collections.ObjectModel;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.search.nondeterministic;

namespace tvn.cosine.ai.environment.vacuum
{
    /**
     * Contains useful functions for the vacuum cleaner world.
     *
     * @author Ruediger Lunde
     * @author Andrew Brown
     */
    public class VacuumWorldFunctions
    {

        /**
         * Specifies the actions available to the agent at state s
         */
        public static IList<Action> getActions(VacuumEnvironmentState state)
        {
            IList<Action> actions = new List<Action>();
            actions.Add(VacuumEnvironment.ACTION_SUCK);
            actions.Add(VacuumEnvironment.ACTION_MOVE_LEFT);
            actions.Add(VacuumEnvironment.ACTION_MOVE_RIGHT);
            // Ensure cannot be modified.
            return new ReadOnlyCollection<Action>(actions);
        }

        public static bool testGoal(VacuumEnvironmentState state)
        {
            return state.getLocationState(VacuumEnvironment.LOCATION_A) == VacuumEnvironment.LocationState.Clean
                    && state.getLocationState(VacuumEnvironment.LOCATION_B) == VacuumEnvironment.LocationState.Clean;
        }

        public static ResultsFunction<VacuumEnvironmentState, Action> createResultsFunction(Agent agent)
        {
            return new VacuumWorldResults(agent);
        }

        /**
         * Returns possible results
         */
        private class VacuumWorldResults : ResultsFunction<VacuumEnvironmentState, Action>
        {

            private Agent agent;

            public VacuumWorldResults(Agent agent)
            {
                this.agent = agent;
            }

            /**
             * Returns a list of possible results for a given state and action
             *
             * @return a list of possible results for a given state and action.
             */
            public IList<VacuumEnvironmentState> results(VacuumEnvironmentState state, Action action)
            {
                // Ensure order is consistent across platforms.
                IList<VacuumEnvironmentState> results = new List<VacuumEnvironmentState>();
                string currentLocation = state.getAgentLocation(agent);
                string adjacentLocation = (currentLocation.Equals(VacuumEnvironment.LOCATION_A)) ? VacuumEnvironment.LOCATION_B
                        : VacuumEnvironment.LOCATION_A;
                // case: move right
                if (VacuumEnvironment.ACTION_MOVE_RIGHT == action)
                {
                    VacuumEnvironmentState s = new VacuumEnvironmentState();
                    s.setLocationState(currentLocation,
                            state.getLocationState(currentLocation));
                    s.setLocationState(adjacentLocation,
                            state.getLocationState(adjacentLocation));
                    s.setAgentLocation(this.agent, VacuumEnvironment.LOCATION_B);
                    results.Add(s);
                } // case: move left
                else if (VacuumEnvironment.ACTION_MOVE_LEFT == action)
                {
                    VacuumEnvironmentState s = new VacuumEnvironmentState();
                    s.setLocationState(currentLocation,
                            state.getLocationState(currentLocation));
                    s.setLocationState(adjacentLocation,
                            state.getLocationState(adjacentLocation));
                    s.setAgentLocation(this.agent, VacuumEnvironment.LOCATION_A);
                    results.Add(s);
                } // case: suck
                else if (VacuumEnvironment.ACTION_SUCK == action)
                {
                    // case: square is dirty
                    if (VacuumEnvironment.LocationState.Dirty == state
                            .getLocationState(state.getAgentLocation(this.agent)))
                    {
                        // always clean current
                        VacuumEnvironmentState s1 = new VacuumEnvironmentState();
                        s1.setLocationState(currentLocation,
                                VacuumEnvironment.LocationState.Clean);
                        s1.setLocationState(adjacentLocation,
                                state.getLocationState(adjacentLocation));
                        s1.setAgentLocation(this.agent, currentLocation);
                        results.Add(s1);
                        // sometimes clean adjacent as well
                        VacuumEnvironmentState s2 = new VacuumEnvironmentState();
                        s2.setLocationState(currentLocation,
                                VacuumEnvironment.LocationState.Clean);
                        s2.setLocationState(adjacentLocation,
                                VacuumEnvironment.LocationState.Clean);
                        s2.setAgentLocation(this.agent, currentLocation);
                        results.Add(s2);
                    } // case: square is clean
                    else
                    {
                        // sometimes do nothing
                        VacuumEnvironmentState s1 = new VacuumEnvironmentState();
                        s1.setLocationState(currentLocation,
                                state.getLocationState(currentLocation));
                        s1.setLocationState(adjacentLocation,
                                state.getLocationState(adjacentLocation));
                        s1.setAgentLocation(this.agent, currentLocation);
                        results.Add(s1);
                        // sometimes deposit dirt
                        VacuumEnvironmentState s2 = new VacuumEnvironmentState();
                        s2.setLocationState(currentLocation,
                                VacuumEnvironment.LocationState.Dirty);
                        s2.setLocationState(adjacentLocation,
                                state.getLocationState(adjacentLocation));
                        s2.setAgentLocation(this.agent, currentLocation);
                        results.Add(s2);
                    }
                }
                return results;
            }
        }
    }
}
