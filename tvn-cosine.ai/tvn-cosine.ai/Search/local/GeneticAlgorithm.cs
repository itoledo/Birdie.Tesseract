using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.search.local
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.8, page
     * 129. 
     *  
     * 
     * <pre>
     * function GENETIC-ALGORITHM(population, FITNESS-FN) returns an individual
     *   inputs: population, a set of individuals
     *           FITNESS-FN, a function that measures the fitness of an individual
     *           
     *   repeat
     *     new_population &lt;- empty set
     *     for i = 1 to SIZE(population) do
     *       x &lt;- RANDOM-SELECTION(population, FITNESS-FN)
     *       y &lt;- RANDOM-SELECTION(population, FITNESS-FN)
     *       child &lt;- REPRODUCE(x, y)
     *       if (small random probability) then child &lt;- MUTATE(child)
     *       add child to new_population
     *     population &lt;- new_population
     *   until some individual is fit enough, or enough time has elapsed
     *   return the best individual in population, according to FITNESS-FN
     * --------------------------------------------------------------------------------
     * function REPRODUCE(x, y) returns an individual
     *   inputs: x, y, parent individuals
     *   
     *   n &lt;- LENGTH(x); c &lt;- random number from 1 to n
     *   return APPEND(SUBSTRING(x, 1, c), SUBSTRING(y, c+1, n))
     * </pre>
     * 
     * Figure 4.8 A genetic algorithm. The algorithm is the same as the one
     * diagrammed in Figure 4.6, with one variation: in this more popular version,
     * each mating of two parents produces only one offspring, not two.
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * @author Ruediger Lunde
     * 
     * @param <A>
     *            the type of the alphabet used in the representation of the
     *            individuals in the population (this is to provide flexibility in
     *            terms of how a problem can be encoded).
     */
    public class GeneticAlgorithm<A>
    {
        protected const string POPULATION_SIZE = "populationSize";
        protected const string ITERATIONS = "iterations";
        protected const string TIME_IN_MILLISECONDS = "timeInMSec";
        //
        protected IDictionary<string, double> metrics = new Dictionary<string, double>();
        //
        protected int individualLength;
        protected List<A> finiteAlphabet;
        protected double mutationProbability;

        protected Random random;
        private List<ProgressTracker> progressTrackers = new List<ProgressTracker>();

        public GeneticAlgorithm(int individualLength, ICollection<A> finiteAlphabet, double mutationProbability)
            : this(individualLength, finiteAlphabet, mutationProbability, new Random())
        { }

        public GeneticAlgorithm(int individualLength, ICollection<A> finiteAlphabet, double mutationProbability,
                Random random)
        {
            this.individualLength = individualLength;
            this.finiteAlphabet = new List<A>(finiteAlphabet);
            this.mutationProbability = mutationProbability;
            this.random = random;

            Debug.Assert(this.mutationProbability >= 0.0 && this.mutationProbability <= 1.0);
        }

        /** Progress tracers can be used to display progress information. */
        public void addProgressTracer(ProgressTracker pTracer)
        {
            progressTrackers.Add(pTracer);
        }

        /**
         * Starts the genetic algorithm and stops after a specified number of
         * iterations.
         */
        public virtual Individual<A> geneticAlgorithm(ICollection<Individual<A>> initPopulation,
                FitnessFunction<A> fitnessFn, int maxIterations)
        {
            GoalTest<Individual<A>> goalTest = state => getIterations() >= maxIterations;
            return geneticAlgorithm(initPopulation, fitnessFn, goalTest, 0L);
        }

        public virtual Individual<A> geneticAlgorithm(ICollection<Individual<A>> initPopulation,
            FitnessFunction<A> fitnessFn, GoalTest<Individual<A>> goalTest, long maxTimeMilliseconds)
        {
            return geneticAlgorithm(initPopulation, fitnessFn, goalTest, maxTimeMilliseconds, CancellationToken.None);
        }
        /**
         * Template method controlling search. It returns the best individual in the
         * specified population, according to the specified FITNESS-FN and goal
         * test.
         * 
         * @param initPopulation
         *            a set of individuals
         * @param fitnessFn
         *            a function that measures the fitness of an individual
         * @param goalTest
         *            test determines whether a given individual is fit enough to
         *            return. Can be used in subclasses to implement additional
         *            termination criteria, e.g. maximum number of iterations.
         * @param maxTimeMilliseconds
         *            the maximum time in milliseconds that the algorithm is to run
         *            for (approximate). Only used if > 0L.
         * @return the best individual in the specified population, according to the
         *         specified FITNESS-FN and goal test.
         */
        // function GENETIC-ALGORITHM(population, FITNESS-FN) returns an individual
        // inputs: population, a set of individuals
        // FITNESS-FN, a function that measures the fitness of an individual
        public virtual Individual<A> geneticAlgorithm(ICollection<Individual<A>> initPopulation,
            FitnessFunction<A> fitnessFn, GoalTest<Individual<A>> goalTest, long maxTimeMilliseconds, CancellationToken cancellationToken)
        {
            Individual<A> bestIndividual = null;

            // Create a local copy of the population to work with
            List<Individual<A>> population = new List<Individual<A>>(initPopulation);
            // Validate the population and setup the instrumentation
            validatePopulation(population);
            updateMetrics(population, 0, 0L);

            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            // repeat
            int itCount = 0;
            do
            {
                population = nextGeneration(population, fitnessFn);
                bestIndividual = retrieveBestIndividual(population, fitnessFn);

                updateMetrics(population, ++itCount, sw.ElapsedMilliseconds);

                // until some individual is fit enough, or enough time has elapsed
                if (maxTimeMilliseconds > 0L && sw.ElapsedMilliseconds > maxTimeMilliseconds)
                    break;
                if (cancellationToken.IsCancellationRequested)
                    break;
            } while (!goalTest(bestIndividual));

            notifyProgressTrackers(itCount, population);
            // return the best individual in population, according to FITNESS-FN
            return bestIndividual;
        }

        public virtual Individual<A> retrieveBestIndividual(ICollection<Individual<A>> population, FitnessFunction<A> fitnessFn)
        {
            Individual<A> bestIndividual = null;
            double bestSoFarFValue = double.NegativeInfinity;

            foreach (Individual<A> individual in population)
            {
                double fValue = fitnessFn.apply(individual);
                if (fValue > bestSoFarFValue)
                {
                    bestIndividual = individual;
                    bestSoFarFValue = fValue;
                }
            }

            return bestIndividual;
        }

        /**
         * Sets the population size and number of iterations to zero.
         */
        public void clearInstrumentation()
        {
            updateMetrics(new List<Individual<A>>(), 0, 0L);
        }

        /**
         * Returns all the metrics of the genetic algorithm.
         * 
         * @return all the metrics of the genetic algorithm.
         */
        public IDictionary<string, double> getMetrics()
        {
            return metrics;
        }

        /**
         * Returns the population size.
         * 
         * @return the population size.
         */
        public int getPopulationSize()
        {
            return (int)metrics[POPULATION_SIZE];
        }

        /**
         * Returns the number of iterations of the genetic algorithm.
         * 
         * @return the number of iterations of the genetic algorithm.
         */
        public int getIterations()
        {
            return (int)metrics[ITERATIONS];
        }

        /**
         * 
         * @return the time in milliseconds that the genetic algorithm took.
         */
        public long getTimeInMilliseconds()
        {
            return (long)metrics[TIME_IN_MILLISECONDS];
        }

        /**
         * Updates statistic data collected during search.
         * 
         * @param itCount
         *            the number of iterations.
         * @param time
         *            the time in milliseconds that the genetic algorithm took.
         */
        protected virtual void updateMetrics(ICollection<Individual<A>> population, int itCount, long time)
        {
            metrics[POPULATION_SIZE] = population.Count;
            metrics[ITERATIONS] = itCount;
            metrics[TIME_IN_MILLISECONDS] = time;
        }

        //
        // PROTECTED METHODS
        //
        // Note: Override these protected methods to create your own desired
        // behavior.
        //
        /**
         * Primitive operation which is responsible for creating the next
         * generation. Override to get progress information!
         */
        protected virtual List<Individual<A>> nextGeneration(List<Individual<A>> population, FitnessFunction<A> fitnessFn)
        {
            // new_population <- empty set
            List<Individual<A>> newPopulation = new List<Individual<A>>(population.Count);
            // for i = 1 to SIZE(population) do
            for (int i = 0; i < population.Count; i++)
            {
                // x <- RANDOM-SELECTION(population, FITNESS-FN)
                Individual<A> x = randomSelection(population, fitnessFn);
                // y <- RANDOM-SELECTION(population, FITNESS-FN)
                Individual<A> y = randomSelection(population, fitnessFn);
                // child <- REPRODUCE(x, y)
                Individual<A> child = reproduce(x, y);
                // if (small random probability) then child <- MUTATE(child)
                if (random.NextDouble() <= mutationProbability)
                {
                    child = mutate(child);
                }
                // add child to new_population
                newPopulation.Add(child);
            }
            notifyProgressTrackers(getIterations(), population);
            return newPopulation;
        }

        // RANDOM-SELECTION(population, FITNESS-FN)
        protected virtual Individual<A> randomSelection(List<Individual<A>> population, FitnessFunction<A> fitnessFn)
        {
            // Default result is last individual
            // (just to avoid problems with rounding errors)
            Individual<A> selected = population[population.Count - 1];

            // Determine all of the fitness values
            double[] fValues = new double[population.Count];
            for (int i = 0; i < population.Count; i++)
            {
                fValues[i] = fitnessFn.apply(population[i]);
            }
            // Normalize the fitness values
            fValues = Util.normalize(fValues);
            double prob = random.NextDouble();
            double totalSoFar = 0.0;
            for (int i = 0; i < fValues.Length; i++)
            {
                // Are at last element so assign by default
                // in case there are rounding issues with the normalized values
                totalSoFar += fValues[i];
                if (prob <= totalSoFar)
                {
                    selected = population[i];
                    break;
                }
            }

            selected.incDescendants();
            return selected;
        }

        // function REPRODUCE(x, y) returns an individual
        // inputs: x, y, parent individuals
        protected virtual Individual<A> reproduce(Individual<A> x, Individual<A> y)
        {
            // n <- LENGTH(x);
            // Note: this is = this.individualLength
            // c <- random number from 1 to n
            int c = randomOffset(individualLength);
            // return APPEND(SUBSTRING(x, 1, c), SUBSTRING(y, c+1, n))
            List<A> childRepresentation = new List<A>();
            childRepresentation.AddRange(x.getRepresentation().Take(c));
            childRepresentation.AddRange(y.getRepresentation().Skip(c).Take(individualLength));

            return new Individual<A>(childRepresentation);
        }

        protected virtual Individual<A> mutate(Individual<A> child)
        {
            int mutateOffset = randomOffset(individualLength);
            int alphaOffset = randomOffset(finiteAlphabet.Count);

            List<A> mutatedRepresentation = new List<A>(child.getRepresentation());

            mutatedRepresentation.Insert(mutateOffset, finiteAlphabet[alphaOffset]);

            return new Individual<A>(mutatedRepresentation);
        }

        protected virtual int randomOffset(int length)
        {
            return random.Next(length);
        }

        protected virtual void validatePopulation(ICollection<Individual<A>> population)
        {
            // Require at least 1 individual in population in order
            // for algorithm to work
            if (population.Count < 1)
            {
                throw new ArgumentException("Must start with at least a population of size 1");
            }
            // string lengths are assumed to be of fixed size,
            // therefore ensure initial populations lengths correspond to this
            foreach (Individual<A> individual in population)
            {
                if (individual.length() != this.individualLength)
                {
                    throw new ArgumentException("Individual [" + individual + "] in population is not the required length of " + this.individualLength);
                }
            }
        }

        private void notifyProgressTrackers(int itCount, ICollection<Individual<A>> generation)
        {
            foreach (ProgressTracker tracer in progressTrackers)
                tracer.trackProgress(getIterations(), generation);
        }

        /**
         * Interface for progress tracers.
         * 
         * @author Ruediger Lunde
         */
        public interface ProgressTracker
        {
            void trackProgress(int itCount, ICollection<Individual<A>> population);
        }
    }
}
