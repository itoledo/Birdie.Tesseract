using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.propositional.kb.data;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn.cosine.ai.logic.propositional.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 263.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function WALKSAT(clauses, p, max_flips) returns a satisfying model or failure
     *   inputs: clauses, a set of clauses in propositional logic
     *           p, the probability of choosing to do a "random walk" move, typically around 0.5
     *           max_flips, number of flips allowed before giving up
     *           
     *   model <- a random assignment of true/false to the symbols in clauses
     *   for i = 1 to max_flips do
     *       if model satisfies clauses then return model
     *       clause <- a randomly selected clause from clauses that is false in model
     *       with probability p flip the value in model of a randomly selected symbol from clause
     *       else flip whichever symbol in clause maximizes the number of satisfied clauses
     *   return failure
     * </code>
     * </pre>
     * 
     * Figure 7.18 The WALKSAT algorithm for checking satisfiability by randomly
     * flipping the values of variables. Many versions of the algorithm exist.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class WalkSAT
    {

        /**
         * WALKSAT(clauses, p, max_flips)<br>
         * 
         * @param clauses
         *            a set of clauses in propositional logic
         * @param p
         *            the probability of choosing to do a "random walk" move,
         *            typically around 0.5
         * @param maxFlips
         *            number of flips allowed before giving up. Note: a value < 0 is
         *            interpreted as infinity.
         * 
         * @return a satisfying model or failure (null).
         */
        public Model walkSAT(ISet<Clause> clauses, double p, int maxFlips)
        {
            assertLegalProbability(p);

            // model <- a random assignment of true/false to the symbols in clauses
            Model model = randomAssignmentToSymbolsInClauses(clauses);
            // for i = 1 to max_flips do (Note: maxFlips < 0 means infinity)
            for (int i = 0; i < maxFlips || maxFlips < 0; i++)
            {
                // if model satisfies clauses then return model
                if (model.satisfies(clauses))
                {
                    return model;
                }

                // clause <- a randomly selected clause from clauses that is false
                // in model
                Clause clause = randomlySelectFalseClause(clauses, model);

                // with probability p flip the value in model of a randomly selected
                // symbol from clause
                if (random.NextDouble() < p)
                {
                    model = model.flip(randomlySelectSymbolFromClause(clause));
                }
                else
                {
                    // else flip whichever symbol in clause maximizes the number of
                    // satisfied clauses
                    model = flipSymbolInClauseMaximizesNumberSatisfiedClauses(
                            clause, clauses, model);
                }
            }

            // return failure
            return null;
        }

        //
        // SUPPORTING CODE
        //
        private Random random = new Random();

        /**
         * Default Constructor.
         */
        public WalkSAT()
        {
        }

        /**
         * Constructor.
         * 
         * @param random
         *            the random generator to be used by the algorithm.
         */
        public WalkSAT(Random random)
        {
            this.random = random;
        }

        //
        // PROTECTED
        //
        protected void assertLegalProbability(double p)
        {
            if (p < 0 || p > 1)
            {
                throw new ArgumentException("p is not a legal propbability value [0-1]: " + p);
            }
        }

        protected Model randomAssignmentToSymbolsInClauses(ISet<Clause> clauses)
        {
            // Collect the symbols in clauses
            ISet<PropositionSymbol> symbols = new HashSet<PropositionSymbol>();
            foreach (Clause c in clauses)
            {
                foreach (var v in c.getSymbols())
                    symbols.Add(v);
            }

            // Make initial set of assignments
            IDictionary<PropositionSymbol, bool?> values = new Dictionary<PropositionSymbol, bool?>();
            foreach (PropositionSymbol symbol in symbols)
            {
                // a random assignment of true/false to the symbols in clauses
                values.Add(symbol, random.Next(2) == 1 ? true : false);
            }

            Model result = new Model(values);

            return result;
        }

        protected Clause randomlySelectFalseClause(ISet<Clause> clauses, Model model)
        {
            // Collect the clauses that are false in the model
            List<Clause> falseClauses = new List<Clause>();
            foreach (Clause c in clauses)
            {
                if (!Equals(model.determineValue(c)))
                {
                    falseClauses.Add(c);
                }
            }

            // a randomly selected clause from clauses that is false
            Clause result = falseClauses[random.Next(falseClauses.Count)];
            return result;
        }

        protected PropositionSymbol randomlySelectSymbolFromClause(Clause clause)
        {
            // all the symbols in clause
            ISet<PropositionSymbol> symbols = clause.getSymbols();

            // a randomly selected symbol from clause
            PropositionSymbol result = (new List<PropositionSymbol>(symbols))[random.Next(symbols.Count)];
            return result;
        }

        protected Model flipSymbolInClauseMaximizesNumberSatisfiedClauses(
                Clause clause, ISet<Clause> clauses, Model model)
        {
            Model result = model;

            // all the symbols in clause
            ISet<PropositionSymbol> symbols = clause.getSymbols();
            int maxClausesSatisfied = -1;
            foreach (PropositionSymbol symbol in symbols)
            {
                Model flippedModel = result.flip(symbol);
                int numberClausesSatisfied = 0;
                foreach (Clause c in clauses)
                {
                    if (Equals(flippedModel.determineValue(c)))
                    {
                        numberClausesSatisfied++;
                    }
                }
                // test if this symbol flip is the new maximum
                if (numberClausesSatisfied > maxClausesSatisfied)
                {
                    result = flippedModel;
                    maxClausesSatisfied = numberClausesSatisfied;
                    if (numberClausesSatisfied == clauses.Count)
                    {
                        // i.e. satisfies all clauses
                        break; // this is our goal.
                    }
                }
            }

            return result;
        }
    } 
}
