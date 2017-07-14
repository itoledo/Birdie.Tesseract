using System.Text;

namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicAction : ObjectWithDynamicAttributes<string, object>, Action
    {
        private const string ATTRIBUTE_NAME = "name";

        private readonly bool _isNoOp;

        public DynamicAction(string name)
            : this(name, false)
        { }

        public DynamicAction(string name, bool isNoOp)
        {
            SetAttribute(ATTRIBUTE_NAME, name);
            this._isNoOp = isNoOp;
        }

        /// <summary>
        /// Returns the value of the name attribute.
        /// </summary>
        /// <returns>the value of the name attribute.</returns>
        public virtual string getName()
        {
            return GetAttribute(ATTRIBUTE_NAME).ToString();
        }

        public virtual bool isNoOp()
        {
            return _isNoOp;
        }

        public override string DescribeType()
        {
            return typeof(Action).Name;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[Action[name=="); 
            stringBuilder.Append(getName());
            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }

        public static readonly DynamicAction NO_OP = new DynamicAction("NoOp", true);
    }
}