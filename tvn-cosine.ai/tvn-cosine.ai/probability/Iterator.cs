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
    public interface Iterator<T>
    {
        /**
         * Called for each possible assignment for the Random Variables
         * comprising this Factor.
         * 
         * @param possibleAssignment
         *            a possible assignment, &omega;, of variable/value pairs.
         * @param value
         *            the value associated with &omega;
         */
        void iterate(IDictionary<RandomVariable, T> possibleAssignment, double value);
    }
}
