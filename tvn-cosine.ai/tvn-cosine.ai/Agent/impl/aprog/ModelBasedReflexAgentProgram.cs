using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent.impl.aprog.simplerule;

namespace tvn.cosine.ai.agent.impl.aprog
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.12, page
     * 51.<br>
     * <br>
     * 
     * <pre>
     * function MODEL-BASED-REFLEX-AGENT(percept) returns an action
     *   persistent: state, the agent's current conception of the world state
     *               model, a description of how the next state depends on current state and action
     *               rules, a set of condition-action rules
     *               action, the most recent action, initially none
     *               
     *   state  <- UPDATE-STATE(state, action, percept, model)
     *   rule   <- RULE-MATCH(state, rules)
     *   action <- rule.ACTION
     *   return action
     * </pre>
     * 
     * Figure 2.12 A model-based reflex agent. It keeps track of the current state
     * of the world using an internal model. It then chooses an action in the same
     * way as the reflex agent.
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * 
     */
    public abstract class ModelBasedReflexAgentProgram : AgentProgram
    {
        //
        // persistent: state, the agent's current conception of the world state
        private DynamicState state = null;

        // model, a description of how the next state depends on current state and
        // action
        private Model model = null;

        // rules, a set of condition-action rules
        private ISet<Rule> rules = null;

        // action, the most recent action, initially none
        private Action action = null;

        public ModelBasedReflexAgentProgram()
        {
            init();
        }

        /**
         * Set the agent's current conception of the world state.
         * 
         * @param state
         *            the agent's current conception of the world state.
         */
        public void setState(DynamicState state)
        {
            this.state = state;
        }

        /**
         * Set the program's description of how the next state depends on the state
         * and action.
         * 
         * @param model
         *            a description of how the next state depends on the current
         *            state and action.
         */
        public void setModel(Model model)
        {
            this.model = model;
        }

        /**
         * Set the program's condition-action rules
         * 
         * @param ruleSet
         *            a set of condition-action rules
         */
        public void setRules(ISet<Rule> ruleSet)
        {
            rules = ruleSet;
        }

        //
        // START-AgentProgram

        // function MODEL-BASED-REFLEX-AGENT(percept) returns an action
        public Action execute(Percept percept)
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

        // END-AgentProgram
        //

        //
        // PROTECTED METHODS
        //

        /**
         * Realizations of this class should implement the init() method so that it
         * calls the setState(), setModel(), and setRules() method.
         */
        protected abstract void init();

        protected abstract DynamicState updateState(DynamicState state,
                Action action, Percept percept, Model model);

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

        protected Action ruleAction(Rule r)
        {
            return null == r ? NoOpAction.NO_OP : r.getAction();
        }
    }

}
