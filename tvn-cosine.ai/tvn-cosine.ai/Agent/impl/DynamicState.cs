using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicState : ObjectWithDynamicAttributes, IState
    {
        public DynamicState()
        {

        }

        public override string describeType()
        {
            return "State";
        }
    }
}
