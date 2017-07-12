using System;
using System.Collections.Generic;
using System.Text;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.vacuum
{
    /// <summary>
    /// Represents a state in the Vacuum World
    /// </summary>
    public class VacuumEnvironmentState : IEnvironmentState, FullyObservableVacuumEnvironmentPercept, ICloneable
    {
        private IDictionary<string, VacuumEnvironment.LocationState> state;
        private IDictionary<IAgent, string> agentLocations;

        public VacuumEnvironmentState()
        {
            state = new Dictionary<string, VacuumEnvironment.LocationState>();
            agentLocations = new Dictionary<IAgent, string>();
        }

        public VacuumEnvironmentState(VacuumEnvironment.LocationState locAState, VacuumEnvironment.LocationState locBState)
            : this()
        {

            state[VacuumEnvironment.LOCATION_A] = locAState;
            state[VacuumEnvironment.LOCATION_B] = locBState;
        }

        public string getAgentLocation(IAgent a)
        {
            return agentLocations[a];
        }

        /// <summary>
        /// Sets the agent location
        /// </summary>
        /// <param name="a"></param>
        /// <param name="location"></param>
        public void setAgentLocation(IAgent a, string location)
        {
            agentLocations[a] = location;
        }

        public VacuumEnvironment.LocationState getLocationState(string location)
        {
            return state[location];
        }

        /// <summary>
        /// Sets the location state
        /// </summary>
        /// <param name="location"></param>
        /// <param name="s"></param>
        public void setLocationState(string location, VacuumEnvironment.LocationState s)
        {
            state[location] = s;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && GetType() == obj.GetType())
            {
                VacuumEnvironmentState s = (VacuumEnvironmentState)obj;
                if (state.Count == s.state.Count
                 && agentLocations.Count == s.agentLocations.Count)
                {
                    foreach (var key in state.Keys)
                    {
                        if (!(s.state.ContainsKey(key)
                           && s.state[key] == state[key]))
                        {
                            return false;
                        }
                    }

                    foreach (var key in agentLocations.Keys)
                    {
                        if (!(s.agentLocations.ContainsKey(key)
                           && s.agentLocations[key] == agentLocations[key]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 3 * state.GetHashCode() + 13 * agentLocations.GetHashCode();
        }

        public object Clone()
        {
            VacuumEnvironmentState result = new VacuumEnvironmentState();
            foreach (var v in state)
            {
                result.state[v.Key] = v.Value;
            }
            foreach (var v in agentLocations)
            {
                result.agentLocations[v.Key] = v.Value;
            }

            return result;
        }


        /// <summary>
        /// Returns a string representation of the environment
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('{');
            bool first = true;
            foreach (var v in state)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    stringBuilder.Append(", ");
                }
                stringBuilder.Append(v.Key);
                stringBuilder.Append('=');
                stringBuilder.Append(v.Value);
            }
            stringBuilder.Append('}');

            return stringBuilder.ToString();
        }
    }
}
