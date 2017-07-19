﻿using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
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
        public static IQueue<Action> getActions(VacuumEnvironmentState state)
        {
            IQueue<Action> actions = Factory.CreateQueue<Action>();
            actions.Add(VacuumEnvironment.ACTION_SUCK);
            actions.Add(VacuumEnvironment.ACTION_MOVE_LEFT);
            actions.Add(VacuumEnvironment.ACTION_MOVE_RIGHT);
            // Ensure cannot be modified.
            return Factory.CreateReadOnlyQueue<Action>(actions);
        }

        public static bool testGoal(VacuumEnvironmentState state)
        {
            return state.getLocationState(VacuumEnvironment.LOCATION_A) == VacuumEnvironment.LocationState.Clean
                    && state.getLocationState(VacuumEnvironment.LOCATION_B) == VacuumEnvironment.LocationState.Clean;
        }

        public static ResultsFunction<VacuumEnvironmentState, Action>
            createResultsFunction(Agent agent)
        {
            return new VacuumWorldResults(agent);
        }

        /**
         * Returns possible results.
         */
        private class VacuumWorldResults : ResultsFunction<VacuumEnvironmentState, Action>
        {
            private Agent agent;

            public VacuumWorldResults(Agent agent)
            {
                this.agent = agent;
            }

            /**
             * Returns a list of possible results for a given state and action.
             */
            public IQueue<VacuumEnvironmentState> results(VacuumEnvironmentState state, Action action)
            {
                IQueue<VacuumEnvironmentState> results = Factory.CreateQueue<VacuumEnvironmentState>();
                // add clone of state to results, modify later...
                VacuumEnvironmentState s = state.clone();
                results.Add(s);

               string currentLocation = state.getAgentLocation(agent);
               string adjacentLocation = (currentLocation.Equals(VacuumEnvironment.LOCATION_A))
                         ? VacuumEnvironment.LOCATION_B : VacuumEnvironment.LOCATION_A;

                if (action == VacuumEnvironment.ACTION_MOVE_RIGHT)
                {
                    s.setAgentLocation(agent, VacuumEnvironment.LOCATION_B);

                }
                else if (action == VacuumEnvironment.ACTION_MOVE_LEFT)
                {
                    s.setAgentLocation(agent, VacuumEnvironment.LOCATION_A);

                }
                else if (action == VacuumEnvironment.ACTION_SUCK)
                {
                    if (state.getLocationState(currentLocation) == VacuumEnvironment.LocationState.Dirty)
                    {
                        // always clean current
                        s.setLocationState(currentLocation, VacuumEnvironment.LocationState.Clean);
                        // sometimes clean adjacent as well
                        VacuumEnvironmentState s2 = s.clone();
                        s2.setLocationState(adjacentLocation, VacuumEnvironment.LocationState.Clean);
                        if (!s2.Equals(s))
                            results.Add(s2);
                    }
                    else
                    {
                        // sometimes do nothing (-> s unchanged)
                        // sometimes deposit dirt
                        VacuumEnvironmentState s2 = s.clone();
                        s2.setLocationState(currentLocation, VacuumEnvironment.LocationState.Dirty);
                        if (!s2.Equals(s))
                            results.Add(s2);
                    }
                }
                return results;
            }
        }
    }
}
