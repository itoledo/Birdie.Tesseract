﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.fol.inference.proof;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.logic.fol.kb.data
{
    /**
     * A Clause: A disjunction of literals.
     * 
     * 
     * @author Ciaran O'Reilly
     * @author Tobias Barth
     * 
     */
    public class Clause
    {
        //
        private static StandardizeApartIndexical _saIndexical = StandardizeApartIndexicalFactory.newStandardizeApartIndexical('c');
        private static Unifier _unifier = new Unifier();
        private static SubstVisitor _substVisitor = new SubstVisitor();
        private static VariableCollector _variableCollector = new VariableCollector();
        private static StandardizeApart _standardizeApart = new StandardizeApart();
        private static LiteralsSorter _literalSorter = new LiteralsSorter();
        //
        private readonly ISet<Literal> literals = new HashSet<Literal>();
        private readonly List<Literal> positiveLiterals = new List<Literal>();
        private readonly List<Literal> negativeLiterals = new List<Literal>();
        private bool immutable = false;
        private bool saCheckRequired = true;
        private string equalityIdentity = "";
        private ISet<Clause> factors = null;
        private ISet<Clause> nonTrivialFactors = null;
        private string stringRep = null;
        private ProofStep proofStep = null;

        public Clause()
        {
            // i.e. the empty clause
        }

        public Clause(IList<Literal> lits)
        {
            foreach (var v in lits)
                this.literals.Add(v);
            foreach (Literal l in literals)
            {
                if (l.isPositiveLiteral())
                {
                    this.positiveLiterals.Add(l);
                }
                else
                {
                    this.negativeLiterals.Add(l);
                }
            }
            recalculateIdentity();
        }

        public Clause(IList<Literal> lits1, IList<Literal> lits2)
        {
            foreach (var v in lits1)
                literals.Add(v);

            foreach (var v in lits2)
                literals.Add(v);
            foreach (Literal l in literals)
            {
                if (l.isPositiveLiteral())
                {
                    this.positiveLiterals.Add(l);
                }
                else
                {
                    this.negativeLiterals.Add(l);
                }
            }
            recalculateIdentity();
        }

        public ProofStep getProofStep()
        {
            if (null == proofStep)
            {
                // Assume was a premise
                proofStep = new ProofStepPremise(this);
            }
            return proofStep;
        }

        public void setProofStep(ProofStep proofStep)
        {
            this.proofStep = proofStep;
        }

        public bool isImmutable()
        {
            return immutable;
        }

        public void setImmutable()
        {
            immutable = true;
        }

        public bool isStandardizedApartCheckRequired()
        {
            return saCheckRequired;
        }

        public void setStandardizedApartCheckNotRequired()
        {
            saCheckRequired = false;
        }

        public bool isEmpty()
        {
            return literals.Count == 0;
        }

        public bool isUnitClause()
        {
            return literals.Count == 1;
        }

        public bool isDefiniteClause()
        {
            // A Definite Clause is a disjunction of literals of which exactly 1 is
            // positive.
            return !isEmpty() && positiveLiterals.Count == 1;
        }

        public bool isImplicationDefiniteClause()
        {
            // An Implication Definite Clause is a disjunction of literals of
            // which exactly 1 is positive and there is 1 or more negative
            // literals.
            return isDefiniteClause() && negativeLiterals.Count >= 1;
        }

        public bool isHornClause()
        {
            // A Horn clause is a disjunction of literals of which at most one is
            // positive.
            return !isEmpty() && positiveLiterals.Count <= 1;
        }

        public bool isTautology()
        {
            foreach (Literal pl in positiveLiterals)
            {
                // Literals in a clause must be exact complements
                // for tautology elimination to apply. Do not
                // remove non-identical literals just because
                // they are complements under unification, see pg16:
                // http://logic.stanford.edu/classes/cs157/2008/notes/chap09.pdf
                foreach (Literal nl in negativeLiterals)
                {
                    if (pl.getAtomicSentence().Equals(nl.getAtomicSentence()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void addLiteral(Literal literal)
        {
            if (isImmutable())
            {
                throw new Exception("Clause is immutable, cannot be updated.");
            }
            int origSize = literals.Count;
            literals.Add(literal);
            if (literals.Count > origSize)
            {
                if (literal.isPositiveLiteral())
                {
                    positiveLiterals.Add(literal);
                }
                else
                {
                    negativeLiterals.Add(literal);
                }
            }
            recalculateIdentity();
        }

        public void addPositiveLiteral(AtomicSentence atom)
        {
            addLiteral(new Literal(atom));
        }

        public void addNegativeLiteral(AtomicSentence atom)
        {
            addLiteral(new Literal(atom, true));
        }

        public int getNumberLiterals()
        {
            return literals.Count;
        }

        public int getNumberPositiveLiterals()
        {
            return positiveLiterals.Count;
        }

        public int getNumberNegativeLiterals()
        {
            return negativeLiterals.Count;
        }

        public ISet<Literal> getLiterals()
        {
            return new HashSet<Literal>(literals);
        }

        public IList<Literal> getPositiveLiterals()
        {
            return new ReadOnlyCollection<Literal>(positiveLiterals);
        }

        public IList<Literal> getNegativeLiterals()
        {
            return new ReadOnlyCollection<Literal>(negativeLiterals);
        }

        public ISet<Clause> getFactors()
        {
            if (null == factors)
            {
                calculateFactors(null);
            }
            return new HashSet<Clause>(factors);
        }

        public ISet<Clause> getNonTrivialFactors()
        {
            if (null == nonTrivialFactors)
            {
                calculateFactors(null);
            }
            return new HashSet<Clause>(nonTrivialFactors);
        }

        public bool subsumes(Clause othC)
        {
            bool subsumes = false;

            // Equality is not subsumption
            if (!(this == othC))
            {
                // Ensure this has less literals total and that
                // it is a subset of the other clauses positive and negative counts
                if (this.getNumberLiterals() < othC.getNumberLiterals()
                        && this.getNumberPositiveLiterals() <= othC
                                .getNumberPositiveLiterals()
                        && this.getNumberNegativeLiterals() <= othC
                                .getNumberNegativeLiterals())
                {

                    IDictionary<string, IList<Literal>> thisToTry = collectLikeLiterals(this.literals);
                    IDictionary<string, IList<Literal>> othCToTry = collectLikeLiterals(othC.literals);
                    // Ensure all like literals from this clause are a subset of the other clause.
                    if (othCToTry.Keys.Intersect(thisToTry.Keys).Count() == othCToTry.Keys.Count)
                    {
                        bool isAPossSubset = true;
                        // Ensure that each set of same named literals
                        // from this clause is a subset of the other
                        // clauses same named literals.
                        foreach (string pk in thisToTry.Keys)
                        {
                            if (thisToTry[pk].Count > othCToTry[pk].Count)
                            {
                                isAPossSubset = false;
                                break;
                            }
                        }
                        if (isAPossSubset)
                        {
                            // At this point I know this this Clause's
                            // literal/arity names are a subset of the
                            // other clauses literal/arity names
                            subsumes = checkSubsumes(othC, thisToTry, othCToTry);
                        }
                    }
                }
            }

            return subsumes;
        }

        // Note: Applies binary resolution rule
        // Note: returns a set with an empty clause if both clauses
        // are empty, otherwise returns a set of binary resolvents.
        public ISet<Clause> binaryResolvents(Clause othC)
        {
            ISet<Clause> resolvents = new HashSet<Clause>();
            // Resolving two empty clauses
            // gives you an empty clause
            if (isEmpty() && othC.isEmpty())
            {
                resolvents.Add(new Clause());
                return resolvents;
            }

            // Ensure Standardized Apart
            // Before attempting binary resolution
            othC = saIfRequired(othC);

            List<Literal> allPosLits = new List<Literal>();
            List<Literal> allNegLits = new List<Literal>();
            allPosLits.AddRange(this.positiveLiterals);
            allPosLits.AddRange(othC.positiveLiterals);
            allNegLits.AddRange(this.negativeLiterals);
            allNegLits.AddRange(othC.negativeLiterals);

            List<Literal> trPosLits = new List<Literal>();
            List<Literal> trNegLits = new List<Literal>();
            List<Literal> copyRPosLits = new List<Literal>();
            List<Literal> copyRNegLits = new List<Literal>();

            for (int i = 0; i < 2; i++)
            {
                trPosLits.Clear();
                trNegLits.Clear();

                if (i == 0)
                {
                    // See if this clauses positives
                    // unify with the other clauses
                    // negatives
                    trPosLits.AddRange(this.positiveLiterals);
                    trNegLits.AddRange(othC.negativeLiterals);
                }
                else
                {
                    // Try the other way round now
                    trPosLits.AddRange(othC.positiveLiterals);
                    trNegLits.AddRange(this.negativeLiterals);
                }

                // Now check to see if they resolve
                IDictionary<Variable, Term> copyRBindings = new Dictionary<Variable, Term>();
                foreach (Literal pl in trPosLits)
                {
                    foreach (Literal nl in trNegLits)
                    {
                        copyRBindings.Clear();
                        if (null != _unifier.unify(pl.getAtomicSentence(),
                                nl.getAtomicSentence(), copyRBindings))
                        {
                            copyRPosLits.Clear();
                            copyRNegLits.Clear();
                            bool found = false;
                            foreach (Literal l in allPosLits)
                            {
                                if (!found && pl.Equals(l))
                                {
                                    found = true;
                                    continue;
                                }
                                copyRPosLits.Add(_substVisitor.subst(copyRBindings, l));
                            }
                            found = false;
                            foreach (Literal l in allNegLits)
                            {
                                if (!found && nl.Equals(l))
                                {
                                    found = true;
                                    continue;
                                }
                                copyRNegLits.Add(_substVisitor.subst(copyRBindings, l));
                            }
                            // Ensure the resolvents are standardized apart
                            IDictionary<Variable, Term> renameSubstitituon = _standardizeApart.standardizeApart(copyRPosLits, copyRNegLits, _saIndexical);
                            Clause c = new Clause(copyRPosLits, copyRNegLits);
                            c.setProofStep(new ProofStepClauseBinaryResolvent(c,
                                    pl, nl, this, othC, copyRBindings,
                                    renameSubstitituon));
                            if (isImmutable())
                            {
                                c.setImmutable();
                            }
                            if (!isStandardizedApartCheckRequired())
                            {
                                c.setStandardizedApartCheckNotRequired();
                            }
                            resolvents.Add(c);
                        }
                    }
                }
            }

            return resolvents;
        }

        public override string ToString()
        {
            if (null == stringRep)
            {
                List<Literal> sortedLiterals = new List<Literal>(literals);
                sortedLiterals.Sort(_literalSorter);

                stringRep = sortedLiterals.ToString();
            }
            return stringRep;
        }

        public override int GetHashCode()
        {
            return equalityIdentity.GetHashCode();
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
            if (!(othObj is Clause))
            {
                return false;
            }
            Clause othClause = (Clause)othObj;

            return equalityIdentity.Equals(othClause.equalityIdentity);
        }

        public string getEqualityIdentity()
        {
            return equalityIdentity;
        }

        //
        // PRIVATE METHODS
        //
        private void recalculateIdentity()
        {

            // Sort the literals first based on negation, atomic sentence,
            // constant, function and variable.
            List<Literal> sortedLiterals = new List<Literal>(literals);
            sortedLiterals.Sort(_literalSorter);

            // All variables are considered the same as regards
            // sorting. Therefore, to determine if two clauses
            // are equivalent you need to determine
            // the # of unique variables they contain and
            // there positions across the clauses
            ClauseEqualityIdentityConstructor ceic = new ClauseEqualityIdentityConstructor(sortedLiterals, _literalSorter);

            equalityIdentity = ceic.getIdentity();

            // Reset, these as will need to re-calcualte
            // if requested for again, best to only
            // access lazily.
            factors = null;
            nonTrivialFactors = null;
            // Reset the objects string representation
            // until it is requested for.
            stringRep = null;
        }

        private void calculateFactors(ISet<Clause> parentFactors)
        {
            nonTrivialFactors = new HashSet<Clause>();

            IDictionary<Variable, Term> theta = new Dictionary<Variable, Term>();
            List<Literal> lits = new List<Literal>();
            for (int i = 0; i < 2; i++)
            {
                lits.Clear();
                if (i == 0)
                {
                    // Look at the positive literals
                    lits.AddRange(positiveLiterals);
                }
                else
                {
                    // Look at the negative literals
                    lits.AddRange(negativeLiterals);
                }
                for (int x = 0; x < lits.Count; x++)
                {
                    for (int y = x + 1; y < lits.Count; y++)
                    {
                        Literal litX = lits[x];
                        Literal litY = lits[y];

                        theta.Clear();
                        IDictionary<Variable, Term> substitution = _unifier.unify(litX.getAtomicSentence(), litY.getAtomicSentence(), theta);
                        if (null != substitution)
                        {
                            List<Literal> posLits = new List<Literal>();
                            List<Literal> negLits = new List<Literal>();
                            if (i == 0)
                            {
                                posLits.Add(_substVisitor.subst(substitution, litX));
                            }
                            else
                            {
                                negLits.Add(_substVisitor.subst(substitution, litX));
                            }
                            foreach (Literal pl in positiveLiterals)
                            {
                                if (pl == litX || pl == litY)
                                {
                                    continue;
                                }
                                posLits.Add(_substVisitor.subst(substitution, pl));
                            }
                            foreach (Literal nl in negativeLiterals)
                            {
                                if (nl == litX || nl == litY)
                                {
                                    continue;
                                }
                                negLits.Add(_substVisitor.subst(substitution, nl));
                            }
                            // Ensure the non trivial factor is standardized apart
                            IDictionary<Variable, Term> renameSubst = _standardizeApart
                                    .standardizeApart(posLits, negLits,
                                            _saIndexical);
                            Clause c = new Clause(posLits, negLits);
                            c.setProofStep(new ProofStepClauseFactor(c, this, litX,
                                    litY, substitution, renameSubst));
                            if (isImmutable())
                            {
                                c.setImmutable();
                            }
                            if (!isStandardizedApartCheckRequired())
                            {
                                c.setStandardizedApartCheckNotRequired();
                            }
                            if (null == parentFactors)
                            {
                                c.calculateFactors(nonTrivialFactors);
                                foreach (var v in c.getFactors())
                                    nonTrivialFactors.Add(v);
                            }
                            else
                            {
                                if (!parentFactors.Contains(c))
                                {
                                    c.calculateFactors(nonTrivialFactors);
                                    foreach (var v in c.getFactors())
                                        nonTrivialFactors.Add(v);
                                }
                            }
                        }
                    }
                }
            }

            factors = new HashSet<Clause>();
            // Need to add self, even though a non-trivial
            // factor. See: slide 30
            // http://logic.stanford.edu/classes/cs157/2008/lectures/lecture10.pdf
            // for example of incompleteness when
            // trivial factor not included.
            factors.Add(this);
            foreach (var v in nonTrivialFactors)
                factors.Add(v);
        }

        private Clause saIfRequired(Clause othClause)
        {

            // If performing resolution with self
            // then need to standardize apart in
            // order to work correctly.
            if (isStandardizedApartCheckRequired() || this == othClause)
            {
                ISet<Variable> mVariables = _variableCollector
                        .collectAllVariables(this);
                ISet<Variable> oVariables = _variableCollector
                        .collectAllVariables(othClause);

                ISet<Variable> cVariables = new HashSet<Variable>();

                foreach (var v in mVariables)
                    cVariables.Add(v);

                foreach (var v in oVariables)
                    cVariables.Add(v);

                if (cVariables.Count < (mVariables.Count + oVariables.Count))
                {
                    othClause = _standardizeApart.standardizeApart(othClause,
                            _saIndexical);
                }
            }

            return othClause;
        }

        private IDictionary<string, IList<Literal>> collectLikeLiterals(ISet<Literal> literals)
        {
            IDictionary<string, IList<Literal>> likeLiterals = new Dictionary<string, IList<Literal>>();
            foreach (Literal l in literals)
            {
                // Want to ensure P(a, b) is considered different than P(a, b, c)
                // i.e. consider an atom's arity P/#.
                string literalName = (l.isNegativeLiteral() ? "~" : "")
                        + l.getAtomicSentence().getSymbolicName() + "/"
                        + l.getAtomicSentence().getArgs().Count;
                IList<Literal> like = likeLiterals[literalName];
                if (null == like)
                {
                    like = new List<Literal>();
                    likeLiterals.Add(literalName, like);
                }
                like.Add(l);
            }
            return likeLiterals;
        }

        private bool checkSubsumes(Clause othC, IDictionary<string, IList<Literal>> thisToTry, IDictionary<string, IList<Literal>> othCToTry)
        {
            bool subsumes = false;

            IList<Term> thisTerms = new List<Term>();
            IList<Term> othCTerms = new List<Term>();

            // Want to track possible number of permuations
            List<int> radices = new List<int>();
            foreach (string literalName in thisToTry.Keys)
            {
                int sizeT = thisToTry[literalName].Count;
                int sizeO = othCToTry[literalName].Count;

                if (sizeO > 1)
                {
                    // The following is being used to
                    // track the number of permutations
                    // that can be mapped from the
                    // other clauses like literals to this
                    // clauses like literals.
                    // i.e. n!/(n-r)!
                    // where n=sizeO and r =sizeT
                    for (int i = 0; i < sizeT; i++)
                    {
                        int r = sizeO - i;
                        if (r > 1)
                        {
                            radices.Add(r);
                        }
                    }
                }
                // Track the terms for this clause
                foreach (Literal tl in thisToTry[literalName])
                {
                    (thisTerms as List<Term>).AddRange(tl.getAtomicSentence().getArgs().Select(x => x as Term));
                }
            }

            MixedRadixNumber permutation = null;
            long numPermutations = 1L;
            if (radices.Count > 0)
            {
                permutation = new MixedRadixNumber(0, radices);
                numPermutations = permutation.getMaxAllowedValue() + 1;
            }
            // Want to ensure none of the othCVariables are
            // part of the key set of a unification as
            // this indicates it is not a legal subsumption.
            ISet<Variable> othCVariables = _variableCollector.collectAllVariables(othC);
            IDictionary<Variable, Term> theta = new Dictionary<Variable, Term>();
            List<Literal> literalPermuations = new List<Literal>();
            for (long l = 0L; l < numPermutations; l++)
            {
                // Track the other clause's terms for this
                // permutation.
                othCTerms.Clear();
                int radixIdx = 0;
                foreach (string literalName in thisToTry.Keys)
                {
                    int sizeT = thisToTry[literalName].Count;
                    literalPermuations.Clear();
                    literalPermuations.AddRange(othCToTry[literalName]);
                    int sizeO = literalPermuations.Count;

                    if (sizeO > 1)
                    {
                        for (int i = 0; i < sizeT; i++)
                        {
                            int r = sizeO - i;
                            if (r > 1)
                            {
                                // If not a 1 to 1 mapping then you need
                                // to use the correct permuation
                                int numPos = permutation
                                        .getCurrentNumeralValue(radixIdx);
                                var d = literalPermuations[numPos];
                                (othCTerms as List<Term>).AddRange(d.getAtomicSentence().getArgs().Select(q => q as Term));
                                radixIdx++;
                            }
                            else
                            {
                                // is the last mapping, therefore
                                // won't be on the radix
                                (othCTerms as List<Term>).AddRange(literalPermuations[0].getAtomicSentence().getArgs().Select(q => q as Term));
                            }
                        }
                    }
                    else
                    {
                        // a 1 to 1 mapping
                        (othCTerms as List<Term>).AddRange(literalPermuations[0].getAtomicSentence().getArgs().Select(q => q as Term));
                    }
                }

                // Note: on unifier
                // unifier.unify(P(w, x), P(y, z)))={w=y, x=z}
                // unifier.unify(P(y, z), P(w, x)))={y=w, z=x}
                // Therefore want this clause to be the first
                // so can do the othCVariables check for an invalid
                // subsumes.
                theta.Clear();
                if (null != _unifier.unify(thisTerms.Select(q => q as FOLNode).ToList(),
                    othCTerms.Select(q => q as FOLNode).ToList(), theta))
                {
                    bool containsAny = false;
                    foreach (Variable v in theta.Keys)
                    {
                        if (othCVariables.Contains(v))
                        {
                            containsAny = true;
                            break;
                        }
                    }
                    if (!containsAny)
                    {
                        subsumes = true;
                        break;
                    }
                }

                // If there is more than 1 mapping
                // keep track of where I am in the
                // possible number of mapping permutations.
                if (null != permutation)
                {
                    permutation.increment();
                }
            }

            return subsumes;
        }
    }

    class LiteralsSorter : IComparer<Literal>
    {

        public int Compare(Literal o1, Literal o2)
        {
            int rVal = 0;
            // If literals are not negated the same
            // then positive literals are considered
            // (by convention here) to be of higher
            // order than negative literals
            if (o1.isPositiveLiteral() != o2.isPositiveLiteral())
            {
                if (o1.isPositiveLiteral())
                {
                    return 1;
                }
                return -1;
            }

            // Check their symbolic names for order first
            rVal = o1.getAtomicSentence().getSymbolicName()
                    .CompareTo(o2.getAtomicSentence().getSymbolicName());

            // If have same symbolic names
            // then need to compare individual arguments
            // for order.
            if (0 == rVal)
            {
                rVal = compareArgs(o1.getAtomicSentence().getArgs().Select(q => q as Term).ToList(),
                    o2.getAtomicSentence().getArgs().Select(q => q as Term).ToList());
            }

            return rVal;
        }

        private int compareArgs(IList<Term> args1, IList<Term> args2)
        {
            int rVal = 0;

            // Compare argument sizes first
            rVal = args1.Count - args2.Count;

            if (0 == rVal && args1.Count > 0)
            {
                // Move forward and compare the
                // first arguments
                Term t1 = args1[0];
                Term t2 = args2[0];

                if (t1.GetType() == t2.GetType())
                {
                    // Note: Variables are considered to have
                    // the same order
                    if (t1 is Constant)
                    {
                        rVal = t1.getSymbolicName().CompareTo(t2.getSymbolicName());
                    }
                    else if (t1 is Function)
                    {
                        rVal = t1.getSymbolicName().CompareTo(t2.getSymbolicName());
                        if (0 == rVal)
                        {
                            // Same function names, therefore
                            // compare the function arguments
                            rVal = compareArgs(t1.getArgs().Select(q => q as Term).ToList(), t2.getArgs().Select(q => q as Term).ToList());
                        }
                    }

                    // If the first args are the same
                    // then compare the ordering of the
                    // remaining arguments
                    if (0 == rVal)
                    {
                        rVal = compareArgs(args1.Skip(1).Select(q => q as Term).ToList(),
                                args2.Skip(1).Select(q => q as Term).ToList());
                    }
                }
                else
                {
                    // Order for different Terms is:
                    // Constant > Function > Variable
                    if (t1 is Constant)
                    {
                        rVal = 1;
                    }
                    else if (t2 is Constant)
                    {
                        rVal = -1;
                    }
                    else if (t1 is Function)
                    {
                        rVal = 1;
                    }
                    else
                    {
                        rVal = -1;
                    }
                }
            }

            return rVal;
        }
    }

    class ClauseEqualityIdentityConstructor : FOLVisitor
    {
        private StringBuilder identity = new StringBuilder();
        private int noVarPositions = 0;
        private int[] clauseVarCounts = null;
        private int currentLiteral = 0;
        private IDictionary<string, List<int>> varPositions = new Dictionary<string, List<int>>();

        public ClauseEqualityIdentityConstructor(List<Literal> literals,
                LiteralsSorter sorter)
        {

            clauseVarCounts = new int[literals.Count];

            foreach (Literal l in literals)
            {
                if (l.isNegativeLiteral())
                {
                    identity.Append("~");
                }
                identity.Append(l.getAtomicSentence().getSymbolicName());
                identity.Append("(");
                bool firstTerm = true;
                foreach (Term t in l.getAtomicSentence().getArgs())
                {
                    if (firstTerm)
                    {
                        firstTerm = false;
                    }
                    else
                    {
                        identity.Append(",");
                    }
                    t.accept(this, null);
                }
                identity.Append(")");
                currentLiteral++;
            }

            int min, max;
            min = max = 0;
            for (int i = 0; i < literals.Count; i++)
            {
                int incITo = i;
                int next = i + 1;
                max += clauseVarCounts[i];
                while (next < literals.Count)
                {
                    if (0 != sorter.Compare(literals[i], literals[next]))
                    {
                        break;
                    }
                    max += clauseVarCounts[next];
                    incITo = next; // Need to skip to the end of the range
                    next++;
                }
                // This indicates two or more literals are identical
                // except for variable naming (note: identical
                // same name would be removed as are working
                // with sets so don't need to worry about this).
                if ((next - i) > 1)
                {
                    // Need to check each variable
                    // and if it has a position within the
                    // current min/max range then need
                    // to include its alternative
                    // sort order positions as well
                    foreach (string key in varPositions.Keys)
                    {
                        List<int> positions = varPositions[key];
                        List<int> additPositions = new List<int>();
                        // Add then subtract for all possible
                        // positions in range
                        foreach (int pos in positions)
                        {
                            if (pos >= min && pos < max)
                            {
                                int pPos = pos;
                                int nPos = pos;
                                for (int candSlot = i; candSlot < (next - 1); candSlot++)
                                {
                                    pPos += clauseVarCounts[i];
                                    if (pPos >= min && pPos < max)
                                    {
                                        if (!positions.Contains(pPos)
                                                && !additPositions.Contains(pPos))
                                        {
                                            additPositions.Add(pPos);
                                        }
                                    }
                                    nPos -= clauseVarCounts[i];
                                    if (nPos >= min && nPos < max)
                                    {
                                        if (!positions.Contains(nPos)
                                                && !additPositions.Contains(nPos))
                                        {
                                            additPositions.Add(nPos);
                                        }
                                    }
                                }
                            }
                        }
                        positions.AddRange(additPositions);
                    }
                }
                min = max;
                i = incITo;
            }

            // Determine the maxWidth
            int maxWidth = 1;
            while (noVarPositions >= 10)
            {
                noVarPositions = noVarPositions / 10;
                maxWidth++;
            }

            // Sort the individual position lists
            // And then add their string representations
            // together
            List<string> varOffsets = new List<string>();
            foreach (string key in varPositions.Keys)
            {
                List<int> positions = varPositions[key];
                positions.Sort();
                StringBuilder sb = new StringBuilder();
                foreach (int pos in positions)
                {
                    string posStr = pos.ToString();
                    int posStrLen = posStr.Length;
                    int padLen = maxWidth - posStrLen;
                    for (int i = 0; i < padLen; i++)
                    {
                        sb.Append('0');
                    }
                    sb.Append(posStr);
                }
                varOffsets.Add(sb.ToString());
            }
            varOffsets.Sort();
            for (int i = 0; i < varOffsets.Count; i++)
            {
                identity.Append(varOffsets[i]);
                if (i < (varOffsets.Count - 1))
                {
                    identity.Append(",");
                }
            }
        }

        public string getIdentity()
        {
            return identity.ToString();
        }

        //
        // START-FOLVisitor
        public object visitVariable(Variable var, object arg)
        {
            // All variables will be marked with an *
            identity.Append("*");

            List<int> positions = varPositions[var.getValue()];
            if (null == positions)
            {
                positions = new List<int>();
                varPositions.Add(var.getValue(), positions);
            }
            positions.Add(noVarPositions);

            noVarPositions++;
            clauseVarCounts[currentLiteral]++;
            return var;
        }

        public object visitConstant(Constant constant, object arg)
        {
            identity.Append(constant.getValue());
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            bool firstTerm = true;
            identity.Append(function.getFunctionName());
            identity.Append("(");
            foreach (Term t in function.getTerms())
            {
                if (firstTerm)
                {
                    firstTerm = false;
                }
                else
                {
                    identity.Append(",");
                }
                t.accept(this, arg);
            }
            identity.Append(")");

            return function;
        }

        public object visitPredicate(Predicate predicate, object arg)
        {
            throw new Exception("Should not be called");
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            throw new Exception("Should not be called");
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence,
                object arg)
        {
            throw new Exception("Should not be called");
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            throw new Exception("Should not be called");
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            throw new Exception("Should not be called");
        }

        // END-FOLVisitor
        //
    }
}