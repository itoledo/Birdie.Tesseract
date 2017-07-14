using System.Collections.Generic;
using tvn.cosine.ai.agent.impl.aprog.simplerule;

namespace tvn.cosine.ai.agent.impl.aprog
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.10, page 49. <para />
    /// 
    /// Figure 2.10 A simple reflex agent. It acts according to a rule whose
    /// condition matches the current state, as defined by the percept. 
    /// </summary>
    public class SimpleReflexAgentProgram : AgentProgram
    {
        /// <summary>
        /// a set of condition-action rules
        /// </summary>
        private ISet<Rule> rules;
         
        /// <summary>
        /// Constructs a SimpleReflexAgentProgram with a set of condition-action rules.
        /// </summary>
        /// <param name="ruleSet">a set of condition-action rules</param>
        public SimpleReflexAgentProgram(ISet<Rule> ruleSet)
        {
            rules = ruleSet;
        }
         
        /// <summary>
        /// SIMPLE-RELEX-AGENT(percept)
        /// </summary>
        /// <param name="percept"></param>
        /// <returns>an action</returns> 
        public Action execute(Percept percept)
        { 
            // state <- INTERPRET-INPUT(percept);
            ObjectWithDynamicAttributes<string, object> state = interpretInput(percept);
            // rule <- RULE-MATCH(state, rules);
            Rule rule = ruleMatch(state, rules);
            // action <- rule.ACTION;
            // return action
            return ruleAction(rule);
        }
         
        protected ObjectWithDynamicAttributes<string, object> interpretInput(Percept p)
        {
            return (DynamicPercept)p;
        }

        protected Rule ruleMatch(ObjectWithDynamicAttributes<string, object> state, ISet<Rule> rulesSet)
        {
            foreach (Rule r in rulesSet)
            {
                if (r.evaluate(state))
                {
                    return r;
                }
            }
            return null;
        }

        protected Action ruleAction(Rule r)
        {
            return null == r ? DynamicAction.NO_OP : r.getAction();
        }
    } 
}
