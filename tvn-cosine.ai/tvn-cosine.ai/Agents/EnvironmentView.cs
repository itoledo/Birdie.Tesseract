using System;
using System.Linq;

namespace tvn_cosine.ai.Agents
{
    /// <summary>
    /// Simple environment view which uses the standard output stream to inform about
    /// relevant events.
    /// </summary>
    public class EnvironmentView : IEnvironmentView
    {
        public virtual void AgentActed(IAgent agent, IPercept percept, IAction action, IEnvironment source)
        {
            var agents = source.Agents;

            for (int index = 1; index <= agents.Count; ++index)
            {
                if (agents.ElementAt(index) == agent)
                {
                    Console.WriteLine("Agent {0} added.", index);
                    Console.WriteLine("Percept: {0}", percept);
                    Console.WriteLine("Action: {0}", action);
                    break;
                }
            }

        }

        public virtual void AgentAdded(IAgent agent, IEnvironment source)
        {
            var agents = source.Agents;
            for (int index = 1; index <= agents.Count; ++index)
            {
                if (agents.ElementAt(index) == agent)
                {
                    Console.WriteLine("Agent {0} added.", index);
                    break;
                }
            }
        }

        public virtual void Notify(string message)
        {
            Console.WriteLine("Message: {0}", message);
        }
    }
}
