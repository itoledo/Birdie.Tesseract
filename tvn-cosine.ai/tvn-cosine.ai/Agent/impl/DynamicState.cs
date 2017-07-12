namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     */
    public class DynamicState : ObjectWithDynamicAttributes<object, object>, IState
    {
        public DynamicState()
        { }

        public override string describeType()
        {
            return typeof(IState).Name;
        }
    }
}
