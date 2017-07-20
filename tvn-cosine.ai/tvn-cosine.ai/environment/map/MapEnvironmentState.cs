﻿using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;

namespace tvn.cosine.ai.environment.map
{
    public class MapEnvironmentState : IEnvironmentState
    {

        private IMap<IAgent, Pair<string, double>> agentLocationAndTravelDistance;

        public MapEnvironmentState()
        {
            agentLocationAndTravelDistance = Factory.CreateMap<IAgent, Pair<string, double>>();
        }

        public string getAgentLocation(IAgent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance.Get(a);
            if (null == locAndTDistance)
            {
                return null;
            }
            return locAndTDistance.getFirst();
        }

        public double getAgentTravelDistance(IAgent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance.Get(a);
            if (null == locAndTDistance)
            {
                return 0D;
            }
            return locAndTDistance.getSecond();
        }

        public void setAgentLocationAndTravelDistance(IAgent a, string location,
                double travelDistance)
        {
            agentLocationAndTravelDistance.Put(a, new Pair<string, double>(
                    location, travelDistance));
        }
    }
}
