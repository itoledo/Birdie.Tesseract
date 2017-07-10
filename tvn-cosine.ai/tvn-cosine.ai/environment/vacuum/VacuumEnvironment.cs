using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;

namespace tvn.cosine.ai.environment.vacuum
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 58.<br>
     * <br>
     * Let the world contain just two locations. Each location may or may not
     * contain dirt, and the agent may be in one location or the other. There are 8
     * possible world states, as shown in Figure 3.2. The agent has three possible
     * actions in this version of the vacuum world: <em>Left</em>, <em>Right</em>,
     * and <em>Suck</em>. Assume for the moment, that sucking is 100% effective. The
     * goal is to clean up all the dirt.
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class VacuumEnvironment : AbstractEnvironment
    {
        // Allowable Actions within the Vacuum Environment
        public static readonly Action ACTION_MOVE_LEFT = new DynamicAction("Left");
        public static readonly Action ACTION_MOVE_RIGHT = new DynamicAction("Right");
        public static readonly Action ACTION_SUCK = new DynamicAction("Suck");
        public const string LOCATION_A = "A";
        public const string LOCATION_B = "B";

        public enum LocationState
        {
            Clean, Dirty
        }

        //
        protected VacuumEnvironmentState envState = null;
        protected bool _isDone = false;

        /**
         * Constructs a vacuum environment with two locations, in which dirt is
         * placed at random.
         */
        public VacuumEnvironment()
        {
            System.Random r = new System.Random();
            envState = new VacuumEnvironmentState(
                    0 == r.Next(2) ? LocationState.Clean : LocationState.Dirty,
                    0 == r.Next(2) ? LocationState.Clean : LocationState.Dirty);
        }

        /**
         * Constructs a vacuum environment with two locations, in which dirt is
         * placed as specified.
         * 
         * @param locAState
         *            the initial state of location A, which is either
         *            <em>Clean</em> or <em>Dirty</em>.
         * @param locBState
         *            the initial state of location B, which is either
         *            <em>Clean</em> or <em>Dirty</em>.
         */
        public VacuumEnvironment(LocationState locAState, LocationState locBState)
        {
            envState = new VacuumEnvironmentState(locAState, locBState);
        }

        public EnvironmentState getCurrentState()
        {
            return envState;
        }

        public IList<string> getLocations()
        {
            return new[] { LOCATION_A, LOCATION_B }.ToList();
        }

        public override void executeAction(Agent a, Action agentAction)
        {

            if (ACTION_MOVE_RIGHT == agentAction)
            {
                envState.setAgentLocation(a, LOCATION_B);
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_MOVE_LEFT == agentAction)
            {
                envState.setAgentLocation(a, LOCATION_A);
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_SUCK == agentAction)
            {
                if (LocationState.Dirty == envState.getLocationState(envState
                        .getAgentLocation(a)))
                {
                    envState.setLocationState(envState.getAgentLocation(a),
                            LocationState.Clean);
                    updatePerformanceMeasure(a, 10);
                }
            }
            else if (agentAction.isNoOp())
            {
                // In the Vacuum Environment we consider things done if
                // the agent generates a NoOp.
                _isDone = true;
            }
        }

        public override Percept getPerceptSeenBy(Agent anAgent)
        {
            if (anAgent is NondeterministicVacuumAgent)
            {
                // Note: implements FullyObservableVacuumEnvironmentPercept
                return envState.Clone();
            }
            string agentLocation = envState.getAgentLocation(anAgent);
            return new LocalVacuumEnvironmentPercept(agentLocation,
                    envState.getLocationState(agentLocation));
        }

        public override bool isDone()
        {
            return base.isDone() || _isDone;
        }

        public override void addAgent(Agent a)
        {
            int idx = new System.Random().Next(2);
            envState.setAgentLocation(a, idx == 0 ? LOCATION_A : LOCATION_B);
            base.addAgent(a);
        }

        public void addAgent(Agent a, string location)
        {
            // Ensure the agent state information is tracked before
            // adding to super, as super will notify the registered
            // EnvironmentViews that is was added.
            envState.setAgentLocation(a, location);
            base.addAgent(a);
        }

        public LocationState getLocationState(string location)
        {
            return envState.getLocationState(location);
        }

        public string getAgentLocation(Agent a)
        {
            return envState.getAgentLocation(a);
        }
    }
}
