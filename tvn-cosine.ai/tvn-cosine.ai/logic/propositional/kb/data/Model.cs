using System;
using System.Collections.Generic;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn.cosine.ai.logic.propositional.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pages 240, 245. 
     *  
     * Models are mathematical abstractions, each of which simply fixes the truth or
     * falsehood of every relevant sentence. In propositional logic, a model simply
     * fixes the <b>truth value</b> - <em>true</em> or <em>false</em> - for
     * every proposition symbol. 
     *  
     * Models as implemented here can represent partial assignments 
     * to the set of proposition symbols in a Knowledge Base (i.e. a partial model).
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class Model : PLVisitor<bool?, bool?>
    {
        private IDictionary<PropositionSymbol, bool?> assignments = new Dictionary<PropositionSymbol, bool?>();

        /**
         * Default Constructor.
         */
        public Model()
        {
        }

        public Model(IDictionary<PropositionSymbol, bool?> values)
        {
            foreach (var v in values)
                assignments.Add(v);
        }

        public bool? getValue(PropositionSymbol symbol)
        {
            return assignments[symbol];
        }

        public bool isTrue(PropositionSymbol symbol)
        {
            return Equals(assignments[symbol]);
        }

        public bool isFalse(PropositionSymbol symbol)
        {
            return !Equals(assignments[symbol]);
        }

        public Model union(PropositionSymbol symbol, bool? b)
        {
            Model m = new Model();
            foreach (var v in assignments)
                m.assignments.Add(v);
            m.assignments.Add(symbol, b);
            return m;
        }

        public Model unionInPlace(PropositionSymbol symbol, bool? b)
        {
            assignments.Add(symbol, b);
            return this;
        }

        public bool remove(PropositionSymbol p)
        {
            return assignments.Remove(p);
        }

        public bool isTrue(Sentence s)
        {
            return Equals(s.accept(this, null));
        }

        public bool isFalse(Sentence s)
        {
            return !Equals(s.accept(this, null));
        }

        public bool isUnknown(Sentence s)
        {
            return null == s.accept(this, null);
        }

        public Model flip(PropositionSymbol s)
        {
            if (isTrue(s))
            {
                return union(s, false);
            }
            if (isFalse(s))
            {
                return union(s, true);
            }
            return this;
        }

        public ISet<PropositionSymbol> getAssignedSymbols()
        {
            return new HashSet<PropositionSymbol>(assignments.Keys);
        }

        /**
         * Determine if the model satisfies a set of clauses.
         * 
         * @param clauses
         *            a set of propositional clauses.
         * @return if the model satisfies the clauses, false otherwise.
         */
        public bool satisfies(ISet<Clause> clauses)
        {
            foreach (Clause c in clauses)
            {
                // All must to be true
                if (!Equals(determineValue(c)))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Determine based on the current assignments within the model, whether a
         * clause is known to be true, false, or unknown.
         * 
         * @param c
         *            a propositional clause.
         * @return true, if the clause is known to be true under the model's
         *         assignments. false, if the clause is known to be false under the
         *         model's assignments. null, if it is unknown whether the clause is
         *         true or false under the model's current assignments.
         */
        public bool? determineValue(Clause c)
        {
            bool? result = null; // i.e. unknown

            if (c.isTautology())
            { // Test independent of the model's assignments.
                result = true;
            }
            else if (c.isFalse())
            { // Test independent of the model's
              // assignments.
                result = false;
            }
            else
            {
                bool unassignedSymbols = false;
                bool? value = null;
                foreach (PropositionSymbol positive in c.getPositiveSymbols())
                {
                    if (assignments.ContainsKey(positive))
                    {
                        if (Equals(assignments[positive]))
                        {
                            result = true;
                            break;
                        }
                    }
                    else
                    {
                        unassignedSymbols = true;
                    }
                }
                // If truth not determined, continue checking negative symbols
                if (result == null)
                {
                    foreach (PropositionSymbol negative in c.getNegativeSymbols())
                    {
                        value = assignments[negative];
                        if (value != null)
                        {
                            if (!Equals(value))
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            unassignedSymbols = true;
                        }
                    }

                    if (result == null)
                    {
                        // If truth not determined and there are no
                        // unassigned symbols then we can determine falsehood
                        // (i.e. all of its literals are assigned false under the
                        // model)
                        if (!unassignedSymbols)
                        {
                            result = false;
                        }
                    }
                }
            }

            return result;
        }

        public void print()
        {
            foreach (var e in assignments)
            {
                Console.WriteLine(e.Key + " = " + e.Value + " ");
            }
            Console.WriteLine();
        }

        public override string ToString()
        {
            return assignments.ToString();
        }

        //
        // START-PLVisitor 
        public bool? visitPropositionSymbol(PropositionSymbol s, bool? arg)
        {
            if (s.isAlwaysTrue())
            {
                return true;
            }
            if (s.isAlwaysFalse())
            {
                return false;
            }
            return getValue(s).Value;
        }

        public bool? visitUnarySentence(ComplexSentence fs, bool? arg)
        {
            object negatedValue = fs.getSimplerSentence(0).accept(this, null);
            if (negatedValue != null)
            {
                return !((bool)negatedValue);
            }
            else
            {
                return null;
            }
        }

        public bool? visitBinarySentence(ComplexSentence bs, bool? arg)
        {
            bool? firstValue = (bool)bs.getSimplerSentence(0).accept(this, null);
            bool? secondValue = (bool)bs.getSimplerSentence(1).accept(this, null);
            if ((firstValue == null) || (secondValue == null))
            {
                // strictly not true for or/and
                // -FIX later
                return null;
            }
            else
            {
                Connective connective = bs.getConnective();
                if (connective.Equals(Connective.AND))
                {
                    return firstValue.Value && secondValue.Value;
                }
                else if (connective.Equals(Connective.OR))
                {
                    return firstValue.Value || secondValue.Value;
                }
                else if (connective.Equals(Connective.IMPLICATION))
                {
                    return !(firstValue.Value && !secondValue.Value);
                }
                else if (connective.Equals(Connective.BICONDITIONAL))
                {
                    return firstValue.Equals(secondValue);
                }
                return null;
            }
        }
        // END-PLVisitor
        //
    }
}
