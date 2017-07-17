namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicState : ObjectWithDynamicAttributes, State
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
