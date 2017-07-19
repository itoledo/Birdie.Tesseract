﻿namespace tvn.cosine.ai.logic.propositional.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253.<br>
     * <br>
     * A conjunction of clauses, where each clause is a disjunction of literals.
     * Here we represent a conjunction of clauses as a set of clauses, where each
     * clause is a set of literals. In addition, a conjunction of clauses, as
     * implemented, are immutable.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ConjunctionOfClauses
    {
        private ISet<Clause> clauses = Factory.CreateSet<Clause>();
        //
        private string cachedStringRep = null;
        private int cachedHashCode = -1;

        /**
         * Constructor.
         * 
         * @param conjunctionOfClauses
         *            a collection of clauses that represent a conjunction.
         */
        public ConjunctionOfClauses(IQueue<Clause> conjunctionOfClauses)
        {
            this.clauses.AddAll(conjunctionOfClauses);
            // Make immutable
            this.clauses = Factory.CreateReadOnlySet<>(this.clauses);
        }

        /**
         * 
         * @return the number of clauses contained by this conjunction.
         */
        public int getNumberOfClauses()
        {
            return clauses.size();
        }

        /**
         * 
         * @return the set of clauses contained by this conjunction.
         */
        public ISet<Clause> getClauses()
        {
            return clauses;
        }

        /**
         * Create a new conjunction of clauses by taking the clauses from the
         * current conjunction and adding additional clauses to it.
         * 
         * @param additionalClauses
         *            the additional clauses to be added to the existing set of
         *            clauses in order to create a new conjunction.
         * @return a new conjunction of clauses containing the existing and
         *         additional clauses passed in.
         */
        public ConjunctionOfClauses extend(IQueue<Clause> additionalClauses)
        {
            ISet<Clause> extendedClauses = Factory.CreateSet<Clause>();
            extendedClauses.AddAll(clauses);
            extendedClauses.AddAll(additionalClauses);

            ConjunctionOfClauses result = new ConjunctionOfClauses(extendedClauses);

            return result;
        }

         
        public override string ToString()
        {
            if (cachedStringRep == null)
            {
                StringBuilder sb = new StringBuilder();
                bool first = true;
                sb.Append("{");
                for (Clause c : clauses)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(", ");
                    }
                    sb.Append(c);
                }
                sb.Append("}");
                cachedStringRep = sb.ToString();
            }

            return cachedStringRep;
        }

         
        public override int GetHashCode()
        {
            if (cachedHashCode == -1)
            {
                cachedHashCode = clauses.GetHashCode();
            }
            return cachedHashCode;
        }

         
        public bool equals(object othObj)
        {
            if (null == othObj)
            {
                return false;
            }
            if (this == othObj)
            {
                return true;
            }
            if (!(othObj is ConjunctionOfClauses)) {
                return false;
            }
            ConjunctionOfClauses othConjunctionOfClauses = (ConjunctionOfClauses)othObj;

            return othConjunctionOfClauses.clauses.Equals(this.clauses);
        }
    }
}
