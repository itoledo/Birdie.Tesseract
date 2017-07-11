﻿using System;
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
    public class ProofStepChainReduction : AbstractProofStep
    {

    private List<ProofStep> predecessors = new ArrayList<ProofStep>();
    private Chain reduction = null;
    private Chain nearParent, farParent = null;
    private Map<Variable, Term> subst = null;

    public ProofStepChainReduction(Chain reduction, Chain nearParent,
            Chain farParent, Map<Variable, Term> subst)
    {
        this.reduction = reduction;
        this.nearParent = nearParent;
        this.farParent = farParent;
        this.subst = subst;
        this.predecessors.add(farParent.getProofStep());
        this.predecessors.add(nearParent.getProofStep());
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
        return reduction.toString();
    }

    @Override
    public String getJustification()
    {
        return "Reduction: " + nearParent.getProofStep().getStepNumber() + ","
                + farParent.getProofStep().getStepNumber() + " " + subst;
    }
    // END-ProofStep
    //
}

}
