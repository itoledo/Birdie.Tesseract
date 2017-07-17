namespace tvn.cosine.ai.logic.fol.inference.trace
{
    public interface FOLModelEliminationTracer
    {
        void reset();

        void increment(int depth, int noFarParents);
    }
}
