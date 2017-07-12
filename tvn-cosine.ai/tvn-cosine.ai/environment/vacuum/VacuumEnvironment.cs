using System;
using System.Collections.Generic;
using System.Linq;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;

namespace tvn.cosine.ai.environment.vacuum
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): pg 58. <para />
    /// Let the world contain just two locations. Each location may or may not
    /// contain dirt, and the agent may be in one location or the other. There are 8
    /// possible world states, as shown in Figure 3.2. The agent has three possible
    /// actions in this version of the vacuum world: Left , Right ,
    /// and Suck . Assume for the moment, that sucking is 100% effective. The
    /// goal is to clean up all the dirt.
    /// </summary>
    public class VacuumEnvironment : AbstractEnvironment
    {
        // Allowable Actions within the Vacuum Environment
        public static readonly IAction ACTION_MOVE_LEFT = new DynamicAction("Left");
        public static readonly IAction ACTION_MOVE_RIGHT = new DynamicAction("Right");
        public static readonly IAction ACTION_SUCK = new DynamicAction("Suck");
        public const string LOCATION_A = "A";
        public const string LOCATION_B = "B";
        private readonly Random random = new Random();

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
            Random r = new Random();
            envState = new VacuumEnvironmentState(
                    0 == r.Next(2) ? LocationState.Clean : LocationState.Dirty,
                    0 == r.Next(2) ? LocationState.Clean : LocationState.Dirty);
        }

        /// <summary>
        /// Constructs a vacuum environment with two locations, in which dirt is placed as specified.
        /// </summary>
        /// <param name="locAState">the initial state of location A, which is either Clean or Dirty.</param>
        /// <param name="locBState">the initial state of location B, which is either Clean or Dirty.</param>
        public VacuumEnvironment(LocationState locAState, LocationState locBState)
        {
            envState = new VacuumEnvironmentState(locAState, locBState);
        }

        public IEnvironmentState getCurrentState()
        {
            return envState;
        }

        public IList<string> getLocations()
        {
            return new[] { LOCATION_A, LOCATION_B }.ToList();
        }

        public override void executeAction(IAgent a, IAction agentAction)
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
            else if (agentAction.IsNoOp())
            {
                // In the Vacuum Environment we consider things done if
                // the agent generates a NoOp.
                _isDone = true;
            }
        }

        public override IPercept getPerceptSeenBy(IAgent anAgent)
        {
            if (anAgent is NondeterministicVacuumAgent<VacuumEnvironmentState, IAction>)
            {
                // Note: implements FullyObservableVacuumEnvironmentPercept
                return (VacuumEnvironmentState)envState.Clone();
            }
            string agentLocation = envState.getAgentLocation(anAgent);
            return new LocalVacuumEnvironmentPercept(agentLocation, envState.getLocationState(agentLocation));
        }

        public override bool IsDone()
        {
            return base.IsDone() || _isDone;
        }

        public override void AddAgent(IAgent a)
        {
            int idx = random.Next(2);
            envState.setAgentLocation(a, idx == 0 ? LOCATION_A : LOCATION_B);
            base.AddAgent(a);
        }

        public void AddAgent(IAgent a, string location)
        {
            // Ensure the agent state information is tracked before
            // adding to super, as super will notify the registered
            // EnvironmentViews that is was added.
            envState.setAgentLocation(a, location);
            base.AddAgent(a);
        }

        public LocationState getLocationState(string location)
        {
            return envState.getLocationState(location);
        }

        public string getAgentLocation(IAgent a)
        {
            return envState.getAgentLocation(a);
        }
    }
}
