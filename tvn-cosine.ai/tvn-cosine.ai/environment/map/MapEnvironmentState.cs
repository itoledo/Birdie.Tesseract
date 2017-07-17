using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;

namespace tvn.cosine.ai.environment.map
{
    public class MapEnvironmentState : EnvironmentState
    {

        private IMap<Agent, Pair<string, double>> agentLocationAndTravelDistance;

        public MapEnvironmentState()
        {
            agentLocationAndTravelDistance = Factory.CreateMap<Agent, Pair<string, double>>();
        }

        public string getAgentLocation(Agent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance.Get(a);
            if (null == locAndTDistance)
            {
                return null;
            }
            return locAndTDistance.getFirst();
        }

        public double getAgentTravelDistance(Agent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance.Get(a);
            if (null == locAndTDistance)
            {
                return 0D;
            }
            return locAndTDistance.getSecond();
        }

        public void setAgentLocationAndTravelDistance(Agent a, string location,
                double travelDistance)
        {
            agentLocationAndTravelDistance.Put(a, new Pair<string, double>(
                    location, travelDistance));
        }
    }
}
