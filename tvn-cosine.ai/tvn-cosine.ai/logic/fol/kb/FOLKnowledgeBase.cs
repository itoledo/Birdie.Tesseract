using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.inference;
using tvn.cosine.ai.logic.fol.inference.proof;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

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
        private StandardizeApart _standardizeApart;
        private CNFConverter cnfConverter;
        //
        // Persistent data structures
        //
        // Keeps track of the Sentences in their original form as added to the
        // Knowledge base.
        private IList<Sentence> originalSentences = new List<Sentence>();
        // The KB in clause form
        private ISet<Clause> clauses = new HashSet<Clause>();
        // Keep track of all of the definite clauses in the database
        // along with those that represent implications.
        private IList<Clause> allDefiniteClauses = new List<Clause>();
        private IList<Clause> implicationDefiniteClauses = new List<Clause>();
        // All the facts in the KB indexed by Atomic Sentence name (Note: pg. 279)
        private IDictionary<string, IList<Literal>> indexFacts = new Dictionary<string, IList<Literal>>();
        // Keep track of indexical keys for uniquely standardizing apart sentences
        private StandardizeApartIndexical variableIndexical = StandardizeApartIndexicalFactory.newStandardizeApartIndexical('v');
        private StandardizeApartIndexical queryIndexical = StandardizeApartIndexicalFactory.newStandardizeApartIndexical('q');

        //
        // PUBLIC METHODS
        //
        public FOLKnowledgeBase(FOLDomain domain)
            : this(domain, new FOLOTTERLikeTheoremProver())    // Default to Full Resolution if not set.
        { }

        public FOLKnowledgeBase(FOLDomain domain, InferenceProcedure inferenceProcedure)
            : this(domain, inferenceProcedure, new Unifier())
        { }

        public FOLKnowledgeBase(FOLDomain domain, InferenceProcedure inferenceProcedure, Unifier unifier)
        {
            this.parser = new FOLParser(new FOLDomain(domain));
            this.inferenceProcedure = inferenceProcedure;
            this.unifier = unifier;
            //
            this.substVisitor = new SubstVisitor();
            this.variableCollector = new VariableCollector();
            this._standardizeApart = new StandardizeApart(variableCollector, substVisitor);
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

        public void tell(IList<Sentence> sentences)
        {
            foreach (Sentence s in sentences)
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
            StandardizeApartResult saResult = _standardizeApart.standardizeApart(query, queryIndexical);

            // Need to map the result variables (as they are standardized apart)
            // to the original queries variables so that the caller can easily
            // understand and use the returned set of substitutions
            InferenceResult infResult = getInferenceProcedure().ask(this, saResult.getStandardized());
            foreach (Proof p in infResult.getProofs())
            {
                IDictionary<Variable, Term> im = p.getAnswerBindings();
                IDictionary<Variable, Term> em = new Dictionary<Variable, Term>();
                foreach (Variable rev in saResult.getReverseSubstitution().Keys)
                {
                    em.Add((Variable)saResult.getReverseSubstitution()[rev], im[rev]);
                }
                p.replaceAnswerBindings(em);
            }

            return infResult;
        }

        public int getNumberFacts()
        {
            return allDefiniteClauses.Count - implicationDefiniteClauses.Count;
        }

        public int getNumberRules()
        {
            return clauses.Count - getNumberFacts();
        }

        public IList<Sentence> getOriginalSentences()
        {
            return new ReadOnlyCollection<Sentence>(originalSentences);
        }

        public IList<Clause> getAllDefiniteClauses()
        {
            return new ReadOnlyCollection<Clause>(allDefiniteClauses);
        }

        public IList<Clause> getAllDefiniteClauseImplications()
        {
            return new ReadOnlyCollection<Clause>(implicationDefiniteClauses);
        }

        public ISet<Clause> getAllClauses()
        {
            return new HashSet<Clause>(clauses);
        }

        // Note: pg 278, FETCH(q) concept.
        public ISet<IDictionary<Variable, Term>> fetch(Literal l)
        {
            // Get all of the substitutions in the KB that p unifies with
            ISet<IDictionary<Variable, Term>> allUnifiers = new HashSet<IDictionary<Variable, Term>>();

            IList<Literal> matchingFacts = fetchMatchingFacts(l);
            if (null != matchingFacts)
            {
                foreach (Literal fact in matchingFacts)
                {
                    IDictionary<Variable, Term> substitution = unifier.unify(l.getAtomicSentence(), fact.getAtomicSentence());
                    if (null != substitution)
                    {
                        allUnifiers.Add(substitution);
                    }
                }
            }

            return allUnifiers;
        }

        // Note: To support FOL-FC-Ask
        public ISet<IDictionary<Variable, Term>> fetch(IList<Literal> literals)
        {
            ISet<IDictionary<Variable, Term>> possibleSubstitutions = new HashSet<IDictionary<Variable, Term>>();

            if (literals.Count > 0)
            {
                Literal first = literals[0];
                IList<Literal> rest = literals.Skip(1).ToList();

                recursiveFetch(new Dictionary<Variable, Term>(), first, rest, possibleSubstitutions);
            }

            return possibleSubstitutions;
        }

        public IDictionary<Variable, Term> unify(FOLNode x, FOLNode y)
        {
            return unifier.unify(x, y);
        }

        public Sentence subst(IDictionary<Variable, Term> theta, Sentence aSentence)
        {
            return substVisitor.subst(theta, aSentence);
        }

        public Literal subst(IDictionary<Variable, Term> theta, Literal l)
        {
            return substVisitor.subst(theta, l);
        }

        public Term subst(IDictionary<Variable, Term> theta, Term term)
        {
            return substVisitor.subst(theta, term);
        }

        // Note: see page 277.
        public Sentence standardizeApart(Sentence sentence)
        {
            return _standardizeApart.standardizeApart(sentence, variableIndexical)
                    .getStandardized();
        }

        public Clause standardizeApart(Clause clause)
        {
            return _standardizeApart.standardizeApart(clause, variableIndexical);
        }

        public Chain standardizeApart(Chain chain)
        {
            return _standardizeApart.standardizeApart(chain, variableIndexical);
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

            return new HashSet<Clause>(cnf.getConjunctionOfClauses());
        }

        public Literal createAnswerLiteral(Sentence forQuery)
        {
            string alName = parser.getFOLDomain().addAnswerLiteral();
            List<Term> terms = new List<Term>();

            ISet<Variable> vars = variableCollector.collectAllVariables(forQuery);
            foreach (Variable v in vars)
            {
                // Ensure copies of the variables are used.
                terms.Add(v.copy());
            }

            return new Literal(new Predicate(alName, terms));
        }

        // Note: see pg. 281
        public bool isRenaming(Literal l)
        {
            IList<Literal> possibleMatches = fetchMatchingFacts(l);
            if (null != possibleMatches)
            {
                return isRenaming(l, possibleMatches);
            }

            return false;
        }

        // Note: see pg. 281
        public bool isRenaming(Literal l, IList<Literal> possibleMatches)
        {
            foreach (Literal q in possibleMatches)
            {
                if (l.isPositiveLiteral() != q.isPositiveLiteral())
                {
                    continue;
                }
                IDictionary<Variable, Term> subst = unifier.unify(l.getAtomicSentence(),
                        q.getAtomicSentence());
                if (null != subst)
                {
                    int cntVarTerms = 0;
                    foreach (Term t in subst.Values)
                    {
                        if (t is Variable)
                        {
                            cntVarTerms++;
                        }
                    }
                    // If all the substitutions, even if none, map to Variables
                    // then this is a renaming
                    if (subst.Count == cntVarTerms)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Sentence s in originalSentences)
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
        private void store(Sentence sentence)
        {
            originalSentences.Add(sentence);

            // Convert the sentence to CNF
            CNF cnfOfOrig = cnfConverter.convertToCNF(sentence);
            foreach (Clause c in cnfOfOrig.getConjunctionOfClauses())
            {
                c.setProofStep(new ProofStepClauseClausifySentence(c, sentence));
                if (c.isEmpty())
                {
                    // This should not happen, if so the user
                    // is trying to add an unsatisfiable sentence
                    // to the KB.
                    throw new ArgumentException("Attempted to add unsatisfiable sentence to KB, orig=[" + sentence + "] CNF=" + cnfOfOrig);
                }

                // Ensure all clauses added to the KB are Standardized Apart.
                /*c = */_standardizeApart.standardizeApart(c, variableIndexical);

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
                        indexFact(c.getLiterals().First());
                    }
                }
            }
        }

        // Only if it is a unit clause does it get indexed as a fact
        // see pg. 279 for general idea.
        private void indexFact(Literal fact)
        {
            string factKey = getFactKey(fact);
            if (!indexFacts.ContainsKey(factKey))
            {
                indexFacts.Add(factKey, new List<Literal>());
            }

            indexFacts[factKey].Add(fact);
        }

        private void recursiveFetch(IDictionary<Variable, Term> theta, Literal l,
                IList<Literal> remainingLiterals,
                ISet<IDictionary<Variable, Term>> possibleSubstitutions)
        {

            // Find all substitutions for current predicate based on the
            // substitutions of prior predicates in the list (i.e. SUBST with
            // theta).
            ISet<IDictionary<Variable, Term>> pSubsts = fetch(subst(theta, l));

            // No substitutions, therefore cannot continue
            if (null == pSubsts)
            {
                return;
            }

            foreach (IDictionary<Variable, Term> psubst in pSubsts)
            {
                // Ensure all prior substitution information is maintained
                // along the chain of predicates (i.e. for shared variables
                // across the predicates).
                foreach (var v in theta)
                    psubst.Add(v);
                if (remainingLiterals.Count == 0)
                {
                    // This means I am at the end of the chain of predicates
                    // and have found a valid substitution.
                    possibleSubstitutions.Add(psubst);
                }
                else
                {
                    // Need to move to the next link in the chain of substitutions
                    Literal first = remainingLiterals[0];
                    IList<Literal> rest = remainingLiterals.Skip(1).ToList();

                    recursiveFetch(psubst, first, rest, possibleSubstitutions);
                }
            }
        }

        private IList<Literal> fetchMatchingFacts(Literal l)
        {
            if (!indexFacts.ContainsKey(getFactKey(l)))
                return null;
            else 
            return indexFacts[getFactKey(l)];
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
