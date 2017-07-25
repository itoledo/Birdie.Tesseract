using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.agent 
{ 
    public class DynamicAction : ObjectWithDynamicAttributes, IAction
    {
        public static readonly DynamicAction NO_OP = new DynamicAction("NoOp", true);

        public const string ATTRIBUTE_NAME = "name";
        private readonly bool isNoOp;
         
        public DynamicAction(string name)
        {
            this.SetAttribute(ATTRIBUTE_NAME, name);
        }

        public DynamicAction(string name, bool isNoOp)
        {
            this.SetAttribute(ATTRIBUTE_NAME, name);
            this.isNoOp = isNoOp;
        }

        /// <summary>
        /// Returns the value of the name attribute.
        /// </summary>
        /// <returns></returns>
        public virtual string getName()
        {
            return (string)GetAttribute(ATTRIBUTE_NAME);
        }
         
        public virtual bool IsNoOp()
        {
            return isNoOp;
        }
         
        public override string DescribeType()
        {
            return "Action";
        }
    }
}
