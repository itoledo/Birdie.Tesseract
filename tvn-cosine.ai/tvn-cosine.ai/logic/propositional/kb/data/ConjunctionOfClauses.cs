using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.logic.propositional.kb.data
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
            this.clauses.addAll(conjunctionOfClauses);
            //TODO  Make immutable
            this.clauses = Collections.unmodifiableSet(this.clauses);
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
        public Set<Clause> getClauses()
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
        public ConjunctionOfClauses extend(Collection<Clause> additionalClauses)
        {
            Set<Clause> extendedClauses = new LinkedHashSet<Clause>();
            extendedClauses.addAll(clauses);
            extendedClauses.addAll(additionalClauses);

            ConjunctionOfClauses result = new ConjunctionOfClauses(extendedClauses);

            return result;
        }

        @Override
        public String toString()
        {
            if (cachedStringRep == null)
            {
                StringBuilder sb = new StringBuilder();
                boolean first = true;
                sb.append("{");
                for (Clause c : clauses)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.append(", ");
                    }
                    sb.append(c);
                }
                sb.append("}");
                cachedStringRep = sb.toString();
            }

            return cachedStringRep;
        }

        @Override
        public int hashCode()
        {
            if (cachedHashCode == -1)
            {
                cachedHashCode = clauses.hashCode();
            }
            return cachedHashCode;
        }

        @Override
        public boolean equals(Object othObj)
        {
            if (null == othObj)
            {
                return false;
            }
            if (this == othObj)
            {
                return true;
            }
            if (!(othObj instanceof ConjunctionOfClauses)) {
                return false;
            }
            ConjunctionOfClauses othConjunctionOfClauses = (ConjunctionOfClauses)othObj;

            return othConjunctionOfClauses.clauses.equals(this.clauses);
        }
    }

}
