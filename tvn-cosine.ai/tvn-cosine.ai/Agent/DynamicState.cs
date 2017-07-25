using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent
{ 
    public class DynamicState : ObjectWithDynamicAttributes, IState
    {
        public DynamicState()
        { }

        public override string DescribeType()
        {
            return "State";
        }
    }
}
