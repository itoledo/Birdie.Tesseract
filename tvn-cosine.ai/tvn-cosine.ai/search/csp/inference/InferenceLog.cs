namespace tvn.cosine.ai.search.csp.inference
{
    /**
     * Provides information about (1) whether changes have been performed, (2) possibly inferred empty domains , and
     * (3) how to restore the CSP.
     *
     * @author Ruediger Lunde
     */
    public interface InferenceLog<VAR : Variable, VAL>
    {
        bool isEmpty();
        bool inconsistencyFound();
        void undo(CSP<VAR, VAL> csp);

        /**
         * Returns an empty inference log.
         */
        static <VAR : Variable, VAL> InferenceLog<VAR, VAL> emptyLog()
        {
            return new InferenceLog<VAR, VAL>() {
             
            public bool isEmpty() { return true; }

         
                public bool inconsistencyFound() { return false; }

         
                public void undo(CSP<VAR, VAL> csp) { }
    };
}
}

}
