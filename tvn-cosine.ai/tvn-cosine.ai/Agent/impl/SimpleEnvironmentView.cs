namespace tvn.cosine.ai.agent.impl
{
    /**
     * Simple environment view which uses the standard output stream to inform about
     * relevant events.
     * 
     * @author Ruediger Lunde
     */
    public class SimpleEnvironmentView : EnvironmentView
    {
        public void notify(string msg)
        {
            System.Console.WriteLine("Message: " + msg);
        }

        public void agentAdded(Agent agent, Environment source)
        {
            int agentId = source.getAgents().IndexOf(agent) + 1;
            System.Console.WriteLine("Agent " + agentId + " added.");
        }

        public void agentActed(Agent agent, Percept percept, Action action, Environment source)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            int agentId = source.getAgents().IndexOf(agent) + 1;
            builder.Append("Agent ").Append(agentId).Append(" acted.");
            builder.Append("\n   Percept: ").Append(percept.ToString());
            builder.Append("\n   Action: ").Append(action.ToString());
            System.Console.WriteLine(builder);
        }
    } 
}
