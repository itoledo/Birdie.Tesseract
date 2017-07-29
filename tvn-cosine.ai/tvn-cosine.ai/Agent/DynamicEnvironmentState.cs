using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent
{
    public class DynamicEnvironmentState : ObjectWithDynamicAttributes, IEnvironmentState
    {
        public const string TYPE = "EnvironmentState";

        public DynamicEnvironmentState()
        { }

        public override string DescribeType()
        {
            return TYPE;
        }
    }
}
