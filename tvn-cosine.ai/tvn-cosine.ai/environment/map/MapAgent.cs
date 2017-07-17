﻿using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.util;

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
        protected readonly IQueue<string> goals = Factory.CreateQueue<string>();
        protected int currGoalIdx = -1;

        // possibly null...
        protected EnvironmentViewNotifier notifier = null;
        private SearchForActions<string, MoveToAction> search = null;
        private Function<string, ToDoubleFunction<Node<string, MoveToAction>>> hFnFactory;

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search, string goal)
        {
            this.map = map;
            this.search = search;
            goals.Add(goal);
        }

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search,
            string goal, EnvironmentViewNotifier notifier)
            : this(map, search, goal)
        {
            this.notifier = notifier;
        }

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search,
            IQueue<string> goals)
        {
            this.map = map;
            this.search = search;
            this.goals.AddAll(goals);
        }

        public MapAgent(Map map, SearchForActions<string, MoveToAction> search,
            IQueue<string> goals,
            EnvironmentViewNotifier notifier)
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
         * @param hFnFactory Factory, mapping goals to heuristic functions. When using
         *                   informed search, the agent must be able to estimate remaining costs for
         *                   the goals he has selected.
         */
        public MapAgent(Map map,
            SearchForActions<string, MoveToAction> search, IQueue<string> goals,
            EnvironmentViewNotifier notifier,
            Function<string, ToDoubleFunction<Node<string, MoveToAction>>> hFnFactory)
            : this(map, search, goals, notifier)
        {
            this.hFnFactory = hFnFactory;
        }

        //
        // PROTECTED METHODS
        // 
        protected override void updateState(Percept p)
        {
            DynamicPercept dp = (DynamicPercept)p;
            state.setAttribute(DynAttributeNames.AGENT_LOCATION, dp.getAttribute(DynAttributeNames.PERCEPT_IN));
        }

        protected override object formulateGoal()
        {
            string goal = null;
            if (currGoalIdx < goals.Size() - 1)
            {
                goal = goals.Get(++currGoalIdx);
                if (hFnFactory != null && search is Informed)
                    ((Informed<string, MoveToAction>)search)
                        .setHeuristicFunction(hFnFactory(goal));

                if (notifier != null)
                    notifier.notifyViews("Current location: In(" + state.getAttribute(DynAttributeNames.AGENT_LOCATION)
                            + "), Goal: In(" + goal + ")");
            }
            return goal;
        }

        protected override Problem<string, MoveToAction> formulateProblem(object goal)
        {
            return new BidirectionalMapProblem(map, 
                (string)state.getAttribute(DynAttributeNames.AGENT_LOCATION),
                    (string)goal);
        }
         
    protected IQueue<MoveToAction> search(Problem<string, MoveToAction> problem)
        {
            IQueue<MoveToAction> result = search.findActions(problem);
            notifyViewOfMetrics();
            return result;
        }

        protected void notifyViewOfMetrics()
        {
            if (notifier != null)
                notifier.notifyViews("Search metrics: " + search.getMetrics());
        }
    }
}
