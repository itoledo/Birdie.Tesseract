using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepGoal : AbstractProofStep
    {
        //
        private static readonly IList<ProofStep> _noPredecessors = new List<ProofStep>();
        //
        private Object proof = "";

        public ProofStepGoal(Object proof)
        {
            this.proof = proof;
        }

        //
        // START-ProofStep
        @Override
    public List<ProofStep> getPredecessorSteps()
        {
            return Collections.unmodifiableList(_noPredecessors);
        }

        @Override
    public String getProof()
        {
            return proof.toString();
        }

        @Override
    public String getJustification()
        {
            return "Goal";
        }
        // END-ProofStep
        //
    }

}
