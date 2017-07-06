namespace tvn.cosine.ai.search.csp.inferences
{
    /**
     * Provides information about (1) whether changes have been performed, (2) possibly inferred empty domains , and
     * (3) how to restore the CSP.
     *
     * @author Ruediger Lunde
     */
    public interface InferenceLog<VAR, VAL>
        where VAR : Variable
    {
        bool isEmpty();
        bool inconsistencyFound();
        void undo(CSP<VAR, VAL> csp);
    } 
}
