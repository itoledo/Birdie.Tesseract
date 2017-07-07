namespace tvn.cosine.ai.probability.bayes
{
    /**
     * A node over a Random Variable that has a finite countable domain.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public interface FiniteNode<T> : DiscreteNode<T>
    { 
        /**
         * 
         * @return the Conditional Probability Table detailing the finite set of
         *         probabilities for this Node.
         */
        ConditionalProbabilityTable<T> getCPT();
    }
}
