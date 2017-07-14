﻿using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.inference.proof;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol
{
    /**
  * @author Ciaran O'Reilly
  * 
  */
    public class StandardizeApart
    {
        private VariableCollector variableCollector = null;
        private SubstVisitor substVisitor = null;

        public StandardizeApart()
        {
            variableCollector = new VariableCollector();
            substVisitor = new SubstVisitor();
        }

        public StandardizeApart(VariableCollector variableCollector, SubstVisitor substVisitor)
        {
            this.variableCollector = variableCollector;
            this.substVisitor = substVisitor;
        }

        // Note: see page 327.
        public StandardizeApartResult standardizeApart(Sentence sentence, StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = variableCollector.collectAllVariables(sentence);
            IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();
            IDictionary<Variable, Term> reverseSubstitution = new Dictionary<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution[var] = v;
                reverseSubstitution[v] = var;
            }

            Sentence standardized = substVisitor.subst(renameSubstitution, sentence);

            return new StandardizeApartResult(sentence, standardized,
                    renameSubstitution, reverseSubstitution);
        }

        public Clause standardizeApart(Clause clause, StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = variableCollector.collectAllVariables(clause);
            IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution[var] = v;
            }

            if (renameSubstitution.Count > 0)
            {
                IList<Literal> literals = new List<Literal>();

                foreach (Literal l in clause.getLiterals())
                {
                    literals.Add(substVisitor.subst(renameSubstitution, l));
                }
                Clause renamed = new Clause(literals);
                renamed.setProofStep(new ProofStepRenaming(renamed, clause
                        .getProofStep()));
                return renamed;
            }

            return clause;
        }

        public Chain standardizeApart(Chain chain, StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = variableCollector.collectAllVariables(chain);
            IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix() + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution[var] = v;
            }

            if (renameSubstitution.Count > 0)
            {
                IList<Literal> lits = new List<Literal>();

                foreach (Literal l in chain.getLiterals())
                {
                    AtomicSentence atom = (AtomicSentence)substVisitor.subst(renameSubstitution, l.getAtomicSentence());
                    lits.Add(l.newInstance(atom));
                }

                Chain renamed = new Chain(lits);

                renamed.setProofStep(new ProofStepRenaming(renamed, chain.getProofStep()));

                return renamed;
            }

            return chain;
        }

        public IDictionary<Variable, Term> standardizeApart(IList<Literal> l1Literals, IList<Literal> l2Literals, StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = new HashSet<Variable>();

            foreach (Literal pl in l1Literals)
            {
                foreach (var v in variableCollector.collectAllVariables(pl.getAtomicSentence()))
                    toRename.Add(v);
            }
            foreach (Literal nl in l2Literals)
            {
                foreach (var v in variableCollector.collectAllVariables(nl.getAtomicSentence()))
                    toRename.Add(v);
            }

            IDictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution[var] = v;
            }

            IList<Literal> posLits = new List<Literal>();
            IList<Literal> negLits = new List<Literal>();

            foreach (Literal pl in l1Literals)
            {
                posLits.Add(substVisitor.subst(renameSubstitution, pl));
            }
            foreach (Literal nl in l2Literals)
            {
                negLits.Add(substVisitor.subst(renameSubstitution, nl));
            }

            l1Literals.Clear();
            foreach (var v in posLits)
                l1Literals.Add(v);
            l2Literals.Clear();
            foreach (var v in negLits)
                l2Literals.Add(v);

            return renameSubstitution;
        }
    } 
}
