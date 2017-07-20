using System.Text;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.environment.vacuum
{
    public class VacuumEnvironmentViewActionTracker : IEnvironmentView
    {

        private StringBuilder actions = null;

        public VacuumEnvironmentViewActionTracker(StringBuilder envChanges)
        {
            this.actions = envChanges;
        }

        //
        // START-EnvironmentView
        public void Notify(string msg)
        {
            // Do nothing by default.
        }

        public void AgentAdded(IAgent agent, IEnvironment source)
        {
            // Do nothing by default.
        }

        public void AgentActed(IAgent agent, IPercept percept, IAction action, IEnvironment source)
        {
            actions.Append(action);
        }

        // END-EnvironmentView
        //
    }

}
