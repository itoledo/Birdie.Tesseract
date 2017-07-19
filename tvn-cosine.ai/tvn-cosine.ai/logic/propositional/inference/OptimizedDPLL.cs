﻿namespace tvn.cosine.ai.logic.propositional.inference
{
    public class OptimizedDPLL : DPLL
    {

        //
        // START-DPLL
         

    public bool dpllSatisfiable(Sentence s)
    {
        // clauses <- the set of clauses in the CNF representation of s
        ISet<Clause> clauses = ConvertToConjunctionOfClauses.convert(s)
                .getClauses();
        // symbols <- a list of the proposition symbols in s
        IQueue<PropositionSymbol> symbols = getPropositionSymbolsInSentence(s);

        // return DPLL(clauses, symbols, {})
        return dpll(clauses, symbols, new Model());
    }

    /**
	 * DPLL(clauses, symbols, model)<br>
	 * 
	 * @param clauses
	 *            the set of clauses.
	 * @param symbols
	 *            a list of unassigned symbols.
	 * @param model
	 *            contains the values for assigned symbols.
	 * @return true if the model is satisfiable under current assignments, false
	 *         otherwise.
	 */
     
    public bool dpll(Set<Clause> clauses, IQueue<PropositionSymbol> symbols, Model model)
    {
        // if every clause in clauses is true in model then return true
        // if some clause in clauses is false in model then return false
        // NOTE: for optimization reasons we only want to determine the
        // values of clauses once on each call to dpll
        bool allTrue = true;
        ISet<Clause> unknownClauses = Factory.CreateSet<>();
        for (Clause c : clauses)
        {
            Boolean value = model.determineValue(c);
            if (!true.Equals(value))
            {
                allTrue = false;
                if (false.Equals(value))
                {
                    return false;
                }
                unknownClauses.Add(c);
            }
        }
        if (allTrue)
        {
            return true;
        }
        else if (Tasks.currIsCancelled())
            return false;

        // NOTE: Performance Optimization -
        // Going forward, algorithm can ignore clauses that are already 
        // known to be true (reduces overhead on recursive calls and simplifies
        // findPureSymbols() and findUnitClauses() logic as they can
        // always assume unknown).
        clauses = unknownClauses;

        // P, value <- FIND-PURE-SYMBOL(symbols, clauses, model)
        Pair<PropositionSymbol, bool?> pAndValue = findPureSymbol(symbols,
                clauses, model);
        // if P is non-null then
        if (pAndValue != null)
        {
            // return DPLL(clauses, symbols - P, model U {P = value})
            return callDPLL(clauses, minus(symbols, pAndValue.getFirst()), model,
                    pAndValue.getFirst(), pAndValue.getSecond());
        }

        // P, value <- FIND-UNIT-CLAUSE(clauses, model)
        pAndValue = findUnitClause(clauses, model);
        // if P is non-null then
        if (pAndValue != null)
        {
            // return DPLL(clauses, symbols - P, model U {P = value})
            return callDPLL(clauses, minus(symbols, pAndValue.getFirst()), model,
                    pAndValue.getFirst(), pAndValue.getSecond());
        }

        // P <- FIRST(symbols); rest <- REST(symbols)
        PropositionSymbol p = Util.first(symbols);
        IQueue<PropositionSymbol> rest = Util.rest(symbols);
        // return DPLL(clauses, rest, model U {P = true}) or
        // ...... DPLL(clauses, rest, model U {P = false})
        return callDPLL(clauses, rest, model, p, true)
                || callDPLL(clauses, rest, model, p, false);
    }

    /**
	 * Determine if KB |= &alpha;, i.e. alpha is entailed by KB.
	 * 
	 * @param kb
	 *            a Knowledge Base in propositional logic.
	 * @param alpha
	 *            a propositional sentence.
	 * @return true, if &alpha; is entailed by KB, false otherwise.
	 */
     
    public bool isEntailed(KnowledgeBase kb, Sentence alpha)
    {
        // AIMA3e p.g. 260: kb |= alpha, can be done by testing
        // unsatisfiability of kb & ~alpha.
        ISet<Clause> kbAndNotAlpha = Factory.CreateSet<>();
        Sentence notQuery = new ComplexSentence(Connective.NOT, alpha);
        ISet<PropositionSymbol> symbols = Factory.CreateSet<>();
        IQueue<PropositionSymbol> querySymbols = Factory.CreateQueue<>(SymbolCollector.getSymbolsFrom(notQuery));

        kbAndNotAlpha.AddAll(kb.asCNF());
        kbAndNotAlpha.AddAll(ConvertToConjunctionOfClauses.convert(notQuery).getClauses());
        symbols.AddAll(querySymbols);
        symbols.AddAll(kb.getSymbols());

        return !dpll(kbAndNotAlpha, Factory.CreateQueue<>(symbols), new Model());
    }
    // END-DPLL
    //

    //
    // PROTECTED
    //

    // Note: Override this method if you wish to change the initial variable
    // ordering when dpllSatisfiable is called.
    protected IQueue<PropositionSymbol> getPropositionSymbolsInSentence(Sentence s)
    {
        return Factory.CreateQueue<>(SymbolCollector.getSymbolsFrom(s));
    }

    protected bool callDPLL(Set<Clause> clauses, IQueue<PropositionSymbol> symbols,
            Model model, PropositionSymbol p, bool value)
    {
        // We update the model in place with the assignment p=value,
        bool result = dpll(clauses, symbols, model.unionInPlace(p, value));
        // as backtracking can occur during the recursive calls we
        // need to remove the assigned value before we pop back out from this
        // call.
        model.Remove(p);
        return result;
    }

    /**
	 * AIMA3e p.g. 260:<br>
	 * <quote><i>Pure symbol heuristic:</i> A <b>pure symbol</b> is a symbol
	 * that always appears with the same "sign" in all clauses. For example, in
	 * the three clauses (A | ~B), (~B | ~C), and (C | A), the symbol A is pure
	 * because only the positive literal appears, B is pure because only the
	 * negative literal appears, and C is impure. It is easy to see that if a
	 * sentence has a model, then it has a model with the pure symbols assigned
	 * so as to make their literals true, because doing so can never make a
	 * clause false. Note that, in determining the purity of a symbol, the
	 * algorithm can ignore clauses that are already known to be true in the
	 * model constructed so far. For example, if the model contains B=false,
	 * then the clause (~B | ~C) is already true, and in the remaining clauses C
	 * appears only as a positive literal; therefore C becomes pure.</quote>
	 * 
	 * @param symbols
	 *            a list of currently unassigned symbols in the model (to be
	 *            checked if pure or not).
	 * @param clauses
	 * @param model
	 * @return a proposition symbol and value pair identifying a pure symbol and
	 *         a value to be assigned to it, otherwise null if no pure symbol
	 *         can be identified.
	 */
    protected Pair<PropositionSymbol, bool?> findPureSymbol(
            IQueue<PropositionSymbol> symbols, ISet<Clause> clauses, Model model)
    {
        Pair<PropositionSymbol, bool?> result = null;

        ISet<PropositionSymbol> symbolsToKeep = Factory.CreateSet<>(symbols);
        // Collect up possible positive and negative candidate sets of pure
        // symbols
        ISet<PropositionSymbol> candidatePurePositiveSymbols = Factory.CreateSet<>();
        ISet<PropositionSymbol> candidatePureNegativeSymbols = Factory.CreateSet<>();
        for (Clause c : clauses)
        {
            // Algorithm can ignore clauses that are already known to be true
            // NOTE: no longer need to do this here as we remove, true clauses
            // up front in the dpll call (as an optimization)

            // Collect possible candidates, removing all candidates that are
            // not part of the input list of symbols to be considered.
            for (PropositionSymbol p : c.getPositiveSymbols())
            {
                if (symbolsToKeep.contains(p))
                {
                    candidatePurePositiveSymbols.Add(p);
                }
            }
            for (PropositionSymbol n : c.getNegativeSymbols())
            {
                if (symbolsToKeep.contains(n))
                {
                    candidatePureNegativeSymbols.Add(n);
                }
            }
        }

        // Determine the overlap/intersection between the positive and negative
        // candidates
        for (PropositionSymbol s : symbolsToKeep)
        {
            // Remove the non-pure symbols
            if (candidatePurePositiveSymbols.contains(s) && candidatePureNegativeSymbols.contains(s))
            {
                candidatePurePositiveSymbols.Remove(s);
                candidatePureNegativeSymbols.Remove(s);
            }
        }

        // We have an implicit preference for positive pure symbols
        if (candidatePurePositiveSymbols.size() > 0)
        {
            result = new Pair<>(candidatePurePositiveSymbols.iterator().next(), true);
        } // We have a negative pure symbol
        else if (candidatePureNegativeSymbols.size() > 0)
        {
            result = new Pair<PropositionSymbol, bool?>(
                    candidatePureNegativeSymbols.iterator().next(), false);
        }

        return result;
    }

    /**
	 * AIMA3e p.g. 260:<br>
	 * <quote><i>Unit clause heuristic:</i> A <b>unit clause</b> was defined
	 * earlier as a clause with just one literal. In the context of DPLL, it
	 * also means clauses in which all literals but one are already assigned
	 * false by the model. For example, if the model contains B = true, then (~B
	 * | ~C) simplifies to ~C, which is a unit clause. Obviously, for this
	 * clause to be true, C must be set to false. The unit clause heuristic
	 * assigns all such symbols before branching on the remainder. One important
	 * consequence of the heuristic is that any attempt to prove (by refutation)
	 * a literal that is already in the knowledge base will succeed immediately.
	 * Notice also that assigning one unit clause can create another unit clause
	 * - for example, when C is set to false, (C | A) becomes a unit clause,
	 * causing true to be assigned to A. This "cascade" of forced assignments is
	 * called <b>unit propagation</b>. It resembles the process of forward
	 * chaining with definite clauses, and indeed, if the CNF expression
	 * contains only definite clauses then DPLL essentially replicates forward
	 * chaining.</quote>
	 * 
	 * @param clauses
	 * @param model
	 * @return a proposition symbol and value pair identifying a unit clause and
	 *         a value to be assigned to it, otherwise null if no unit clause
	 *         can be identified.
	 */
    protected Pair<PropositionSymbol, bool?> findUnitClause(
            ISet<Clause> clauses, Model model)
    {
        Pair<PropositionSymbol, bool?> result = null;

        for (Clause c : clauses)
        {
            // if clauses value is currently unknown
            // (i.e. means known literals are false)
            // NOTE: no longer need to perform this check
            // as only clauses with unknown values will
            // be passed to this routine from dpll as it
            // removes known ones up front.
            Literal unassigned = null;
            // Default definition of a unit clause is a clause
            // with just one literal
            if (c.isUnitClause())
            {
                unassigned = c.getLiterals().iterator().next();
            }
            else
            {
                // Also, a unit clause in the context of DPLL, also means a
                // clauseF in which all literals but one are already
                // assigned false by the model.
                // Note: at this point we already know the clause is not
                // true, so just need to determine if the clause has a
                // single unassigned literal
                for (Literal l : c.getLiterals())
                {
                    Boolean value = model.getValue(l.getAtomicSentence());
                    if (value == null)
                    {
                        // The first unassigned literal encountered.
                        if (unassigned == null)
                        {
                            unassigned = l;
                        }
                        else
                        {
                            // This means we have more than 1 unassigned
                            // literal so lets skip
                            unassigned = null;
                            break;
                        }
                    }
                }
            }

            // if a value assigned it means we have a single
            // unassigned literal and all the assigned literals
            // are not true under the current model as we were
            // unable to determine a value.
            if (unassigned != null)
            {
                result = new Pair<>(
                        unassigned.getAtomicSentence(),
                        unassigned.isPositiveLiteral());
                break;
            }
        }

        return result;
    }

    // symbols - P
    protected IQueue<PropositionSymbol> minus(IQueue<PropositionSymbol> symbols,
            PropositionSymbol p)
    {
        IQueue<PropositionSymbol> result = Factory.CreateQueue<>(
                symbols.size());
        for (PropositionSymbol s : symbols)
        {
            // symbols - P
            if (!p.Equals(s))
                result.Add(s);
        }
        return result;
    }
}
}
