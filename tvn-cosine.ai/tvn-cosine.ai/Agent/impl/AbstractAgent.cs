﻿namespace tvn.cosine.ai.agent.impl
{ 
    public abstract class AbstractAgent : IAgent
    {
        protected IAgentProgram program;
        private bool alive = true;

        public AbstractAgent()
        { }

        /**
         * Constructs an Agent with the specified AgentProgram.
         * 
         * @param aProgram
         *            the Agent's program, which maps any given percept sequences to an action.
         */
        public AbstractAgent(IAgentProgram aProgram)
        {
            program = aProgram;
        }
         
        public virtual IAction Execute(IPercept p)
        {
            if (null != program)
            {
                return program.Execute(p);
            }
            return NoOpAction.NO_OP;
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
