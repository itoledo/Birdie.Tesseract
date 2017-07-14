namespace tvn.cosine.ai.agent.impl
{ 
    public abstract class AbstractAgent : Agent
    {
        protected AgentProgram program;
        private bool alive = true;

        public AbstractAgent()
        { }
         
        /// <summary>
        /// Constructs an Agent with the specified AgentProgram.
        /// </summary>
        /// <param name="aProgram">the Agent's program, which maps any given percept sequences to an action.</param>
        public AbstractAgent(AgentProgram aProgram)
        {
            program = aProgram;
        }
         
        public virtual Action execute(Percept percept)
        {
            if (null != program)
            {
                return program.execute(percept);
            }
            return DynamicAction.NO_OP;
        }

        public virtual bool isAlive()
        {
            return alive;
        }

        public virtual void setAlive(bool alive)
        {
            this.alive = alive;
        } 
    }
}
