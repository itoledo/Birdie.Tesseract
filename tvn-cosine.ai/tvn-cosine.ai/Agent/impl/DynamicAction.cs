namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class DynamicAction : ObjectWithDynamicAttributes, Action
    {
        public const string ATTRIBUTE_NAME = "name";
         
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
         
        public virtual bool isNoOp()
        {
            return false;
        }
         
        public override string describeType()
        {
            return "Action";
        }
    }
}
