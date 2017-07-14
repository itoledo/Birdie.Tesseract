using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.vacuum
{
    /// <summary>
    ///  Implements a fully observable environment percept, in accordance with page 134, AIMAv3.
    /// </summary>
    public interface FullyObservableVacuumEnvironmentPercept : Percept
    {
        /// <summary>
        /// Returns the agent location
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        string getAgentLocation(Agent a);

        /// <summary>
        /// Returns the location state
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        VacuumEnvironment.LocationState getLocationState(string location);
    }
     
}
