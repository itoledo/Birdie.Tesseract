namespace tvn.cosine.ai.agent.impl
{ 
    public abstract class AbstractAgent : Agent
    {
        protected AgentProgram program;
        private bool alive = true;

        public AbstractAgent()
        {

        }

        /**
         * Constructs an Agent with the specified AgentProgram.
         * 
         * @param aProgram
         *            the Agent's program, which maps any given percept sequences to an action.
         */
        public AbstractAgent(AgentProgram aProgram)
        {
            program = aProgram;
        }
         
        public Action execute(Percept p)
        {
            if (null != program)
            {
                return program.execute(p);
            }
            return NoOpAction.NO_OP;
        }

        public bool isAlive()
        {
            return alive;
        }

        public void setAlive(bool alive)
        {
            this.alive = alive;
        } 
    }
}
