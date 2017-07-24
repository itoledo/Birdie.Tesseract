using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent
{ 
    public class DynamicEnvironmentState : ObjectWithDynamicAttributes, IEnvironmentState
    {
        public DynamicEnvironmentState()
        { }

        public override string describeType()
        {
            return "EnvironmentState";
        }
    }
}
