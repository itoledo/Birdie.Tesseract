using System.Collections.Generic;
using tvn.cosine.ai.agent.impl.aprog.simplerule;

namespace tvn.cosine.ai.agent.impl.aprog
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.12, page 51. <para />
    ///    
    /// Figure 2.12 A model-based reflex agent. It keeps track of the current state
    /// of the world using an internal model. It then chooses an action in the same
    /// way as the reflex agent.
    /// </summary>
    public abstract class ModelBasedReflexAgentProgram : IAgentProgram
    {
        /// <summary>
        /// the agent's current conception of the world state
        /// </summary>
        private DynamicState state = null;

        /// <summary>
        /// a description of how the next state depends on current state and action
        /// </summary>
        private IModel model = null;

        /// <summary>
        /// a set of condition-action rules
        /// </summary>
        private ISet<Rule> rules = null;

        /// <summary>
        /// the most recent action, initially none
        /// </summary>
        private IAction action = null;

        public ModelBasedReflexAgentProgram()
        {
            Init();
        }
         
        /// <summary>
        /// Set the agent's current conception of the world state.
        /// </summary>
        /// <param name="state">the agent's current conception of the world state.</param>
        public void setState(DynamicState state)
        {
            this.state = state;
        }

        /// <summary>
        /// Set the program's description of how the next state depends on the state and action.
        /// </summary>
        /// <param name="model">a description of how the next state depends on the current state and action.</param>
        public void setModel(IModel model)
        {
            this.model = model;
        }
         
        /// <summary>
        /// Set the program's condition-action rules
        /// </summary>
        /// <param name="ruleSet">a set of condition-action rules</param>
        public void setRules(ISet<Rule> ruleSet)
        {
            rules = ruleSet;
        }
         
        /// <summary>
        /// MODEL-BASED-REFLEX-AGENT(percept)
        /// </summary>
        /// <param name="percept"></param>
        /// <returns>an action</returns>
        public IAction Execute(IPercept percept)
        {
            // state <- UPDATE-STATE(state, action, percept, model)
            state = updateState(state, action, percept, model);
            // rule <- RULE-MATCH(state, rules)
            Rule rule = ruleMatch(state, rules);
            // action <- rule.ACTION
            action = ruleAction(rule);
            // return action
            return action;
        }

        /// <summary> 
        /// Realizations of this class should implement the init() method so that it
        /// calls the setState(), setModel(), and setRules() method.
        /// </summary>
        protected abstract void Init();

        protected abstract DynamicState updateState(DynamicState state, IAction action, IPercept percept, IModel model);

        protected Rule ruleMatch(DynamicState state, ISet<Rule> rules)
        {
            foreach (Rule r in rules)
            {
                if (r.evaluate(state))
                {
                    return r;
                }
            }
            return null;
        }

        protected IAction ruleAction(Rule r)
        {
            return null == r ? DynamicAction.NO_OP : r.getAction();
        }
    } 
}
