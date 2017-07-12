using System;
using System.Text;

namespace tvn.cosine.ai.agent.impl
{
    /**
     * Simple environment view which uses the standard output stream to inform about
     * relevant events.
     * 
     * @author Ruediger Lunde
     */
    public class SimpleEnvironmentView : IEnvironmentView
    {

        public void Notify(string msg)
        {
            Console.WriteLine("Message: " + msg);
        }

        public void AgentAdded(IAgent agent, IEnvironment source)
        {
            int agentId = source.GetAgents().IndexOf(agent) + 1;
            Console.WriteLine("Agent " + agentId + " added.");
        }

        public void AgentActed(IAgent agent, IPercept percept, IAction action, IEnvironment source)
        {
            StringBuilder builder = new StringBuilder();
            int agentId = source.GetAgents().IndexOf(agent) + 1;
            builder.Append("Agent ").Append(agentId).Append(" acted.");
            builder.Append("\n   Percept: ").Append(percept.ToString());
            builder.Append("\n   Action: ").Append(action.ToString());
            Console.WriteLine(builder.ToString());
        }
    }
}
