using System.Collections.Generic;

namespace tvn_cosine.ai.Agents.AgentPrograms
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.12, page 51.
    /// 
    /// <pre>
    /// function MODEL-BASED-REFLEX-AGENT(percept) returns an action
    /// persistent: state, the agent's current conception of the world state
    /// model, a description of how the next state depends on current state and action
    /// rules, a set of condition-action rules
    /// action, the most recent action, initially none
    ///
    /// state&lt;- UPDATE-STATE(state, action, percept, model)
    /// rule&lt;- RULE-MATCH(state, rules)
    /// action&lt;- rule.ACTION
    ///   return action
    /// </pre>
    /// 
    /// Figure 2.12 A model-based reflex agent.It keeps track of the current state
    /// of the world using an internal model.It then chooses an action in the same
    /// way as the reflex agent.
    /// </summary>
    public abstract class ModelBasedReflexAgentProgramBase : IAgentProgram
    {
        private readonly ISet<IRule> rules;
        private IAction action;

        public ModelBasedReflexAgentProgramBase()
        {
            rules = new HashSet<IRule>();
            Initialise();
        }

        public IModel Model { get; set; }
        public IState State { get; set; }

        public ISet<IRule> Rules
        {
            get
            {
                return rules;
            }
            set
            {
                if (value != rules)
                {
                    rules.Clear();
                    foreach (var rule in value)
                    {
                        rules.Add(rule);
                    }
                }
            }
        }

        /// <summary>
        /// UPDATE-STATE(state, action, percept, model)
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <param name="percept"></param>
        /// <param name="model"></param>
        /// <returns>returns a state.</returns>
        protected abstract IState UpdateState(IState state, IAction action, IPercept percept, IModel model);

        protected abstract void Initialise();

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

        /// <summary>
        /// rule.ACTION
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        protected virtual IAction RuleAction(IRule rule)
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
        /// MODEL-BASED-REFLEX-AGENT(percept)
        /// </summary>
        /// <param name="percept"></param>
        /// <returns>returns an action</returns>
        public IAction Execute(IPercept percept)
        {
            // state <- UPDATE-STATE(state, action, percept, model)
            State = UpdateState(State, action, percept, Model);

            // rule <- RULE-MATCH(state, rules)
            var rule = RuleMatch(State, rules);

            // action <- rule.ACTION
            action = RuleAction(rule);

            // return action
            return action;
        }
    }
}
