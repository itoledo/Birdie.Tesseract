namespace tvn.cosine.ai.search.csp.inferences
{
    public sealed class EmptyLog<VAR, VAL> : InferenceLog<VAR, VAL>
        where VAR : Variable
    {
        public bool isEmpty() { return true; }
        public bool inconsistencyFound() { return false; }
        public void undo(CSP<VAR, VAL> csp) { }
    }
}
