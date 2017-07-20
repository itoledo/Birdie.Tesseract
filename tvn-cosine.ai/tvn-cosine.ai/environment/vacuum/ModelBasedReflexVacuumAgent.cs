using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.agent.impl.aprog;
using tvn.cosine.ai.agent.impl.aprog.simplerule;
using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.environment.vacuum
{
    public class ModelBasedReflexVacuumAgent : AbstractAgent
    {
        class ModelBasedReflexVacuumAgentProgram : ModelBasedReflexAgentProgram
        {
            protected override void init()
            {
                setState(new DynamicState());
                setRules(getRuleSet());
            }

            protected override DynamicState updateState(DynamicState state,
                    IAction anAction, IPercept percept, IModel model)
            {

                LocalVacuumEnvironmentPercept vep = (LocalVacuumEnvironmentPercept)percept;

                state.setAttribute(ATTRIBUTE_CURRENT_LOCATION,
                        vep.getAgentLocation());
                state.setAttribute(ATTRIBUTE_CURRENT_STATE,
                        vep.getLocationState());
                // Keep track of the state of the different locations
                if (VacuumEnvironment.LOCATION_A.Equals(vep.getAgentLocation()))
                {
                    state.setAttribute(ATTRIBUTE_STATE_LOCATION_A,
                            vep.getLocationState());
                }
                else
                {
                    state.setAttribute(ATTRIBUTE_STATE_LOCATION_B,
                            vep.getLocationState());
                }
                return state;
            }
        }

        private const string ATTRIBUTE_CURRENT_LOCATION = "currentLocation";
        private const string ATTRIBUTE_CURRENT_STATE = "currentState";
        private const string ATTRIBUTE_STATE_LOCATION_A = "stateLocationA";
        private const string ATTRIBUTE_STATE_LOCATION_B = "stateLocationB";

        public ModelBasedReflexVacuumAgent()
            : base(new ModelBasedReflexVacuumAgentProgram())
        {

        }

        //
        // PRIVATE METHODS
        //
        private static ISet<Rule> getRuleSet()
        {
            // Note: Using a LinkedHashSet so that the iteration order (i.e. implied
            // precedence) of rules can be guaranteed.
            ISet<Rule> rules = Factory.CreateSet<Rule>();

            rules.Add(new Rule(new ANDCondition(new EQUALCondition(
                    ATTRIBUTE_STATE_LOCATION_A,
                    VacuumEnvironment.LocationState.Clean), new EQUALCondition(
                    ATTRIBUTE_STATE_LOCATION_B,
                    VacuumEnvironment.LocationState.Clean)), NoOpAction.NO_OP));
            rules.Add(new Rule(new EQUALCondition(ATTRIBUTE_CURRENT_STATE,
                    VacuumEnvironment.LocationState.Dirty),
                    VacuumEnvironment.ACTION_SUCK));
            rules.Add(new Rule(new EQUALCondition(ATTRIBUTE_CURRENT_LOCATION,
                    VacuumEnvironment.LOCATION_A),
                    VacuumEnvironment.ACTION_MOVE_RIGHT));
            rules.Add(new Rule(new EQUALCondition(ATTRIBUTE_CURRENT_LOCATION,
                    VacuumEnvironment.LOCATION_B),
                    VacuumEnvironment.ACTION_MOVE_LEFT));

            return rules;
        }
    }
}
