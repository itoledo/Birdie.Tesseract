using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent
{ 
    public class DynamicState : ObjectWithDynamicAttributes, IState
    {
        public const string TYPE = "State";

        public DynamicState()
        { }

        public override string DescribeType()
        {
            return TYPE;
        }
    }
}
