namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicAction : ObjectWithDynamicAttributes<string, object>, IAction
    {
        private const string ATTRIBUTE_NAME = "name";

        private readonly bool isNoOp;

        public DynamicAction(string name)
            : this(name, false)
        { }

        public DynamicAction(string name, bool isNoOp)
        {
            SetAttribute(ATTRIBUTE_NAME, name);
            this.isNoOp = isNoOp;
        }

        /// <summary>
        /// Returns the value of the name attribute.
        /// </summary>
        /// <returns>the value of the name attribute.</returns>
        public virtual string getName()
        {
            return GetAttribute(ATTRIBUTE_NAME).ToString();
        }

        public virtual bool IsNoOp()
        {
            return isNoOp;
        }

        public override string DescribeType()
        {
            return typeof(IAction).Name;
        }

        public static readonly DynamicAction NO_OP = new DynamicAction("NoOp", true);
    }
}