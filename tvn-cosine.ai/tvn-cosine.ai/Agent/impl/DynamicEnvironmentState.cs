namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicEnvironmentState : ObjectWithDynamicAttributes<string, object>, IEnvironmentState
    {
        public DynamicEnvironmentState()
        { }

        public override string DescribeType()
        {
            return typeof(IEnvironmentState).Name;
        }
    }
}
