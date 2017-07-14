using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.environment.map
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class MapEnvironmentState : EnvironmentState
    {

        private IDictionary<Agent, Pair<string, double>> agentLocationAndTravelDistance = new Dictionary<Agent, Pair<string, double>>();

        public MapEnvironmentState()
        {

        }

        public string getAgentLocation(Agent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance[a];
            if (null == locAndTDistance)
            {
                return null;
            }
            return locAndTDistance.First;
        }

        public double getAgentTravelDistance(Agent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance[a];
            if (null == locAndTDistance)
            {
                return 0D;
            }
            return locAndTDistance.Second;
        }

        public void setAgentLocationAndTravelDistance(Agent a, string location, double travelDistance)
        {
            agentLocationAndTravelDistance.Add(a, new Pair<string, double>(location, travelDistance));
        }
    }

}
