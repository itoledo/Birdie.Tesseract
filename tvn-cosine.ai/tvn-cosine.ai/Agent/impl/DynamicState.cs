namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     */
    public class DynamicState : ObjectWithDynamicAttributes<object, object>, State
    {
        public DynamicState()
        { }

        public override string describeType()
        {
            return typeof(State).Name;
        }
    }
}
