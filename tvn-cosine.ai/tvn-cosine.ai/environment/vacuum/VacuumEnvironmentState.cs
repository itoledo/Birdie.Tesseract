using System.Text;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;

namespace tvn.cosine.ai.environment.vacuum
{
    /**
     * Represents a state in the Vacuum World
     * 
     * @author Ciaran O'Reilly
     * @author Andrew Brown
     * @author Ruediger Lunde
     */
    public class VacuumEnvironmentState : IEnvironmentState, IPercept,
        ICloneable<VacuumEnvironmentState>,
        IEquatable, IStringable, IHashable
    {
        private IMap<string, VacuumEnvironment.LocationState> state;
        private IMap<IAgent, string> agentLocations;

        /**
         * Constructor
         */
        public VacuumEnvironmentState()
        {
            state = Factory.CreateInsertionOrderedMap<string, VacuumEnvironment.LocationState>();
            agentLocations = Factory.CreateInsertionOrderedMap<IAgent, string>();
        }

        public string getAgentLocation(IAgent a)
        {
            return agentLocations.Get(a);
        }

        /**
         * Sets the agent location
         */
        public void setAgentLocation(IAgent a, string location)
        {
            agentLocations.Put(a, location);
        }

        public VacuumEnvironment.LocationState getLocationState(string location)
        {
            return state.Get(location);
        }

        /**
         * Sets the location state
         */
        public void setLocationState(string location, VacuumEnvironment.LocationState s)
        {
            state.Put(location, s);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && GetType() == obj.GetType())
            {
                VacuumEnvironmentState s = (VacuumEnvironmentState)obj;
                return state.Equals(s.state)
                    && agentLocations.Equals(s.agentLocations);
            }
            return false;
        }

        /**
         * Override hashCode()
         * 
         * @return the hash code for this object.
         */
        public override int GetHashCode()
        {
            return 3 * state.GetHashCode() + 13 * agentLocations.GetHashCode();
        }

        public VacuumEnvironmentState clone()
        {
            VacuumEnvironmentState result = null;

            result = new VacuumEnvironmentState();
            result.state = Factory.CreateMap<string, VacuumEnvironment.LocationState>(state);
            agentLocations = Factory.CreateMap<IAgent, string>(agentLocations);

            return result;
        }

        /**
         * Returns a string representation of the environment
         * 
         * @return a string representation of the environment
         */
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("{");
            foreach (KeyValuePair<string, VacuumEnvironment.LocationState> entity in state)
            {
                if (builder.Length > 2) builder.Append(", ");
                builder.Append(entity.GetKey()).Append("=").Append(entity.GetValue());
            }
            int i = 0;
            foreach (KeyValuePair<IAgent, string> entity in agentLocations)
            {
                if (builder.Length  > 2) builder.Append(", ");
                builder.Append("Loc").Append(++i).Append("=").Append(entity.GetValue());
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
