namespace tvn.cosine.ai.agent.impl
{ 
    public abstract class AbstractAgent : IAgent
    {
        protected IAgentProgram program;
        private bool alive = true;

        public AbstractAgent()
        { }
         
        /// <summary>
        /// Constructs an Agent with the specified AgentProgram.
        /// </summary>
        /// <param name="aProgram">the Agent's program, which maps any given percept sequences to an action.</param>
        public AbstractAgent(IAgentProgram aProgram)
        {
            program = aProgram;
        }
         
        public virtual IAction Execute(IPercept percept)
        {
            if (null != program)
            {
                return program.Execute(percept);
            }
            return DynamicAction.NO_OP;
        }

        public virtual bool IsAlive()
        {
            return alive;
        }

        public virtual void SetAlive(bool alive)
        {
            this.alive = alive;
        } 
    }
}
