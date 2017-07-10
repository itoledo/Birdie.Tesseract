using System;

namespace tvn.cosine.ai.environment.connectfour
{
    /**
     * Helper class for action ordering.
     * @author Ruediger Lunde
     */
    public class ActionValuePair<ACTION> : IComparable<ActionValuePair<ACTION>>
    {
        private ACTION action;
        private double value;

        public static ActionValuePair<ACTION> createFor(ACTION action, double utility)
        {
            return new ActionValuePair<ACTION>(action, utility);
        }

        public ActionValuePair(ACTION action, double utility)
        {
            this.action = action;
            this.value = utility;
        }

        public ACTION getAction()
        {
            return action;
        }

        public double getValue()
        {
            return value;
        }

        public int CompareTo(ActionValuePair<ACTION> pair)
        {
            if (value < pair.value)
                return 1;
            else if (value > pair.value)
                return -1;
            else
                return 0;
        }
    }
}
