namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseBinaryResolvent : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Clause resolvent = null;
    private Literal posLiteral = null;
    private Literal negLiteral = null;
    private Clause parent1, parent2 = null;
    private Map<Variable, Term> subst = Factory.CreateMap<Variable, Term>();
    private Map<Variable, Term> renameSubst = Factory.CreateMap<Variable, Term>();

    public ProofStepClauseBinaryResolvent(Clause resolvent, Literal pl,
            Literal nl, Clause parent1, Clause parent2,
            Map<Variable, Term> subst, Map<Variable, Term> renameSubst)
    {
        this.resolvent = resolvent;
        this.posLiteral = pl;
        this.negLiteral = nl;
        this.parent1 = parent1;
        this.parent2 = parent2;
        this.subst.putAll(subst);
        this.renameSubst.putAll(renameSubst);
        this.predecessors.Add(parent1.getProofStep());
        this.predecessors.Add(parent2.getProofStep());
    }

    //
    // START-ProofStep
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

    public string getProof()
    {
        return resolvent.ToString();
    }

    public string getJustification()
    {
        int lowStep = parent1.getProofStep().getStepNumber();
        int highStep = parent2.getProofStep().getStepNumber();

        if (lowStep > highStep)
        {
            lowStep = highStep;
            highStep = parent1.getProofStep().getStepNumber();
        }

        return "Resolution: " + lowStep + ", " + highStep + "  [" + posLiteral
                + ", " + negLiteral + "], subst=" + subst + ", renaming="
                + renameSubst;
    }
    // END-ProofStep
    //
}
}
