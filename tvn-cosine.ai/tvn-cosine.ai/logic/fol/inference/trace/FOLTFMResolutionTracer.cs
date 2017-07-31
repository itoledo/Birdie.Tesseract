using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.trace
{
    public interface FOLTFMResolutionTracer
    {
        void stepStartWhile(ISet<Clause> clauses, int totalNoClauses, int totalNoNewCandidateClauses);
        void stepOuterFor(Clause i);
        void stepInnerFor(Clause i, Clause j);
        void stepResolved(Clause iFactor, Clause jFactor, ISet<Clause> resolvents);
        void stepFinished(ISet<Clause> clauses, InferenceResult result);
    }
}
