namespace tvn.cosine.ai.agent.impl
{
    public abstract class AbstractAgent : IAgent
    {
        protected IAgentProgram program;
        private bool alive = true;

        public AbstractAgent()
        {

        }

        /**
         * Constructs an Agent with the specified AgentProgram.
         * 
         * @param aProgram
         *            the Agent's program, which maps any given percept sequences to
         *            an action.
         */
        public AbstractAgent(IAgentProgram aProgram)
        {
            AgentProgram = aProgram;
        }

        public IAgentProgram AgentProgram { get; }

        public bool Alive { get; set; }

        public virtual IAction Execute(IPercept percept)
        {
            if (null != program)
            {
                return program.Execute(percept);
            }
            return NoOpAction.NO_OP;
        }
          
        // END-Agent
        //
    }
}
