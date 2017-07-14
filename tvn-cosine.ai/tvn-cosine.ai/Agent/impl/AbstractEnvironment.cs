using System.Collections.Generic;
using System.Linq;

namespace tvn.cosine.ai.agent.impl
{
    public abstract class AbstractEnvironment : Environment
    {
        protected readonly ISet<EnvironmentObject> envObjects;
        protected readonly ISet<Agent> agents;
        protected readonly ISet<EnvironmentView> views;
        protected readonly IDictionary<Agent, double> performanceMeasures;

        public AbstractEnvironment()
        {
            envObjects = new HashSet<EnvironmentObject>();
            agents = new HashSet<Agent>();
            views = new HashSet<EnvironmentView>();
            performanceMeasures = new Dictionary<Agent, double>();
        }

        public abstract void executeAction(Agent agent, Action action);
        public abstract Percept getPerceptSeenBy(Agent anAgent);

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
        public virtual IList<Agent> getAgents()
        {
            return agents.ToList().AsReadOnly();
        }

        public virtual void addAgent(Agent agent)
        {
            addEnvironmentObject(agent);
        }

        public virtual void removeAgent(Agent agent)
        {
            removeEnvironmentObject(agent);
        }

        /// <summary>
        /// Return as a List but also ensures the caller cannot modify
        /// </summary>
        /// <returns></returns>
        public virtual IList<EnvironmentObject> getEnvironmentObjects()
        {
            return envObjects.ToList().AsReadOnly();
        }

        public virtual void addEnvironmentObject(EnvironmentObject environmentObject)
        {
            envObjects.Add(environmentObject);
            if (environmentObject is Agent)
            {
                Agent a = (Agent)environmentObject;
                if (!agents.Contains(a))
                {
                    agents.Add(a);
                    this.notifyEnvironmentViews(a);
                }
            }
        }

        public virtual void removeEnvironmentObject(EnvironmentObject environmentObject)
        {
            envObjects.Remove(environmentObject);
            if (environmentObject is Agent)
            {
                agents.Remove(environmentObject as Agent);
            }
        }

        /// <summary>
        /// Central template method for controlling agent simulation. The concrete
        /// behavior is determined by the primitive operations
        /// #getPerceptSeenBy(Agent), #executeAction(Agent, Action),
        /// and #createExogenousChange().
        /// </summary>
        public virtual void step()
        {
            foreach (Agent agent in agents)
            {
                if (agent.isAlive())
                {
                    Percept percept = getPerceptSeenBy(agent);
                    Action anAction = agent.execute(percept);
                    executeAction(agent, anAction);
                    notifyEnvironmentViews(agent, percept, anAction);
                }
            }
            createExogenousChange();
        }

        public virtual void step(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                step();
            }
        }

        public virtual void stepUntilDone()
        {
            while (!isDone())
            {
                step();
            }
        }

        public virtual bool isDone()
        {
            foreach (Agent agent in agents)
            {
                if (agent.isAlive())
                {
                    return false;
                }
            }
            return true;
        }

        public virtual double getPerformanceMeasure(Agent agent)
        {
            if (!performanceMeasures.ContainsKey(agent))
            {
                performanceMeasures[agent] = 0;
            }

            return performanceMeasures[agent];
        }

        public virtual void addEnvironmentView(EnvironmentView environmentView)
        {
            views.Add(environmentView);
        }

        public virtual void removeEnvironmentView(EnvironmentView environmentView)
        {
            views.Remove(environmentView);
        }

        public virtual void notifyViews(string msg)
        {
            foreach (EnvironmentView ev in views)
            {
                ev.notify(msg);
            }
        }

        protected virtual void updatePerformanceMeasure(Agent forAgent, double addTo)
        {
            performanceMeasures[forAgent] = getPerformanceMeasure(forAgent) + addTo;
        }

        protected virtual void notifyEnvironmentViews(Agent agent)
        {
            foreach (EnvironmentView view in views)
            {
                view.agentAdded(agent, this);
            }
        }

        protected virtual void notifyEnvironmentViews(Agent agent, Percept percept, Action action)
        {
            foreach (EnvironmentView view in views)
            {
                view.agentActed(agent, percept, action, this);
            }
        }
    }
}
