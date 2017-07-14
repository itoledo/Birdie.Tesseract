using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;

namespace tvn.cosine.ai.environment.vacuum
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.8, page 48.<para />
    ///   
    /// Figure 2.8 The agent program for a simple reflex agent in the two-state
    /// vacuum environment. This program implements the action function tabulated in
    /// Figure 2.3.
    /// </summary>
    public class ReflexVacuumAgent : AbstractAgent
    {
        class ReflexVacuumAgentProgram : AgentProgram
        {
            /// <summary>
            /// REFLEX-VACUUM-AGENT([location, status]) returns an action
            /// </summary>
            /// <param name="percept"></param>
            /// <returns>an action</returns>
            public Action execute(Percept percept)
            {
                LocalVacuumEnvironmentPercept vep = (LocalVacuumEnvironmentPercept)percept;

                // if status = Dirty then return Suck
                if (VacuumEnvironment.LocationState.Dirty == vep
                        .getLocationState())
                {
                    return VacuumEnvironment.ACTION_SUCK;
                    // else if location = A then return Right
                }
                else if (VacuumEnvironment.LOCATION_A.Equals(vep.getAgentLocation()))
                {
                    return VacuumEnvironment.ACTION_MOVE_RIGHT;
                }
                else if (VacuumEnvironment.LOCATION_B.Equals(vep.getAgentLocation()))
                {
                    // else if location = B then return Left
                    return VacuumEnvironment.ACTION_MOVE_LEFT;
                }

                // Note: This should not be returned if the
                // environment is correct
                return DynamicAction.NO_OP;
            }
        }

        public ReflexVacuumAgent()
            : base(new ReflexVacuumAgentProgram())
        { }
    }
}
