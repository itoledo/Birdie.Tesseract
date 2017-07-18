namespace tvn.cosine.ai.logic.fol.kb.data
{
    /**
     * 
     * A Chain is a sequence of literals (while a clause is a set) - order is
     * important for a chain.
     * 
     * @see <a
     *      href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf"
     *      >Chain</a>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class Chain
    {
        private static IQueue<Literal> _emptyLiteralsList = Collections
                .unmodifiableList(Factory.CreateQueue<Literal>());
        //
        private IQueue<Literal> literals = Factory.CreateQueue<Literal>();
        private ProofStep proofStep = null;

        public Chain()
        {
            // i.e. the empty chain
        }

        public Chain(IQueue<Literal> literals)
        {
            this.literals.addAll(literals);
        }

        public Chain(Set<Literal> literals)
        {
            this.literals.addAll(literals);
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

        public bool isEmpty()
        {
            return literals.size() == 0;
        }

        public void addLiteral(Literal literal)
        {
            literals.Add(literal);
        }

        public Literal getHead()
        {
            if (0 == literals.size())
            {
                return null;
            }
            return literals.Get(0);
        }

        public IQueue<Literal> getTail()
        {
            if (0 == literals.size())
            {
                return _emptyLiteralsList;
            }
            return Collections
                    .unmodifiableList(literals.subList(1, literals.size()));
        }

        public int getNumberLiterals()
        {
            return literals.size();
        }

        public IQueue<Literal> getLiterals()
        {
            return Factory.CreateReadOnlyQueue<>(literals);
        }

        /**
         * A contrapositive of a chain is a permutation in which a different literal
         * is placed at the front. The contrapositives of a chain are logically
         * equivalent to the original chain.
         * 
         * @return a list of contrapositives for this chain.
         */
        public IQueue<Chain> getContrapositives()
        {
            IQueue<Chain> contrapositives = Factory.CreateQueue<Chain>();
            IQueue<Literal> lits = Factory.CreateQueue<Literal>();

            for (int i = 1; i < literals.size(); i++)
            {
                lits.Clear();
                lits.Add(literals.Get(i));
                lits.addAll(literals.subList(0, i));
                lits.addAll(literals.subList(i + 1, literals.size()));
                Chain cont = new Chain(lits);
                cont.setProofStep(new ProofStepChainContrapositive(cont, this));
                contrapositives.Add(cont);
            }

            return contrapositives;
        }

         
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<");

            for (int i = 0; i < literals.size(); i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(literals.Get(i).ToString());
            }

            sb.Append(">");

            return sb.ToString();
        }
    }
}
