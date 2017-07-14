using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using tvn.cosine.ai.logic.fol.inference.proof;

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
        private static IList<Literal> _emptyLiteralsList = new ReadOnlyCollection<Literal>(new List<Literal>());
        //
        private IList<Literal> literals = new List<Literal>();
        private ProofStep proofStep = null;

        public Chain()
        {
            // i.e. the empty chain
        }

        public Chain(IList<Literal> literals)
        {
            foreach (var v in literals)
                this.literals.Add(v);
        }

        public Chain(ISet<Literal> literals)
        {
            foreach (var v in literals)
                this.literals.Add(v);
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
            return literals.Count == 0;
        }

        public void addLiteral(Literal literal)
        {
            literals.Add(literal);
        }

        public Literal getHead()
        {
            if (0 == literals.Count)
            {
                return null;
            }
            return literals[0];
        }

        public IList<Literal> getTail()
        {
            if (0 == literals.Count)
            {
                return _emptyLiteralsList;
            }
            return new ReadOnlyCollection<Literal>(literals.Skip(1).ToList());
        }

        public int getNumberLiterals()
        {
            return literals.Count;
        }

        public IList<Literal> getLiterals()
        {
            return new ReadOnlyCollection<Literal>(literals);
        }

        /**
         * A contrapositive of a chain is a permutation in which a different literal
         * is placed at the front. The contrapositives of a chain are logically
         * equivalent to the original chain.
         * 
         * @return a list of contrapositives for this chain.
         */
        public IList<Chain> getContrapositives()
        {
            List<Chain> contrapositives = new List<Chain>();
            List<Literal> lits = new List<Literal>();

            for (int i = 1; i < literals.Count; ++i)
            {
                lits.Clear();
                lits.Add(literals[i]);

                foreach (var v in literals.Take(i))
                    lits.Add(v);

                foreach (var v in literals.Skip(i + 1).ToList())
                    lits.Add(v);

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

            for (int i = 0; i < literals.Count; ++i)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(literals[i].ToString());
            }

            sb.Append(">");

            return sb.ToString();
        }
    }

}
