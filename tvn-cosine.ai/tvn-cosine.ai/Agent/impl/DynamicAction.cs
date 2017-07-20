namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicAction : ObjectWithDynamicAttributes, IAction
    {
        public const string ATTRIBUTE_NAME = "name";
         
        public DynamicAction(string name)
        {
            this.setAttribute(ATTRIBUTE_NAME, name);
        }

        /// <summary>
        /// Returns the value of the name attribute.
        /// </summary>
        /// <returns></returns>
        public virtual string getName()
        {
            return (string)getAttribute(ATTRIBUTE_NAME);
        }
         
        public virtual bool IsNoOp()
        {
            return false;
        }
         
        public override string describeType()
        {
            return "Action";
        }
    }
}
