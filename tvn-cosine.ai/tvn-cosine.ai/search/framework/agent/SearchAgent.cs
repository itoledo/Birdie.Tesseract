using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.search.framework.agent
{
    /**
  *
  * @param <S> The type used to represent states
  * @param <A> The type of the actions to be used to navigate through the state space
  *
  * @author Ravi Mohan
  * @author Ruediger Lunde
  */
    public class SearchAgent<S, A> : AgentBase
        where A : IAction
    { 
        private IQueue<A> actionList; 
        private Metrics searchMetrics;

        public SearchAgent(Problem<S, A> p, SearchForActions<S, A> search)
        {
            IQueue<A> actions = search.findActions(p);
            actionList = Factory.CreateQueue<A>();
            if (null != actions)
                actionList.AddAll(actions);

            //   actionIterator = actionList.iterator();
            searchMetrics = search.getMetrics();
        }

        public override IAction Execute(IPercept p)
        {
            if (!actionList.IsEmpty())
                return actionList.Pop();
            return DynamicAction.NO_OP; // no success or at goal
        }

        public bool isDone()
        {
            return actionList.IsEmpty();
        }

        public IQueue<A> getActions()
        {
            return actionList;
        }

        public Properties getInstrumentation()
        {
            Properties result = new Properties();
            foreach (string key in searchMetrics.keySet())
            {
                string value = searchMetrics.get(key);
                result.setProperty(key, value);
            }
            return result;
        }
    }
}
