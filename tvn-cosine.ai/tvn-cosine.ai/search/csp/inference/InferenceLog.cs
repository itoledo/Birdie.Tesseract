namespace tvn.cosine.ai.search.csp.inference
{
    /**
     * Returns an empty inference log.
     */
    public class InferenceEmptyLog<VAR, VAL> : InferenceLog<VAR, VAL>
        where VAR : Variable
    {
        public bool isEmpty() { return true; }
        public bool inconsistencyFound() { return false; }
        public void undo(CSP<VAR, VAL> csp) { }
    }

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