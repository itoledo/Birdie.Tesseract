using System.Collections.Generic;
using System.Linq;

namespace tvn.cosine.ai.agent.impl
{
    public abstract class AbstractEnvironment : IEnvironment
    {
        protected readonly ISet<IEnvironmentObject> envObjects;
        protected readonly ISet<IAgent> agents;
        protected readonly ISet<IEnvironmentView> views;
        protected readonly IDictionary<IAgent, double> performanceMeasures;

        public AbstractEnvironment()
        {
            envObjects = new HashSet<IEnvironmentObject>();
            agents = new HashSet<IAgent>();
            views = new HashSet<IEnvironmentView>();
            performanceMeasures = new Dictionary<IAgent, double>();
        }

        public abstract void executeAction(IAgent agent, IAction action);
        public abstract IPercept getPerceptSeenBy(IAgent anAgent);

        /// <summary>
        /// Method for implementing dynamic environments in which not all changes are
        /// directly caused by agent action execution. The default implementation
        /// does nothing.
        /// </summary>
        public void createExogenousChange()
        { }

        /// <summary>
        /// Return as a List but also ensures the caller cannot modify
        /// </summary>
        /// <returns></returns>
        public virtual IList<IAgent> GetAgents()
        {
            return agents.ToList().AsReadOnly();
        }

        public virtual void AddAgent(IAgent agent)
        {
            AddEnvironmentObject(agent);
        }

        public virtual void RemoveAgent(IAgent agent)
        {
            RemoveEnvironmentObject(agent);
        }

        /// <summary>
        /// Return as a List but also ensures the caller cannot modify
        /// </summary>
        /// <returns></returns>
        public virtual IList<IEnvironmentObject> GetEnvironmentObjects()
        {
            return envObjects.ToList().AsReadOnly();
        }

        public virtual void AddEnvironmentObject(IEnvironmentObject environmentObject)
        {
            envObjects.Add(environmentObject);
            if (environmentObject is IAgent)
            {
                IAgent a = (IAgent)environmentObject;
                if (!agents.Contains(a))
                {
                    agents.Add(a);
                    this.notifyEnvironmentViews(a);
                }
            }
        }

        public virtual void RemoveEnvironmentObject(IEnvironmentObject environmentObject)
        {
            envObjects.Remove(environmentObject);
            if (environmentObject is IAgent)
            {
                agents.Remove(environmentObject as IAgent);
            }
        }

        /// <summary>
        /// Central template method for controlling agent simulation. The concrete
        /// behavior is determined by the primitive operations
        /// #getPerceptSeenBy(Agent), #executeAction(Agent, Action),
        /// and #createExogenousChange().
        /// </summary>
        public virtual void Step()
        {
            foreach (IAgent agent in agents)
            {
                if (agent.IsAlive())
                {
                    IPercept percept = getPerceptSeenBy(agent);
                    IAction anAction = agent.Execute(percept);
                    executeAction(agent, anAction);
                    notifyEnvironmentViews(agent, percept, anAction);
                }
            }
            createExogenousChange();
        }

        public virtual void Step(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Step();
            }
        }

        public virtual void StepUntilDone()
        {
            while (!IsDone())
            {
                Step();
            }
        }

        public virtual bool IsDone()
        {
            foreach (IAgent agent in agents)
            {
                if (agent.IsAlive())
                {
                    return false;
                }
            }
            return true;
        }

        public virtual double GetPerformanceMeasure(IAgent agent)
        {
            if (performanceMeasures.ContainsKey(agent))
            {
                performanceMeasures.Add(agent, 0);
            }

            return performanceMeasures[agent];
        }

        public virtual void AddEnvironmentView(IEnvironmentView environmentView)
        {
            views.Add(environmentView);
        }

        public virtual void RemoveEnvironmentView(IEnvironmentView environmentView)
        {
            views.Remove(environmentView);
        }

        public virtual void NotifyViews(string msg)
        {
            foreach (IEnvironmentView ev in views)
            {
                ev.Notify(msg);
            }
        }

        protected virtual void updatePerformanceMeasure(IAgent forAgent, double addTo)
        {
            performanceMeasures.Add(forAgent, GetPerformanceMeasure(forAgent) + addTo);
        }

        protected virtual void notifyEnvironmentViews(IAgent agent)
        {
            foreach (IEnvironmentView view in views)
            {
                view.AgentAdded(agent, this);
            }
        }

        protected virtual void notifyEnvironmentViews(IAgent agent, IPercept percept, IAction action)
        {
            foreach (IEnvironmentView view in views)
            {
                view.AgentActed(agent, percept, action, this);
            }
        }
    }
}
