using System.Collections.Generic;
using System.Linq;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.kb.data;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.logic.propositional.visitors;

namespace tvn.cosine.ai.logic.propositional.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 255.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function PL-RESOLUTION(KB, &alpha;) returns true or false
     *    inputs: KB, the knowledge base, a sentence in propositional logic
     *            &alpha;, the query, a sentence in propositional logic
     *            
     *    clauses &larr; the set of clauses in the CNF representation of KB &and; &not;&alpha;
     *    new &larr; {}
     *    loop do
     *       for each pair of clauses C<sub>i</sub>, C<sub>j</sub> in clauses do
     *          resolvents &larr; PL-RESOLVE(C<sub>i</sub>, C<sub>j</sub>)
     *          if resolvents contains the empty clause then return true
     *          new &larr; new &cup; resolvents
     *       if new &sube; clauses then return false
     *       clauses &larr; clauses &cup; new
     * </code>
     * </pre>
     * 
     * Figure 7.12 A simple resolution algorithm for propositional logic. The
     * function PL-RESOLVE returns the set of all possible clauses obtained by
     * resolving its two inputs.<br>
     * <br>
     * Note: Optional optimization added to implementation whereby tautological
     * clauses can be removed during processing of the algorithm - see pg. 254 of
     * AIMA3e:<br>
     * <blockquote> Inspection of Figure 7.13 reveals that many resolution steps are
     * pointless. For example, the clause B<sub>1,1</sub> &or; &not;B<sub>1,1</sub>
     * &or; P<sub>1,2</sub> is equivalent to <i>True</i> &or; P<sub>1,2</sub> which
     * is equivalent to <i>True</i>. Deducing that <i>True</i> is true is not very
     * helpful. Therefore, any clauses in which two complementary literals appear
     * can be discarded. </blockquote>
     * 
     * @see Clause#isTautology()
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class PLResolution
    {
        /**
         * PL-RESOLUTION(KB, &alpha;)<br>
         * A simple resolution algorithm for propositional logic.
         * 
         * @param kb
         *            the knowledge base, a sentence in propositional logic.
         * @param alpha
         *            the query, a sentence in propositional logic.
         * @return true if KB |= &alpha;, false otherwise.
         */
        public bool plResolution(KnowledgeBase kb, Sentence alpha)
        {
            // clauses <- the set of clauses in the CNF representation
            // of KB & ~alpha
            ISet<Clause> clauses = setOfClausesInTheCNFRepresentationOfKBAndNotAlpha(kb, alpha);
            // new <- {}
            ISet<Clause> newClauses = new HashSet<Clause>();
            // loop do
            do
            {
                // for each pair of clauses C_i, C_j in clauses do
                List<Clause> clausesAsList = new List<Clause>(clauses);
                for (int i = 0; i < clausesAsList.Count - 1; i++)
                {
                    Clause ci = clausesAsList[i];
                    for (int j = i + 1; j < clausesAsList.Count; j++)
                    {
                        Clause cj = clausesAsList[j];
                        // resolvents <- PL-RESOLVE(C_i, C_j)
                        ISet<Clause> resolvents = plResolve(ci, cj);
                        // if resolvents contains the empty clause then return true
                        if (resolvents.Contains(Clause.EMPTY))
                        {
                            return true;
                        }
                        // new <- new U resolvents
                        foreach (var v in resolvents)
                            newClauses.Add(v);
                    }
                }
                // if new is subset of clauses then return false
                if (clauses.Intersect(newClauses).Count() == clauses.Count)
                {
                    return false;
                }

                // clauses <- clauses U new
                foreach (var v in newClauses)
                    clauses.Add(v);

            } while (true);
        }

        /**
         * PL-RESOLVE(C<sub>i</sub>, C<sub>j</sub>)<br>
         * Calculate the set of all possible clauses by resolving its two inputs.
         * 
         * @param ci
         *            clause 1
         * @param cj
         *            clause 2
         * @return the set of all possible clauses obtained by resolving its two
         *         inputs.
         */
        public ISet<Clause> plResolve(Clause ci, Clause cj)
        {
            ISet<Clause> resolvents = new HashSet<Clause>();

            // The complementary positive literals from C_i
            resolvePositiveWithNegative(ci, cj, resolvents);
            // The complementary negative literals from C_i
            resolvePositiveWithNegative(cj, ci, resolvents);

            return resolvents;
        }

        //
        // SUPPORTING CODE
        //

        private bool _discardTautologies = true;

        /**
         * Default constructor, which will set the algorithm to discard tautologies
         * by default.
         */
        public PLResolution()
            : this(true)
        {

        }

        /**
         * Constructor.
         * 
         * @param discardTautologies
         *            true if the algorithm is to discard tautological clauses
         *            during processing, false otherwise.
         */
        public PLResolution(bool discardTautologies)
        {
            setDiscardTautologies(discardTautologies);
        }

        /**
         * @return true if the algorithm will discard tautological clauses during
         *         processing.
         */
        public bool isDiscardTautologies()
        {
            return _discardTautologies;
        }

        /**
         * Determine whether or not the algorithm should discard tautological
         * clauses during processing.
         * 
         * @param discardTautologies
         */
        public void setDiscardTautologies(bool discardTautologies)
        {
            this._discardTautologies = discardTautologies;
        }

        //
        // PROTECTED
        //
        protected ISet<Clause> setOfClausesInTheCNFRepresentationOfKBAndNotAlpha(KnowledgeBase kb, Sentence alpha)
        {

            // KB & ~alpha;
            Sentence isContradiction = new ComplexSentence(Connective.AND,
                    kb.asSentence(), new ComplexSentence(Connective.NOT, alpha));
            // the set of clauses in the CNF representation
            ISet<Clause> clauses = new HashSet<Clause>(
                    ConvertToConjunctionOfClauses.convert(isContradiction)
                            .getClauses());

            discardTautologies(clauses);

            return clauses;
        }

        protected void resolvePositiveWithNegative(Clause c1, Clause c2, ISet<Clause> resolvents)
        {
            // Calculate the complementary positive literals from c1 with
            // the negative literals from c2
            ISet<PropositionSymbol> complementary =
                  new HashSet<PropositionSymbol>(c1.getPositiveSymbols().Intersect(c2.getNegativeSymbols()));
            // Construct a resolvent clause for each complement found
            foreach (PropositionSymbol complement in complementary)
            {
                List<Literal> resolventLiterals = new List<Literal>();
                // Retrieve the literals from c1 that are not the complement
                foreach (Literal c1l in c1.getLiterals())
                {
                    if (c1l.isNegativeLiteral()
                            || !c1l.getAtomicSentence().Equals(complement))
                    {
                        resolventLiterals.Add(c1l);
                    }
                }
                // Retrieve the literals from c2 that are not the complement
                foreach (Literal c2l in c2.getLiterals())
                {
                    if (c2l.isPositiveLiteral()
                            || !c2l.getAtomicSentence().Equals(complement))
                    {
                        resolventLiterals.Add(c2l);
                    }
                }
                // Construct the resolvent clause
                Clause resolvent = new Clause(resolventLiterals);
                // Discard tautological clauses if this optimization is turned on.
                if (!(isDiscardTautologies() && resolvent.isTautology()))
                {
                    resolvents.Add(resolvent);
                }
            }
        }

        // Utility routine for removing the tautological clauses from a set (in place).
        protected void discardTautologies(ISet<Clause> clauses)
        {
            if (isDiscardTautologies())
            {
                ISet<Clause> toDiscard = new HashSet<Clause>();
                foreach (Clause c in clauses)
                {
                    if (c.isTautology())
                    {
                        toDiscard.Add(c);
                    }
                }
                foreach (var v in toDiscard)
                    clauses.Remove(v);
            }
        }
    }
}
