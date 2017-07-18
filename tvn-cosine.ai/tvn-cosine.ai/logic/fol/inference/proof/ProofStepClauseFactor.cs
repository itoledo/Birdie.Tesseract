namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseFactor : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Clause factor = null;
    private Clause factorOf = null;
    private Literal lx = null;
    private Literal ly = null;
    private Map<Variable, Term> subst = Factory.CreateMap<Variable, Term>();
    private Map<Variable, Term> renameSubst = Factory.CreateMap<Variable, Term>();

    public ProofStepClauseFactor(Clause factor, Clause factorOf, Literal lx,
            Literal ly, Map<Variable, Term> subst,
            Map<Variable, Term> renameSubst)
    {
        this.factor = factor;
        this.factorOf = factorOf;
        this.lx = lx;
        this.ly = ly;
        this.subst.putAll(subst);
        this.renameSubst.putAll(renameSubst);
        this.predecessors.Add(factorOf.getProofStep());
    }

    //
    // START-ProofStep
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

    public string getProof()
    {
        return factor.ToString();
    }

    public string getJustification()
    {
        return "Factor of " + factorOf.getProofStep().getStepNumber() + "  ["
                + lx + ", " + ly + "], subst=" + subst + ", renaming="
                + renameSubst;
    }
    // END-ProofStep
    //
}
}
