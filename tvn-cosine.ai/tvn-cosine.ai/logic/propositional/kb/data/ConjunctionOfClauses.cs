using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.logic.propositional.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253. 
     *  
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
        private ISet<Clause> clauses = new HashSet<Clause>();
        //
        private string cachedStringRep = null;
        private int cachedHashCode = -1;

        /**
         * Constructor.
         * 
         * @param conjunctionOfClauses
         *            a collection of clauses that represent a conjunction.
         */
        public ConjunctionOfClauses(ISet<Clause> conjunctionOfClauses)
        {
            foreach (var v in conjunctionOfClauses)
                this.clauses.Add(v);
            //TODO  Make immutable
            // this.clauses = Collections.unmodifiableSet(this.clauses);
        }

        /**
         * 
         * @return the number of clauses contained by this conjunction.
         */
        public int getNumberOfClauses()
        {
            return clauses.Count;
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
        public ConjunctionOfClauses extend(ICollection<Clause> additionalClauses)
        {
            ISet<Clause> extendedClauses = new HashSet<Clause>();
            foreach (var v in clauses)
                extendedClauses.Add(v);
            foreach (var v in additionalClauses)
                extendedClauses.Add(v);

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
                foreach (Clause c in clauses)
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
         
        public override bool Equals(object othObj)
        {
            if (null == othObj)
            {
                return false;
            }
            if (this == othObj)
            {
                return true;
            }
            if (!(othObj is ConjunctionOfClauses))
            {
                return false;
            }
            ConjunctionOfClauses othConjunctionOfClauses = (ConjunctionOfClauses)othObj;

            return othConjunctionOfClauses.clauses.Equals(this.clauses);
        }
    }

}
