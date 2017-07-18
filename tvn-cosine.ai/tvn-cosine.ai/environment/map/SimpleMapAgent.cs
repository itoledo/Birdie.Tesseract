using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common.collections;

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
        private EnvironmentViewNotifier notifier = null;
        private SearchForActions<string, MoveToAction> search = null;
        private string[] goals = null;
        private int goalTestPos = 0;

        public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier,
            SearchForActions<string, MoveToAction> search)
        {
            this.map = map;
            this.notifier = notifier;
            this.search = search;
        }

        public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier,
            SearchForActions<string, MoveToAction> search,
                              int maxGoalsToFormulate)
            : base(maxGoalsToFormulate)
        {

            this.map = map;
            this.notifier = notifier;
            this.search = search;
        }

        public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier,
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
            this.search = search;
            this.goals = new string[goals.Length];
            System.Array.Copy(goals, 0, this.goals, 0, goals.Length);
        }

        protected override void updateState(Percept p)
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
                notifier.notifyViews("CurrentLocation=In("
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
            return search.findActions(problem);
        }

        protected override void notifyViewOfMetrics()
        {
            if (notifier != null)
            {
                ISet<string> keys = search.getMetrics().GetKeys();
                foreach (string key in keys)
                    notifier.notifyViews("METRIC[" + key + "]=" + search.getMetrics().Get(key));
            }
        }
    }
}
