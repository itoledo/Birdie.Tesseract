namespace tvn.cosine.ai.logic.fol.kb
{
    /**
     * A First Order Logic (FOL) Knowledge Base.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLKnowledgeBase
    {

        private FOLParser parser;
        private InferenceProcedure inferenceProcedure;
        private Unifier unifier;
        private SubstVisitor substVisitor;
        private VariableCollector variableCollector;
        private StandardizeApart standardizeApart;
        private CNFConverter cnfConverter;
        //
        // Persistent data structures
        //
        // Keeps track of the Sentences in their original form as added to the
        // Knowledge base.
        private IQueue<Sentence> originalSentences = Factory.CreateQueue<Sentence>();
        // The KB in clause form
        private ISet<Clause> clauses = Factory.CreateSet<Clause>();
        // Keep track of all of the definite clauses in the database
        // along with those that represent implications.
        private IQueue<Clause> allDefiniteClauses = Factory.CreateQueue<Clause>();
        private IQueue<Clause> implicationDefiniteClauses = Factory.CreateQueue<Clause>();
        // All the facts in the KB indexed by Atomic Sentence name (Note: pg. 279)
        private Map<string, IQueue<Literal>> indexFacts = Factory.CreateMap<string, IQueue<Literal>>();
        // Keep track of indexical keys for uniquely standardizing apart sentences
        private StandardizeApartIndexical variableIndexical = StandardizeApartIndexicalFactory
                .newStandardizeApartIndexical('v');
        private StandardizeApartIndexical queryIndexical = StandardizeApartIndexicalFactory
                .newStandardizeApartIndexical('q');

        //
        // PUBLIC METHODS
        //
        public FOLKnowledgeBase(FOLDomain domain)
        {
            // Default to Full Resolution if not set.
            this(domain, new FOLOTTERLikeTheoremProver());
        }

        public FOLKnowledgeBase(FOLDomain domain,
                InferenceProcedure inferenceProcedure)
        {
            this(domain, inferenceProcedure, new Unifier());
        }

        public FOLKnowledgeBase(FOLDomain domain,
                InferenceProcedure inferenceProcedure, Unifier unifier)
        {
            this.parser = new FOLParser(new FOLDomain(domain));
            this.inferenceProcedure = inferenceProcedure;
            this.unifier = unifier;
            //
            this.substVisitor = new SubstVisitor();
            this.variableCollector = new VariableCollector();
            this.standardizeApart = new StandardizeApart(variableCollector,
                    substVisitor);
            this.cnfConverter = new CNFConverter(parser);
        }

        public void clear()
        {
            this.originalSentences.Clear();
            this.clauses.Clear();
            this.allDefiniteClauses.Clear();
            this.implicationDefiniteClauses.Clear();
            this.indexFacts.Clear();
        }

        public InferenceProcedure getInferenceProcedure()
        {
            return inferenceProcedure;
        }

        public void setInferenceProcedure(InferenceProcedure inferenceProcedure)
        {
            if (null != inferenceProcedure)
            {
                this.inferenceProcedure = inferenceProcedure;
            }
        }

        public Sentence tell(string sentence)
        {
            Sentence s = parser.parse(sentence);
            tell(s);
            return s;
        }

        public void tell(IQueue<? : Sentence> sentences)
        {
            for (Sentence s : sentences)
            {
                tell(s);
            }
        }

        public void tell(Sentence sentence)
        {
            store(sentence);
        }

        /**
         * 
         * @param querySentence
         * @return an InferenceResult.
         */
        public InferenceResult ask(string querySentence)
        {
            return ask(parser.parse(querySentence));
        }

        public InferenceResult ask(Sentence query)
        {
            // Want to standardize apart the query to ensure
            // it does not clash with any of the sentences
            // in the database
            StandardizeApartResult saResult = standardizeApart.standardizeApart(
                    query, queryIndexical);

            // Need to map the result variables (as they are standardized apart)
            // to the original queries variables so that the caller can easily
            // understand and use the returned set of substitutions
            InferenceResult infResult = getInferenceProcedure().ask(this,
                    saResult.getStandardized());
            for (Proof p : infResult.getProofs())
            {
                Map<Variable, Term> im = p.getAnswerBindings();
                Map<Variable, Term> em = Factory.CreateMap<Variable, Term>();
                for (Variable rev : saResult.getReverseSubstitution().GetKeys())
                {
                    em.Put((Variable)saResult.getReverseSubstitution().Get(rev),
                            im.Get(rev));
                }
                p.replaceAnswerBindings(em);
            }

            return infResult;
        }

        public int getNumberFacts()
        {
            return allDefiniteClauses.size() - implicationDefiniteClauses.size();
        }

        public int getNumberRules()
        {
            return clauses.size() - getNumberFacts();
        }

        public IQueue<Sentence> getOriginalSentences()
        {
            return Factory.CreateReadOnlyQueue<>(originalSentences);
        }

        public IQueue<Clause> getAllDefiniteClauses()
        {
            return Factory.CreateReadOnlyQueue<>(allDefiniteClauses);
        }

        public IQueue<Clause> getAllDefiniteClauseImplications()
        {
            return Factory.CreateReadOnlyQueue<>(implicationDefiniteClauses);
        }

        public ISet<Clause> getAllClauses()
        {
            return Factory.CreateReadOnlySet<>(clauses);
        }

        // Note: pg 278, FETCH(q) concept.
        public synchronized ISet<IMap<Variable, Term>> fetch(Literal l) {
            // Get all of the substitutions in the KB that p unifies with
            ISet<IMap<Variable, Term>> allUnifiers = Factory.CreateSet<IMap<Variable, Term>>();

            IQueue<Literal> matchingFacts = fetchMatchingFacts(l);
            if (null != matchingFacts)
            {
                for (Literal fact : matchingFacts)
                {
                    Map<Variable, Term> substitution = unifier.unify(
                            l.getAtomicSentence(), fact.getAtomicSentence());
                    if (null != substitution)
                    {
                        allUnifiers.Add(substitution);
                    }
                }
            }

            return allUnifiers;
        }

        // Note: To support FOL-FC-Ask
        public ISet<IMap<Variable, Term>> fetch(IQueue<Literal> literals)
        {
            ISet<IMap<Variable, Term>> possibleSubstitutions = Factory.CreateSet<IMap<Variable, Term>>();

            if (literals.size() > 0)
            {
                Literal first = literals.Get(0);
                IQueue<Literal> rest = literals.subList(1, literals.size());

                recursiveFetch(Factory.CreateMap<Variable, Term>(), first, rest,
                        possibleSubstitutions);
            }

            return possibleSubstitutions;
        }

        public Map<Variable, Term> unify(FOLNode x, FOLNode y)
        {
            return unifier.unify(x, y);
        }

        public Sentence subst(IMap<Variable, Term> theta, Sentence aSentence)
        {
            return substVisitor.subst(theta, aSentence);
        }

        public Literal subst(IMap<Variable, Term> theta, Literal l)
        {
            return substVisitor.subst(theta, l);
        }

        public Term subst(IMap<Variable, Term> theta, Term term)
        {
            return substVisitor.subst(theta, term);
        }

        // Note: see page 277.
        public Sentence standardizeApart(Sentence sentence)
        {
            return standardizeApart.standardizeApart(sentence, variableIndexical)
                    .getStandardized();
        }

        public Clause standardizeApart(Clause clause)
        {
            return standardizeApart.standardizeApart(clause, variableIndexical);
        }

        public Chain standardizeApart(Chain chain)
        {
            return standardizeApart.standardizeApart(chain, variableIndexical);
        }

        public ISet<Variable> collectAllVariables(Sentence sentence)
        {
            return variableCollector.collectAllVariables(sentence);
        }

        public CNF convertToCNF(Sentence sentence)
        {
            return cnfConverter.convertToCNF(sentence);
        }

        public ISet<Clause> convertToClauses(Sentence sentence)
        {
            CNF cnf = cnfConverter.convertToCNF(sentence);

            return Factory.CreateSet<Clause>(cnf.getConjunctionOfClauses());
        }

        public Literal createAnswerLiteral(Sentence forQuery)
        {
            string alName = parser.getFOLDomain().addAnswerLiteral();
            IQueue<Term> terms = Factory.CreateQueue<Term>();

            ISet<Variable> vars = variableCollector.collectAllVariables(forQuery);
            for (Variable v : vars)
            {
                // Ensure copies of the variables are used.
                terms.Add(v.copy());
            }

            return new Literal(new Predicate(alName, terms));
        }

        // Note: see pg. 281
        public bool isRenaming(Literal l)
        {
            IQueue<Literal> possibleMatches = fetchMatchingFacts(l);
            if (null != possibleMatches)
            {
                return isRenaming(l, possibleMatches);
            }

            return false;
        }

        // Note: see pg. 281
        public bool isRenaming(Literal l, IQueue<Literal> possibleMatches)
        {

            for (Literal q : possibleMatches)
            {
                if (l.isPositiveLiteral() != q.isPositiveLiteral())
                {
                    continue;
                }
                Map<Variable, Term> subst = unifier.unify(l.getAtomicSentence(),
                        q.getAtomicSentence());
                if (null != subst)
                {
                    int cntVarTerms = 0;
                    for (Term t : subst.values())
                    {
                        if (t is Variable) {
                cntVarTerms++;
            }
        }
				// If all the substitutions, even if none, map to Variables
				// then this is a renaming
				if (subst.size() == cntVarTerms) {
					return true;
				}
			}
}

		return false;
	}

	 
    public override string ToString()
{
    StringBuilder sb = new StringBuilder();
    for (Sentence s : originalSentences)
    {
        sb.Append(s.ToString());
        sb.Append("\n");
    }
    return sb.ToString();
}

//
// PROTECTED METHODS
//

protected FOLParser getParser()
{
    return parser;
}

//
// PRIVATE METHODS
//

// Note: pg 278, STORE(s) concept.
private synchronized void store(Sentence sentence)
{
    originalSentences.Add(sentence);

    // Convert the sentence to CNF
    CNF cnfOfOrig = cnfConverter.convertToCNF(sentence);
    for (Clause c : cnfOfOrig.getConjunctionOfClauses())
    {
        c.setProofStep(new ProofStepClauseClausifySentence(c, sentence));
        if (c.isEmpty())
        {
            // This should not happen, if so the user
            // is trying to add an unsatisfiable sentence
            // to the KB.
            throw new IllegalArgumentException(
                    "Attempted to add unsatisfiable sentence to KB, orig=["
                            + sentence + "] CNF=" + cnfOfOrig);
        }

        // Ensure all clauses added to the KB are Standardized Apart.
        c = standardizeApart.standardizeApart(c, variableIndexical);

        // Will make all clauses immutable
        // so that they cannot be modified externally.
        c.setImmutable();
        if (clauses.Add(c))
        {
            // If added keep track of special types of
            // clauses, as useful for query purposes
            if (c.isDefiniteClause())
            {
                allDefiniteClauses.Add(c);
            }
            if (c.isImplicationDefiniteClause())
            {
                implicationDefiniteClauses.Add(c);
            }
            if (c.isUnitClause())
            {
                indexFact(c.getLiterals().iterator().next());
            }
        }
    }
}

// Only if it is a unit clause does it get indexed as a fact
// see pg. 279 for general idea.
private void indexFact(Literal fact)
{
    string factKey = getFactKey(fact);
    if (!indexFacts.containsKey(factKey))
    {
        indexFacts.Put(factKey, Factory.CreateQueue<Literal>());
    }

    indexFacts.Get(factKey).Add(fact);
}

private void recursiveFetch(IMap<Variable, Term> theta, Literal l,
        IQueue<Literal> remainingLiterals,
        ISet<IMap<Variable, Term>> possibleSubstitutions)
{

    // Find all substitutions for current predicate based on the
    // substitutions of prior predicates in the list (i.e. SUBST with
    // theta).
    ISet<IMap<Variable, Term>> pSubsts = fetch(subst(theta, l));

    // No substitutions, therefore cannot continue
    if (null == pSubsts)
    {
        return;
    }

    for (IMap<Variable, Term> psubst : pSubsts)
    {
        // Ensure all prior substitution information is maintained
        // along the chain of predicates (i.e. for shared variables
        // across the predicates).
        psubst.putAll(theta);
        if (remainingLiterals.size() == 0)
        {
            // This means I am at the end of the chain of predicates
            // and have found a valid substitution.
            possibleSubstitutions.Add(psubst);
        }
        else
        {
            // Need to move to the next link in the chain of substitutions
            Literal first = remainingLiterals.Get(0);
            IQueue<Literal> rest = remainingLiterals.subList(1,
                    remainingLiterals.size());

            recursiveFetch(psubst, first, rest, possibleSubstitutions);
        }
    }
}

private IQueue<Literal> fetchMatchingFacts(Literal l)
{
    return indexFacts.Get(getFactKey(l));
}

private string getFactKey(Literal l)
{
    StringBuilder key = new StringBuilder();
    if (l.isPositiveLiteral())
    {
        key.Append("+");
    }
    else
    {
        key.Append("-");
    }
    key.Append(l.getAtomicSentence().getSymbolicName());

    return key.ToString();
}
}
}
