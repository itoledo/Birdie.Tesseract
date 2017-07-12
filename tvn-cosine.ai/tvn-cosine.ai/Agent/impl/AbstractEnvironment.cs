using System.Collections.Generic;

namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public abstract class AbstractEnvironment : IEnvironment 
    {
        // Note: Use LinkedHashSet's in order to ensure order is respected as
        // provide
        // access to these elements via List interface.
        protected ISet<IEnvironmentObject> envObjects = new HashSet<IEnvironmentObject>();
        protected ISet<IAgent> agents = new HashSet<IAgent>();
        protected ISet<IEnvironmentView> views = new HashSet<IEnvironmentView>();
        protected IDictionary<IAgent, double> performanceMeasures = new Dictionary<IAgent, double>();

        //
        // PRUBLIC METHODS
        //

        //
        // Methods to be implemented by subclasses.

        public abstract void executeAction(IAgent agent, IAction action);

        public abstract IPercept getPerceptSeenBy(IAgent anAgent);

        /**
         * Method for implementing dynamic environments in which not all changes are
         * directly caused by agent action execution. The default implementation
         * does nothing.
         */
        public void createExogenousChange()
        {
        }

        //
        // START-Environment
        public virtual IList<IAgent> GetAgents()
        {
            // Return as a List but also ensures the caller cannot modify
            return new List<IAgent>(agents);
        }

        public virtual void AddAgent(IAgent a)
        {
            AddEnvironmentObject(a);
        }

        public virtual void RemoveAgent(IAgent a)
        {
            RemoveEnvironmentObject(a);
        }

        public virtual IList<IEnvironmentObject> GetEnvironmentObjects()
        {
            // Return as a List but also ensures the caller cannot modify
            return new List<IEnvironmentObject>(envObjects);
        }

        public virtual void AddEnvironmentObject(IEnvironmentObject eo)
        {
            envObjects.Add(eo);
            if (eo is IAgent)
            {
                IAgent a = (IAgent)eo;
                if (!agents.Contains(a))
                {
                    agents.Add(a);
                    this.notifyEnvironmentViews(a);
                }
            }
        }

        public virtual void RemoveEnvironmentObject(IEnvironmentObject eo)
        {
            envObjects.Remove(eo);
            agents.Remove(eo as IAgent);
        }

        /**
         * Central template method for controlling agent simulation. The concrete
         * behavior is determined by the primitive operations
         * {@link #getPerceptSeenBy(Agent)}, {@link #executeAction(Agent, Action)},
         * and {@link #createExogenousChange()}.
         */
        public virtual void Step()
        {
            foreach (IAgent agent in agents)
            {
                if (agent.isAlive())
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
                if (agent.isAlive())
                {
                    return false;
                }
            }
            return true;
        }

        public virtual double GetPerformanceMeasure(IAgent forAgent)
        {
            if (performanceMeasures.ContainsKey(forAgent))
            {
                performanceMeasures.Add(forAgent, 0);
            }

            return performanceMeasures[forAgent];
        }

        public virtual void AddEnvironmentView(IEnvironmentView ev)
        {
            views.Add(ev);
        }

        public virtual void RemoveEnvironmentView(IEnvironmentView ev)
        {
            views.Remove(ev);
        }

        public virtual void NotifyViews(string msg)
        {
            foreach (IEnvironmentView ev in views)
            {
                ev.Notify(msg);
            }
        }

        // END-Environment
        //

        //
        // PROTECTED METHODS
        //

        protected virtual void updatePerformanceMeasure(IAgent forAgent, double addTo)
        {
            performanceMeasures.Add(forAgent, GetPerformanceMeasure(forAgent)
                    + addTo);
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
