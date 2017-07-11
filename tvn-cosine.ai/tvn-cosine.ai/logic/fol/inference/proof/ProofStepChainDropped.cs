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
    public class ProofStepChainDropped : AbstractProofStep
    {

    private List<ProofStep> predecessors = new ArrayList<ProofStep>();
    private Chain dropped = null;
    private Chain droppedOff = null;

    public ProofStepChainDropped(Chain dropped, Chain droppedOff)
    {
        this.dropped = dropped;
        this.droppedOff = droppedOff;
        this.predecessors.add(droppedOff.getProofStep());
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
        return dropped.toString();
    }

    @Override
    public String getJustification()
    {
        return "Dropped: " + droppedOff.getProofStep().getStepNumber();
    }
    // END-ProofStep
    //
}

}
