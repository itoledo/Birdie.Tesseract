namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicState : ObjectWithDynamicAttributes<string, object>, IState
    {
        public DynamicState()
        { }

        public override string DescribeType()
        {
            return typeof(IState).Name;
        }
    }
}
