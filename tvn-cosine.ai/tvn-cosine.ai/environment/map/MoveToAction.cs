using tvn.cosine.ai.agent.impl;

namespace tvn.cosine.ai.environment.map
{
    public class MoveToAction : DynamicAction
    { 
        public const string ATTRIBUTE_MOVE_TO_LOCATION = "location";

        public MoveToAction(string location)
            : base("moveTo") 
        {
            setAttribute(ATTRIBUTE_MOVE_TO_LOCATION, location);
        }

        public string getToLocation()
        {
            return (string)getAttribute(ATTRIBUTE_MOVE_TO_LOCATION);
        }
    }
}
