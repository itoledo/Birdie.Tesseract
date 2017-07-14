using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.search.framework.agent
{ 
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public class SearchAgent<S, A> : AbstractAgent
        where A : Action
    {
        private IList<A> actionList;

        private IDictionary<string, double> searchMetrics;

        public SearchAgent(IProblem<S, A> p, SearchForActions<S, A> search)
        {
            IList<A> actions = search.findActions(p);
            actionList = new List<A>();
            if (null != actions)
                foreach (var v in actions)
                    actionList.Add(v);

            searchMetrics = search.getMetrics();
        }

        public override Action execute(Percept p)
        {
            if (actionList.Count > 0)
            {
                var v = actionList[0];
                actionList.Remove(v);
                return v;
            }
            return DynamicAction.NO_OP; // no success or at goal
        }

        public bool IsDone()
        {
            return actionList.Count == 0;
        }

        public IList<A> GetActions()
        {
            return actionList;
        }

        public IDictionary<string, double> GetInstrumentation()
        {
            IDictionary<string, double> result = new Dictionary<string, double>();
            foreach (string key in searchMetrics.Keys)
            {
                double value = searchMetrics[key];
                result.Add(key, value);
            }
            return result;
        }
    }
}
