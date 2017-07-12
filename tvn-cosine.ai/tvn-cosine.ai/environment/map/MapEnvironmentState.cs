using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.environment.map
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class MapEnvironmentState : IEnvironmentState
    {

        private IDictionary<IAgent, Pair<string, double>> agentLocationAndTravelDistance = new Dictionary<IAgent, Pair<string, double>>();

        public MapEnvironmentState()
        {

        }

        public string getAgentLocation(IAgent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance[a];
            if (null == locAndTDistance)
            {
                return null;
            }
            return locAndTDistance.First;
        }

        public double getAgentTravelDistance(IAgent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance[a];
            if (null == locAndTDistance)
            {
                return 0D;
            }
            return locAndTDistance.Second;
        }

        public void setAgentLocationAndTravelDistance(IAgent a, string location, double travelDistance)
        {
            agentLocationAndTravelDistance.Add(a, new Pair<string, double>(location, travelDistance));
        }
    }

}
