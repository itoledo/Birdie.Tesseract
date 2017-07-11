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
    public class ProofStepChainContrapositive : AbstractProofStep
    {

        private List<ProofStep> predecessors = new ArrayList<ProofStep>();
        private Chain contrapositive = null;
        private Chain contrapositiveOf = null;

        public ProofStepChainContrapositive(Chain contrapositive,
                Chain contrapositiveOf)
        {
            this.contrapositive = contrapositive;
            this.contrapositiveOf = contrapositiveOf;
            this.predecessors.add(contrapositiveOf.getProofStep());
        }

        //
        // START-ProofStep
        @Override
    public List<ProofStep> getPredecessorSteps()
        {
            return Collections.unmodifiableList(predecessors);
        }

        @Override
    public String getProof()
        {
            return contrapositive.toString();
        }

        @Override
    public String getJustification()
        {
            return "Contrapositive: "
                    + contrapositiveOf.getProofStep().getStepNumber();
        }
        // END-ProofStep
        //
    }
}
