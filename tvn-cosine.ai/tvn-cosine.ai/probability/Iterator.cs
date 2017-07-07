using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.probability
{
    /**
     * Interface to be implemented by an object/algorithm that wishes to iterate
     * over the possible assignments for the random variables comprising this
     * Factor.
     * 
     * @see Factor#iterateOver(Iterator)
     * @see Factor#iterateOver(Iterator, AssignmentProposition...)
     */
    public class Iterator<T>
    {
        public Iterator(Action<IDictionary<RandomVariable, T>, double> iterate)
        {
            this.iterate = iterate;
        }

        /**
         * Called for each possible assignment for the Random Variables
         * comprising this Factor.
         * 
         * @param possibleAssignment
         *            a possible assignment, &omega;, of variable/value pairs.
         * @param value
         *            the value associated with &omega;
         */
        public Action<IDictionary<RandomVariable, T>, double> iterate { get; }
    }
}
