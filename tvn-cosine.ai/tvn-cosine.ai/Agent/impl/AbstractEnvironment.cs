using System.Collections.Generic;

namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public abstract class AbstractEnvironment : Environment, EnvironmentViewNotifier
    {
        // Note: Use LinkedHashSet's in order to ensure order is respected as
        // provide
        // access to these elements via List interface.
        protected ISet<EnvironmentObject> envObjects = new HashSet<EnvironmentObject>();
        protected ISet<Agent> agents = new HashSet<Agent>();
        protected ISet<EnvironmentView> views = new HashSet<EnvironmentView>();
        protected IDictionary<Agent, double> performanceMeasures = new Dictionary<Agent, double>();

        //
        // PRUBLIC METHODS
        //

        //
        // Methods to be implemented by subclasses.

        public abstract void executeAction(Agent agent, Action action);

        public abstract Percept getPerceptSeenBy(Agent anAgent);

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
        public virtual IList<Agent> getAgents()
        {
            // Return as a List but also ensures the caller cannot modify
            return new List<Agent>(agents);
        }

        public virtual void addAgent(Agent a)
        {
            addEnvironmentObject(a);
        }

        public virtual void removeAgent(Agent a)
        {
            removeEnvironmentObject(a);
        }

        public virtual IList<EnvironmentObject> getEnvironmentObjects()
        {
            // Return as a List but also ensures the caller cannot modify
            return new List<EnvironmentObject>(envObjects);
        }

        public virtual void addEnvironmentObject(EnvironmentObject eo)
        {
            envObjects.Add(eo);
            if (eo is Agent)
            {
                Agent a = (Agent)eo;
                if (!agents.Contains(a))
                {
                    agents.Add(a);
                    this.notifyEnvironmentViews(a);
                }
            }
        }

        public virtual void removeEnvironmentObject(EnvironmentObject eo)
        {
            envObjects.Remove(eo);
            agents.Remove(eo as Agent);
        }

        /**
         * Central template method for controlling agent simulation. The concrete
         * behavior is determined by the primitive operations
         * {@link #getPerceptSeenBy(Agent)}, {@link #executeAction(Agent, Action)},
         * and {@link #createExogenousChange()}.
         */
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
            for (int i = 0; i < n; i++)
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

        public virtual double getPerformanceMeasure(Agent forAgent)
        {
            if (performanceMeasures.ContainsKey(forAgent))
            {
                performanceMeasures.Add(forAgent, 0);
            }

            return performanceMeasures[forAgent];
        }

        public virtual void addEnvironmentView(EnvironmentView ev)
        {
            views.Add(ev);
        }

        public virtual void removeEnvironmentView(EnvironmentView ev)
        {
            views.Remove(ev);
        }

        public virtual void notifyViews(string msg)
        {
            foreach (EnvironmentView ev in views)
            {
                ev.notify(msg);
            }
        }

        // END-Environment
        //

        //
        // PROTECTED METHODS
        //

        protected virtual void updatePerformanceMeasure(Agent forAgent, double addTo)
        {
            performanceMeasures.Add(forAgent, getPerformanceMeasure(forAgent)
                    + addTo);
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
