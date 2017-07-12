using System.Collections.Generic;

namespace tvn.cosine.ai.search.local
{
    /**
     * Variant of the genetic algorithm which uses double numbers from a fixed
     * interval instead of symbols from a finite alphabet in the representations of
     * individuals. Reproduction uses values somewhere between the values of the
     * parents. Mutation adds some random offset. Progress tracer implementations
     * can be used to get informed about the running iterations.  
     * A typical use case for this genetic algorithm version is finding maximums in
     * a given mathematical (fitness) function.
     * 
     * @author Ruediger Lunde
     *
     */
    public class GeneticAlgorithmForNumbers : GeneticAlgorithm<double>
    {
        private double minimum;
        private double maximum;

        /**
         * Constructor.
         * 
         * @param individualLength
         *            vector length used for the representations of individuals. Use
         *            1 for analysis of functions f(x).
         * @param min
         *            minimal value to be used in vector elements.
         * @param max
         *            maximal value to be used in vector elements.
         * @param mutationProbability
         *            probability of mutations.
         */
        public GeneticAlgorithmForNumbers(int individualLength, double min, double max, double mutationProbability)
            : base(individualLength, new List<double>(), mutationProbability)
        {
            minimum = min;
            maximum = max;
        }

        /** Convenience method. */
        public Individual<double> createRandomIndividual()
        {
            List<double> representation = new List<double>(individualLength);
            for (int i = 0; i < individualLength; i++)
                representation.Add(minimum + random.NextDouble() * (maximum - minimum));
            return new Individual<double>(representation);
        }

        /**
         * Produces for each number in the descendant's representation a random
         * value between the corresponding values of its parents.
         */

        protected override Individual<double> reproduce(Individual<double> x, Individual<double> y)
        {
            List<double> newRep = new List<double>(x.length());
            double r = random.NextDouble();
            for (int i = 0; i < x.length(); i++)
                newRep.Add(x.getRepresentation()[i] * r + y.getRepresentation()[i] * (1 - r));
            return new Individual<double>(newRep);
        }

        /**
         * Changes each component in the representation by random. The maximum
         * change is +- (maximum - minimum) / 4, but smaller changes have a higher
         * likelihood.
         */

        protected override Individual<double> mutate(Individual<double> child)
        {
            IList<double> rep = child.getRepresentation();
            IList<double> newRep = new List<double>();
            foreach (double n in rep)
            {
                double num = n;
                double r = random.NextDouble() - 0.5;
                num += r * r * r * (maximum - minimum) / 2;
                if (num < minimum)
                    num = minimum;
                else if (num > maximum)
                    num = maximum;
                newRep.Add(num);
            }
            return new Individual<double>(newRep);
        }
    }

}
