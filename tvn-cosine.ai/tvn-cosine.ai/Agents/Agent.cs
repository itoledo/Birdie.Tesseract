namespace tvn_cosine.ai.Agents
{
    public class Agent : IAgent
    {
        /// <summary>
        /// Constructs an Agent with the specified AgentProgram.
        /// </summary>
        /// <param name="agentProgram">the Agent's program, which maps any given percept sequences to an action.</param>
        public Agent(IAgentProgram agentProgram)
        {
            this.AgentProgram = agentProgram;
        }

        public IAgentProgram AgentProgram { get; }
        public bool IsAlive { get; set; }

        public IAction Execute(IPercept percept)
        {
            if (null != AgentProgram)
            {
                return AgentProgram.Execute(percept);
            }
            else
            {
                return Action.NO_OP;
            }
        }
    }
}
