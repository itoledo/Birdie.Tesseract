namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class DynamicEnvironmentState : ObjectWithDynamicAttributes<object, object>, IEnvironmentState
    {
        public DynamicEnvironmentState()
        { }

        public override string describeType()
        {
            return typeof(IEnvironmentState).Name;
        }
    }
}
