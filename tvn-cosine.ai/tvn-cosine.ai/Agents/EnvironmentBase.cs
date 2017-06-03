using System.Collections.Generic;
using System.Linq;

namespace tvn_cosine.ai.Agents
{
    public abstract class EnvironmentBase : IEnvironment
    {
        protected readonly ISet<IAgent> agents;
        protected readonly ISet<IEnvironmentObject> environmentObjects;
        protected readonly ISet<IEnvironmentView> environmentViews;
        protected readonly IDictionary<IAgent, double> performanceMeasures;
         
        public EnvironmentBase()
        {
            agents = new HashSet<IAgent>();
            environmentObjects = new HashSet<IEnvironmentObject>();
            environmentViews = new HashSet<IEnvironmentView>();
            performanceMeasures = new Dictionary<IAgent, double>();
        }

        public abstract void ExecuteAction(IAgent agent, IAction action);
        public abstract IPercept GetPerceptSeenBy(IAgent agent);

        /// <summary>
        /// Method for implementing dynamic environments in which not all changes are
        /// directly caused by agent action execution.The default implementation
        /// does nothing.  
        /// </summary>
        public virtual void CreateExogenousChange()
        { }

        protected virtual void UpdatePerformanceMeasure(IAgent agent, double addTo)
        {
            performanceMeasures.Add(agent, GetPerformanceMeasure(agent) + addTo);
        }

        protected virtual void NotifyEnvironmentViews(IAgent agent)
        {
            foreach (var environmentView in environmentViews)
            {
                environmentView.AgentAdded(agent, this);
            }
        }

        protected virtual void NotifyEnvironmentViews(IAgent agent, IPercept percept, IAction action)
        {
            foreach (var environmentView in environmentViews)
            {
                environmentView.AgentActed(agent, percept, action, this);
            }
        }
        
        public IReadOnlyCollection<IAgent> Agents
        {
            get
            {
                return agents.ToList().AsReadOnly();
            }
        }

        public IReadOnlyCollection<IEnvironmentObject> EnvironmentObjects
        {
            get
            {
                return environmentObjects.ToList().AsReadOnly();
            }
        }

        public IReadOnlyCollection<IEnvironmentView> EnvironmentViews
        {
            get
            {
                return environmentViews.ToList().AsReadOnly();
            }
        }

        public virtual void AddAgent(IAgent agent)
        {
            AddEnvironmentObject(agent);
        }

        public virtual void RemoveAgent(IAgent agent)
        {
            RemoveEnvironmentObject(agent);
        }

        public virtual void AddEnvironmentObject(IEnvironmentObject environmentObject)
        {
            environmentObjects.Add(environmentObject);
            if (environmentObject is IAgent)
            {
                if (!agents.Contains(environmentObject as IAgent))
                {
                    agents.Add(environmentObject as IAgent);
                    NotifyEnvironmentViews(environmentObject as IAgent);
                }
            }
        }

        public virtual void RemoveEnvironmentObject(IEnvironmentObject environmentObject)
        {
            environmentObjects.Remove(environmentObject);
            if (environmentObject is IAgent)
            {
                agents.Remove(environmentObject as IAgent);
            }
        }

        public virtual void Step()
        {
            foreach (var agent in agents)
            {
                if (agent.IsAlive)
                {
                    var percept = GetPerceptSeenBy(agent);
                    var action = agent.Execute(percept);
                    ExecuteAction(agent, action);
                    NotifyEnvironmentViews(agent, percept, action);
                }
            }
            CreateExogenousChange();
        }

        public virtual void Step(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                Step();
            }
        }

        public virtual void StepUntilDone()
        {
            while (!IsDone)
            {
                Step();
            }
        }

        public bool IsDone
        {
            get
            {
                foreach (var agent in agents)
                {
                    if (agent.IsAlive)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public virtual double GetPerformanceMeasure(IAgent agent)
        {
            if (!performanceMeasures.ContainsKey(agent))
            {
                performanceMeasures[agent] = 0D;
            }

            return performanceMeasures[agent];
        }


        public virtual void AddEnvironmentView(IEnvironmentView environmentView)
        {
            environmentViews.Add(environmentView);
        }

        public virtual void RemoveEnvironmentView(IEnvironmentView environmentView)
        {
            environmentViews.Remove(environmentView);
        }

        public virtual void NotifyViews(string message)
        {
            foreach (var environmentView in environmentViews)
            {
                environmentView.Notify(message);
            }
        }
    }
}
