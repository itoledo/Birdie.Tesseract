namespace tvn.cosine.ai.logic.fol
{
    public class StandardizeApart
    {
        private VariableCollector variableCollector = null;
        private SubstVisitor substVisitor = null;

        public StandardizeApart()
        {
            variableCollector = new VariableCollector();
            substVisitor = new SubstVisitor();
        }

        public StandardizeApart(VariableCollector variableCollector,
                SubstVisitor substVisitor)
        {
            this.variableCollector = variableCollector;
            this.substVisitor = substVisitor;
        }

        // Note: see page 327.
        public StandardizeApartResult standardizeApart(Sentence sentence,
                StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = variableCollector
                    .collectAllVariables(sentence);
            Map<Variable, Term> renameSubstitution = Factory.CreateMap<Variable, Term>();
            Map<Variable, Term> reverseSubstitution = Factory.CreateMap<Variable, Term>();

            for (Variable var : toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.contains(v));

                renameSubstitution.Put(var, v);
                reverseSubstitution.Put(v, var);
            }

            Sentence standardized = substVisitor.subst(renameSubstitution,
                    sentence);

            return new StandardizeApartResult(sentence, standardized,
                    renameSubstitution, reverseSubstitution);
        }

        public Clause standardizeApart(Clause clause,
                StandardizeApartIndexical standardizeApartIndexical)
        {

            ISet<Variable> toRename = variableCollector.collectAllVariables(clause);
            Map<Variable, Term> renameSubstitution = Factory.CreateMap<Variable, Term>();

            for (Variable var : toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.contains(v));

                renameSubstitution.Put(var, v);
            }

            if (renameSubstitution.size() > 0)
            {
                IQueue<Literal> literals = Factory.CreateQueue<Literal>();

                for (Literal l : clause.getLiterals())
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

        public Chain standardizeApart(Chain chain,
                StandardizeApartIndexical standardizeApartIndexical)
        {

            ISet<Variable> toRename = variableCollector.collectAllVariables(chain);
            Map<Variable, Term> renameSubstitution = Factory.CreateMap<Variable, Term>();

            for (Variable var : toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.contains(v));

                renameSubstitution.Put(var, v);
            }

            if (renameSubstitution.size() > 0)
            {
                IQueue<Literal> lits = Factory.CreateQueue<Literal>();

                for (Literal l : chain.getLiterals())
                {
                    AtomicSentence atom = (AtomicSentence)substVisitor.subst(
                            renameSubstitution, l.getAtomicSentence());
                    lits.Add(l.newInstance(atom));
                }

                Chain renamed = new Chain(lits);

                renamed.setProofStep(new ProofStepRenaming(renamed, chain
                        .getProofStep()));

                return renamed;
            }

            return chain;
        }

        public Map<Variable, Term> standardizeApart(IQueue<Literal> l1Literals,
                IQueue<Literal> l2Literals,
                StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = Factory.CreateSet<Variable>();

            for (Literal pl : l1Literals)
            {
                toRename.addAll(variableCollector.collectAllVariables(pl
                        .getAtomicSentence()));
            }
            for (Literal nl : l2Literals)
            {
                toRename.addAll(variableCollector.collectAllVariables(nl
                        .getAtomicSentence()));
            }

            Map<Variable, Term> renameSubstitution = Factory.CreateMap<Variable, Term>();

            for (Variable var : toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.contains(v));

                renameSubstitution.Put(var, v);
            }

            IQueue<Literal> posLits = Factory.CreateQueue<Literal>();
            IQueue<Literal> negLits = Factory.CreateQueue<Literal>();

            for (Literal pl : l1Literals)
            {
                posLits.Add(substVisitor.subst(renameSubstitution, pl));
            }
            for (Literal nl : l2Literals)
            {
                negLits.Add(substVisitor.subst(renameSubstitution, nl));
            }

            l1Literals.Clear();
            l1Literals.addAll(posLits);
            l2Literals.Clear();
            l2Literals.addAll(negLits);

            return renameSubstitution;
        }
    }
}
