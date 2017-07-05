using System.Collections.Generic;

namespace tvn.cosine.ai.search.nondeterministic
{
    /**
     * Represents the path the agent travels through the AND-OR tree (see figure
     * 4.10, page 135, AIMA3e).
     * 
     * @author Andrew Brown
     */
    public class Path : List<object>
    {

        /**
         * Creating a new path based on this path and the passed in appended states.
         * 
         * @param states
         *            the states to append to a new copy of this path.
         * 
         * @return a new Path that contains this path's states along with the passed
         *         in argument states appended to the end.
         */
        public Path append(IEnumerable<object> states)
        {
            Path appendedPath = new Path();
            appendedPath.AddRange(this);
            appendedPath.AddRange(states);
            return appendedPath;
        }

        /**
         * Create a new path based on the passed in prepended state and this path's
         * current states.
         * 
         * @param state
         *            the state to be prepended.
         * @return a new Path that contains the passed in state along with this
         *         path's current states.
         */
        public Path prepend(object state)
        {
            Path prependedPath = new Path();
            prependedPath.Add(state);
            prependedPath.AddRange(this);

            return prependedPath;
        }
    }

}
