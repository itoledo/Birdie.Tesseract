using System;
using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.informed;

namespace tvn.cosine.ai.environment.map
{
    /**
     * Variant of {@link aima.core.environment.map.SimpleMapAgent} which works
     * correctly also for A* and other best-first search implementations. It can be
     * extended also for scenarios, in which the agent faces unforeseen events. When
     * using informed search and more then one goal, make sure, that a heuristic
     * function factory is provided!
     *
     * @author Ruediger Lunde
     */
    public class MapAgent : ProblemSolvingAgent<string, MoveToAction>
    {
        protected readonly Map map;
        protected readonly DynamicState state = new DynamicState();
        protected readonly IList<string> goals = new List<string>();
        protected int currGoalIdx = -1;

        // possibly null...
        protected EnvironmentViewNotifier notifier = null;
        private SearchForActions<string, MoveToAction> _search = null;
        private Func<string, HeuristicEvaluationFunction<Node<string, MoveToAction>>> hFnFactory;

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search, string goal)
        {
            this.map = map;
            this._search = search;
            goals.Add(goal);
        }

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search, string goal, EnvironmentViewNotifier notifier)
            : this(map, search, goal)
        {
            this.notifier = notifier;
        }

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search, IList<string> goals)
        {
            this.map = map;
            this._search = search;
            foreach (var v in goals)
                this.goals.Add(v);
        }

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search, IList<string> goals, EnvironmentViewNotifier notifier)
            : this(map, search, goals)
        {

            this.notifier = notifier;
        }

        /**
         * Constructor.
         * @param map Information about the environment
         * @param search Search strategy to be used
         * @param goals List of locations to be visited
         * @param notifier Gets informed about decisions of the agent
         * @param hFnFactory Factory, mapping goal locations to heuristic functions. When using
         *                   informed search, the agent must be able to estimate rest costs for
         *                   the goals he has selected.
         */
        public MapAgent(Map map, SearchForActions<string, MoveToAction> search, List<string> goals,
                        EnvironmentViewNotifier notifier,
                        Func<string, HeuristicEvaluationFunction<Node<string, MoveToAction>>> hFnFactory)
            : this(map, search, goals, notifier)
        {
            this.hFnFactory = hFnFactory;
        }

        //
        // PROTECTED METHODS
        // 
        protected override void UpdateState(Percept p)
        {
            DynamicPercept dp = (DynamicPercept)p;
            state.SetAttribute(DynAttributeNames.AGENT_LOCATION, dp.GetAttribute(DynAttributeNames.PERCEPT_IN));
        }

        protected override string FormulateGoal()
        {
            string goal = null;
            if (currGoalIdx < goals.Count - 1)
            {
                goal = goals[++currGoalIdx];

                modifyHeuristicFunction(goal);

                if (notifier != null)
                    notifier.notifyViews("CurrentLocation=In(" 
                        + state.GetAttribute(DynAttributeNames.AGENT_LOCATION) 
                        + "), Goal=In(" + goal + ")"); 
            }
            return goal != null ? goal : null;
        }

        protected override IProblem<string, MoveToAction> FormulateProblem(string goal)
        {
            return new BidirectionalMapProblem(map, 
                (string)state.GetAttribute(DynAttributeNames.AGENT_LOCATION), goal);
        }

        protected override IList<MoveToAction> Search(IProblem<string, MoveToAction> problem)
        {
            IList<MoveToAction> result = _search.findActions(problem);
            notifyViewOfMetrics();
            return result;
        }

        protected void notifyViewOfMetrics()
        {
            if (notifier != null)
            {
                ISet<string> keys = new HashSet<string>(_search.getMetrics().Keys);
                foreach (string key in keys)
                {
                    notifier.notifyViews("METRIC[" + key + "]=" + _search.getMetrics()[key]);
                }
            }
        }

        private void modifyHeuristicFunction(string goal)
        {
            if (hFnFactory != null && _search is Informed<string, MoveToAction>)
            {
                ((Informed<string, MoveToAction>)_search).h = hFnFactory(goal);
            }
        }
    }
}
