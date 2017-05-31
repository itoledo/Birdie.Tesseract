using System;
using System.Collections.Generic;

namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach(3rd Edition): Figure 2.10, page 49.
    /// <br />
    ///
    /// <code>
    /// function SIMPLE-RELEX-AGENT(percept) returns an action
    /// persistent: rules, a set of condition-action rules
    ///
    /// state&lt;- INTERPRET-INPUT(percept);
    /// rule&lt;- RULE-MATCH(state, rules);
    /// action&lt;- rule.ACTION;
    ///   return action
    /// </code>
    /// 
    /// Figure 2.10 A simple reflex agent. It acts according to a rule whose
    /// condition matches the current state, as defined by the percept.
    /// </summary>
    public class ReflexAgentProgram : IAgentProgram
    {
        // persistent: rules, a set of condition-action rules
        private readonly ISet<IRule> rules;
        private readonly IStateInterpreter<IPercept> stateInterpreter;

        /// <summary>
        /// Constructs a SimpleReflexAgentProgram with a set of condition-action rules.
        /// </summary>
        /// <param name="rules">a set of condition-action rules.</param>
        public ReflexAgentProgram(ISet<IRule> rules,
                                  IStateInterpreter<IPercept> stateInterpreter)
        {
            if (null == stateInterpreter)
            {
                throw new ArgumentNullException("The state interpreter cannot be null.");
            }
            if (null == rules
                || 0 == rules.Count)
            {
                throw new ArgumentNullException("The rules cannot be null or count 0.");
            }

            this.rules = new HashSet<IRule>();
            this.stateInterpreter = stateInterpreter;

            foreach (var rule in rules)
            {
                this.rules.Add(rule);
            }
        }

        /// <summary>
        /// SIMPLE-RELEX-AGENT(percept) 
        /// </summary>
        /// <param name="percept"></param>
        /// <returns>returns an action</returns>
        public IAction Execute(IPercept percept)
        {
            // state <- INTERPRET-INPUT(percept);
            var state = InterpretInput(percept);

            // rule <- RULE-MATCH(state, rules);
            var rule = RuleMatch(state, rules);

            // action <- rule.ACTION;
            var action = RuleAction(rule);

            // return action
            return action;
        }

        /// <summary>
        /// rule.ACTION
        /// </summary>
        /// <param name="rule"></param>
        /// <returns>returns the action, NoOp on null.</returns>
        private IAction RuleAction(IRule rule)
        {
            if (null == rule)
            {
                return Action.NO_OP;
            }
            else
            {
                return rule.Result;
            }
        }

        /// <summary>
        /// INTERPRET-INPUT(percept)
        /// </summary>
        /// <param name="percept"></param>
        /// <returns>returns a state</returns>
        protected virtual IState InterpretInput(IPercept percept)
        {
            return stateInterpreter.Interpret(percept);
        }

        /// <summary>
        /// RULE-MATCH(state, rules)
        /// </summary>
        /// <param name="state"></param>
        /// <param name="rules"></param>
        /// <returns>returns a rule</returns>
        protected virtual IRule RuleMatch(IState state, ISet<IRule> rules)
        {
            foreach (var rule in rules)
            {
                if (rule.Evaluate(state))
                {
                    return rule;
                }
            }
            return null;
        }
    }
}
