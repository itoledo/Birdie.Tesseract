using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepFoChAssertFact : AbstractProofStep
    {
        //
        private IList<ProofStep> predecessors = new List<ProofStep>();
        //
        private Clause implication = null;
        private Literal fact = null;
        private IDictionary<Variable, Term> bindings = null;

        public ProofStepFoChAssertFact(Clause implication, Literal fact,
                IDictionary<Variable, Term> bindings, ProofStep predecessor)
        {
            this.implication = implication;
            this.fact = fact;
            this.bindings = bindings;
            if (null != predecessor)
            {
                predecessors.Add(predecessor);
            }
        }

        //
        // START-ProofStep

        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            StringBuilder sb = new StringBuilder();
            IList<Literal> nLits = implication.getNegativeLiterals();
            for (int i = 0; i < implication.getNumberNegativeLiterals(); i++)
            {
                sb.Append(nLits[i].getAtomicSentence());
                if (i != (implication.getNumberNegativeLiterals() - 1))
                {
                    sb.Append(" AND ");
                }
            }
            sb.Append(" => ");
            sb.Append(implication.getPositiveLiterals()[0]);
            return sb.ToString();
        }

        public override string getJustification()
        {
            return "Assert fact " + fact.ToString() + ", " + bindings;
        }
        // END-ProofStep
        //
    } 
}
