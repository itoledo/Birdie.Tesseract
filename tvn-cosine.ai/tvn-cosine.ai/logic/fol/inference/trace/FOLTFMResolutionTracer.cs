namespace tvn.cosine.ai.logic.fol.inference.trace
{
    public interface FOLTFMResolutionTracer
    {
        void stepStartWhile(Set<Clause> clauses, int totalNoClauses,
                int totalNoNewCandidateClauses);

        void stepOuterFor(Clause i);

        void stepInnerFor(Clause i, Clause j);

        void stepResolved(Clause iFactor, Clause jFactor, ISet<Clause> resolvents);

        void stepFinished(Set<Clause> clauses, InferenceResult result);
    }
}
