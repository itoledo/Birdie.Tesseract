using System.Collections.Generic;

namespace tvn.cosine.ai.agent.impl
{
    public class DynamicAction : Dictionary<string, string>, IAction
    {
        public const string ATTRIBUTE_NAME = "name";

        public virtual bool IsNoOp
        {
            get
            {
                return false;
            }
        }

        //

        public DynamicAction(string name)
        {
            this[ATTRIBUTE_NAME] = name;
        }

        /**
         * Returns the value of the name attribute.
         * 
         * @return the value of the name attribute.
         */
        public string getName()
        {
            return this[ATTRIBUTE_NAME];
        }

        // END-Action
        //

    }
}