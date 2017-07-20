using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
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
