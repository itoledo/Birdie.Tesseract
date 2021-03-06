﻿using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.agentprogram;
using tvn.cosine.ai.agent.agentprogram.simplerule;
using tvn.cosine.collections;
using tvn.cosine.collections.api;

namespace tvn.cosine.ai.environment.vacuum
{
    public class SimpleReflexVacuumAgent : DynamicAgent
    { 
        public SimpleReflexVacuumAgent()
            : base(new SimpleReflexAgentProgram(getRuleSet()))
        {  }

        //
        // PRIVATE METHODS
        //
        private static ISet<Rule> getRuleSet()
        {
            // Note: Using a LinkedHashSet so that the iteration order (i.e. implied
            // precedence) of rules can be guaranteed.
            ISet<Rule> rules = CollectionFactory.CreateSet<Rule>();

            // Rules based on REFLEX-VACUUM-AGENT:
            // Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.8,
            // page 48.

            rules.Add(new Rule(new EQUALCondition(LocalVacuumEnvironmentPercept.ATTRIBUTE_STATE,
                    VacuumEnvironment.LocationState.Dirty),
                    VacuumEnvironment.ACTION_SUCK));
            rules.Add(new Rule(new EQUALCondition(
                    LocalVacuumEnvironmentPercept.ATTRIBUTE_AGENT_LOCATION,
                    VacuumEnvironment.LOCATION_A),
                    VacuumEnvironment.ACTION_MOVE_RIGHT));
            rules.Add(new Rule(new EQUALCondition(
                    LocalVacuumEnvironmentPercept.ATTRIBUTE_AGENT_LOCATION,
                    VacuumEnvironment.LOCATION_B),
                    VacuumEnvironment.ACTION_MOVE_LEFT));

            return rules;
        }
    }
}
