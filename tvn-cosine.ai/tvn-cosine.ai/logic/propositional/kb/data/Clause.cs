namespace tvn.cosine.ai.logic.propositional.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253.<br>
     * <br>
     * A Clause: A disjunction of literals. Here we view a Clause as a set of
     * literals. This respects the restriction, under resolution, that a resulting
     * clause should contain only 1 copy of a resulting literal. In addition,
     * clauses, as implemented, are immutable.
     * 
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class Clause
    {
        public static final Clause EMPTY = new Clause();
        //
        private ISet<Literal> literals = Factory.CreateSet<Literal>();
        //
        private ISet<PropositionSymbol> cachedPositiveSymbols = Factory.CreateSet<PropositionSymbol>();
        private ISet<PropositionSymbol> cachedNegativeSymbols = Factory.CreateSet<PropositionSymbol>();
        private ISet<PropositionSymbol> cachedSymbols = Factory.CreateSet<PropositionSymbol>();
        //
        private Boolean cachedIsTautologyResult = null;
        private string cachedStringRep = null;
        private int cachedHashCode = -1;

        /**
         * Default constructor - i.e. the empty clause, which is 'False'.
         */
        public Clause()
        {
            // i.e. the empty clause
            this(Factory.CreateQueue<Literal>());
        }

        /**
         * Construct a clause from the given literals. Note: literals the are always
         * 'False' (i.e. False or ~True) are not added to the instantiated clause.
         * 
         * @param literals
         *            the literals to be added to the clause.
         */
        public Clause(Literal...literals)
        {
            this(Arrays.asList(literals));
        }

        /**
         * Construct a clause from the given literals. Note: literals the are always
         * 'False' (i.e. False or ~True) are not added to the instantiated clause.
         * 
         * @param literals
         */
        public Clause(Collection<Literal> literals)
        {
            for (Literal l : literals)
            {
                if (l.isAlwaysFalse())
                {
                    // Don't add literals of the form
                    // False | ~True
                    continue;
                }
                if (this.literals.Add(l))
                {
                    // Only add to caches if not already added
                    if (l.isPositiveLiteral())
                    {
                        this.cachedPositiveSymbols.Add(l.getAtomicSentence());
                    }
                    else
                    {
                        this.cachedNegativeSymbols.Add(l.getAtomicSentence());
                    }
                }
            }

            cachedSymbols.addAll(cachedPositiveSymbols);
            cachedSymbols.addAll(cachedNegativeSymbols);

            // Make immutable
            this.literals = Factory.CreateReadOnlySet<>(this.literals);
            cachedSymbols = Factory.CreateReadOnlySet<>(cachedSymbols);
            cachedPositiveSymbols = Collections
                    .unmodifiableSet(cachedPositiveSymbols);
            cachedNegativeSymbols = Collections
                    .unmodifiableSet(cachedNegativeSymbols);
        }

        /**
         * If a clause is empty - a disjunction of no disjuncts - it is equivalent
         * to 'False' because a disjunction is true only if at least one of its
         * disjuncts is true.
         * 
         * @return true if an empty clause, false otherwise.
         */
        public bool isFalse()
        {
            return isEmpty();
        }

        /**
         * 
         * @return true if the clause is empty (i.e. 'False'), false otherwise.
         */
        public bool isEmpty()
        {
            return literals.size() == 0;
        }

        /**
         * Determine if a clause is unit, i.e. contains a single literal.
         * 
         * @return true if the clause is unit, false otherwise.
         */
        public bool isUnitClause()
        {
            return literals.size() == 1;
        }

        /**
         * Determine if a definite clause. A definite clause is a disjunction of
         * literals of which <i>exactly one is positive</i>. <q>For example, the
         * clause (&not;L<sub>1,1</sub> &or; &not;Breeze &or; B<sub>1,1</sub>) is a
         * definite clause, whereas (&not;B<sub>1,1</sub> &or; P<sub>1,2</sub> &or;
         * P<sub>2,1</sub>) is not.</q>
         * 
         * 
         * @return true if a definite clause, false otherwise.
         */
        public bool isDefiniteClause()
        {
            return cachedPositiveSymbols.size() == 1;
        }

        /**
         * Determine if an implication definite clause. An implication definite
         * clause is disjunction of literals of which exactly 1 is positive and
         * there is 1 or more negative literals.
         * 
         * @return true if an implication definite clause, false otherwise.
         */
        public bool isImplicationDefiniteClause()
        {
            return isDefiniteClause() && cachedNegativeSymbols.size() >= 1;
        }

        /**
         * Determine if a Horn clause. A horn clause is a disjunction of literals of
         * which <i>at most one is positive</i>.
         * 
         * @return true if a Horn clause, false otherwise.
         */
        public bool isHornClause()
        {
            return !isEmpty() && cachedPositiveSymbols.size() <= 1;
        }

        /**
         * Clauses with no positive literals are called <b>goal clauses</b>.
         * 
         * @return true if a Goal clause, false otherwise.
         */
        public bool isGoalClause()
        {
            return !isEmpty() && cachedPositiveSymbols.size() == 0;
        }

        /**
         * Determine if the clause represents a tautology, of which the following
         * are examples:<br>
         * 
         * <pre>
         * {..., True, ...}
         * {..., ~False, ...} 
         * {..., P, ..., ~P, ...}
         * </pre>
         * 
         * @return true if the clause represents a tautology, false otherwise.
         */
        public bool isTautology()
        {
            if (cachedIsTautologyResult == null)
            {
                for (Literal l : literals)
                {
                    if (l.isAlwaysTrue())
                    {
                        // {..., True, ...} is a tautology.
                        // {..., ~False, ...} is a tautology
                        cachedIsTautologyResult = true;
                    }
                }
                // If we still don't know
                if (cachedIsTautologyResult == null)
                {
                    if (SetOps.intersection(cachedPositiveSymbols, cachedNegativeSymbols)
                            .size() > 0)
                    {
                        // We have:
                        // P | ~P
                        // which is always true.
                        cachedIsTautologyResult = true;
                    }
                    else
                    {
                        cachedIsTautologyResult = false;
                    }
                }
            }

            return cachedIsTautologyResult;
        }

        /**
         * 
         * @return the number of literals contained by the clause.
         */
        public int getNumberLiterals()
        {
            return literals.size();
        }

        /**
         * 
         * @return the number of positive literals contained by the clause.
         */
        public int getNumberPositiveLiterals()
        {
            return cachedPositiveSymbols.size();
        }

        /**
         * 
         * @return the number of negative literals contained by the clause.
         */
        public int getNumberNegativeLiterals()
        {
            return cachedNegativeSymbols.size();
        }

        /**
         * 
         * @return the set of literals making up the clause.
         */
        public ISet<Literal> getLiterals()
        {
            return literals;
        }

        /**
         * 
         * @return the set of symbols from the clause's positive and negative literals.
         */
        public ISet<PropositionSymbol> getSymbols()
        {
            return cachedSymbols;
        }

        /**
         * 
         * @return the set of symbols from the clause's positive literals.
         */
        public ISet<PropositionSymbol> getPositiveSymbols()
        {
            return cachedPositiveSymbols;
        }

        /**
         * 
         * @return the set of symbols from the clause's negative literals.
         */
        public ISet<PropositionSymbol> getNegativeSymbols()
        {
            return cachedNegativeSymbols;
        }

         
        public override string ToString()
        {
            if (cachedStringRep == null)
            {
                StringBuilder sb = new StringBuilder();
                bool first = true;
                sb.Append("{");
                for (Literal l : literals)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(", ");
                    }
                    sb.Append(l);
                }
                sb.Append("}");
                cachedStringRep = sb.ToString();
            }
            return cachedStringRep;
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
            if (!(othObj is Clause)) {
                return false;
            }
            Clause othClause = (Clause)othObj;

            return othClause.literals.Equals(this.literals);
        }

         
        public override int GetHashCode()
        {
            if (cachedHashCode == -1)
            {
                cachedHashCode = literals.GetHashCode();
            }
            return cachedHashCode;
        }
    }
}
