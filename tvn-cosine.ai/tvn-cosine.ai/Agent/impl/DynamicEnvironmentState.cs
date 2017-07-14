namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicEnvironmentState : ObjectWithDynamicAttributes<string, object>, EnvironmentState
    {
        public DynamicEnvironmentState()
        { }

        public override string DescribeType()
        {
            return typeof(EnvironmentState).Name;
        }
    }
}
