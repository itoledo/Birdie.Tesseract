namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicState : ObjectWithDynamicAttributes<string, object>, State
    {
        public DynamicState()
        { }

        public override string DescribeType()
        {
            return typeof(State).Name;
        }
    }
}
