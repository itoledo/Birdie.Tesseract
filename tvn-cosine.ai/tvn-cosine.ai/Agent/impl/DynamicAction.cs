namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class DynamicAction : ObjectWithDynamicAttributes<object, object>, IAction
    {
        public const string ATTRIBUTE_NAME = "name";

        //

        public DynamicAction(string name)
        {
            this.setAttribute(ATTRIBUTE_NAME, name);
        }

        /**
         * Returns the value of the name attribute.
         * 
         * @return the value of the name attribute.
         */
        public virtual string getName()
        {
            return (string)getAttribute(ATTRIBUTE_NAME);
        }

        //
        // START-Action
        public virtual bool IsNoOp()
        {
            return false;
        }

        // END-Action
        //

        public override string describeType()
        {
            return typeof(IAction).Name;
        }
    }
}