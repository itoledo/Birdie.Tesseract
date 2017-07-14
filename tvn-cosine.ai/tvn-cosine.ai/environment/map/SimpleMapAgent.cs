using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.environment.map
{
    /**
     * Note: This implementation should be used with one predefined goal only or
     * with uninformed search. As the heuristic of the used search algorithm is
     * never changed, estimates for the second (or randomly created goal) will be
     * wrong.
     * 
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     * 
     */
    public class SimpleMapAgent : SimpleProblemSolvingAgent<string, MoveToAction>
    {

        protected Map map = null;
        protected DynamicState state = new DynamicState();

        // possibly null...
        private EnvironmentViewNotifier notifier;
        private SearchForActions<string, MoveToAction> _search;
        private string[] goals = null;
        private int goalTestPos = 0;

        public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier, SearchForActions<string, MoveToAction> search)
        {
            this.map = map;
            this.notifier = notifier;
            this._search = search;
        }

        public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier, SearchForActions<string, MoveToAction> search, int maxGoalsToFormulate)
            : base(maxGoalsToFormulate)
        {

            this.map = map;
            this.notifier = notifier;
            this._search = search;
        }

        public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier, SearchForActions<string, MoveToAction> search, string[] goals)
          : this(map, search, goals)
        {
            this.notifier = notifier;
        }

        public SimpleMapAgent(Map map, SearchForActions<string, MoveToAction> search, string[] goals)
          : base(goals.Length)
        {
            this.map = map;
            this._search = search;
            this.goals = new string[goals.Length];
            System.Array.Copy(goals, 0, this.goals, 0, goals.Length);
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
            string goal;
            if (goals == null)
            {
                goal = map.randomlyGenerateDestination();
            }
            else
            {
                goal = goals[goalTestPos];
                goalTestPos++;
            }
            if (notifier != null)
                notifier.notifyViews("CurrentLocation=In(" + state.GetAttribute(DynAttributeNames.AGENT_LOCATION) + "), Goal=In(" + goal + ")");

            return goal;
        }

        protected override IProblem<string, MoveToAction> FormulateProblem(string goal)
        {
            return new BidirectionalMapProblem(map, (string)state.GetAttribute(DynAttributeNames.AGENT_LOCATION), goal);
        }

        protected override IList<MoveToAction> Search(IProblem<string, MoveToAction> problem)
        {
            return _search.findActions(problem);
        }

        protected override void NotifyViewOfMetrics()
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
    } 
}
