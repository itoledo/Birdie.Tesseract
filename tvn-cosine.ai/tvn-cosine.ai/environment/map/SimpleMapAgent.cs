using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common.collections;
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
        private IEnvironmentViewNotifier notifier = null;
        private SearchForActions<string, MoveToAction> _search = null;
        private string[] goals = null;
        private int goalTestPos = 0;

        public SimpleMapAgent(Map map, IEnvironmentViewNotifier notifier,
            SearchForActions<string, MoveToAction> search)
        {
            this.map = map;
            this.notifier = notifier;
            this._search = search;
        }

        public SimpleMapAgent(Map map, IEnvironmentViewNotifier notifier,
            SearchForActions<string, MoveToAction> search,
                              int maxGoalsToFormulate)
            : base(maxGoalsToFormulate)
        {

            this.map = map;
            this.notifier = notifier;
            this._search = search;
        }

        public SimpleMapAgent(Map map, IEnvironmentViewNotifier notifier,
            SearchForActions<string, MoveToAction> search,
            string[] goals)
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

        protected override void updateState(IPercept p)
        {
            DynamicPercept dp = (DynamicPercept)p;
            state.setAttribute(DynAttributeNames.AGENT_LOCATION, dp.getAttribute(DynAttributeNames.PERCEPT_IN));
        }

        protected override object formulateGoal()
        {
            object goal;
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
                notifier.NotifyViews("CurrentLocation=In("
                    + state.getAttribute(DynAttributeNames.AGENT_LOCATION)
                        + "), Goal=In(" + goal + ")");
            return goal;
        }

        protected override Problem<string, MoveToAction> formulateProblem(object goal)
        {
            return new BidirectionalMapProblem(map,
                (string)state.getAttribute(DynAttributeNames.AGENT_LOCATION),
                    (string)goal);
        }

        protected override IQueue<MoveToAction> search(Problem<string, MoveToAction> problem)
        {
            return _search.findActions(problem);
        }

        protected override void notifyViewOfMetrics()
        {
            if (notifier != null)
            {
                ISet<string> keys = _search.getMetrics().keySet();
                foreach (string key in keys)
                    notifier.NotifyViews("METRIC[" + key + "]=" + _search.getMetrics().get(key));
            }
        }
    }
}
