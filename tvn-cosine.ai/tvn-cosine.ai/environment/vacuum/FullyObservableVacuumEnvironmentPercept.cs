using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.vacuum
{
    /**
     * Implements a fully observable environment percept, in accordance with page
     * 134, AIMAv3.
     *
     * @author Andrew Brown
     */
    public interface FullyObservableVacuumEnvironmentPercept : IPercept
    {
        /**
         * Returns the agent location
         *
         * @param a
         * @return the agents location
         */
        string getAgentLocation(IAgent a);

        /**
         * Returns the location state
         *
         * @param location
         * @return the location state
         */
        VacuumEnvironment.LocationState getLocationState(string location);
    }
}
