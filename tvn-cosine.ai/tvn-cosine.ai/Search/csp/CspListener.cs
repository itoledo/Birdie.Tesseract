namespace tvn.cosine.ai.search.csp
{
    /**
     * Interface which allows interested clients to register at a CSP solver
     * and follow its progress step by step.
     *
     * @author Ruediger Lunde
     */
    public interface CspListener<VAR, VAL>
        where VAR : Variable
    {
        /**
         * Informs about changed assignments and inference steps.
         *
         * @param csp        a CSP, possibly changed by an inference step.
         * @param assignment a new assignment or null if the last processing step was an inference step.
         * @param variable   a variable, whose domain or assignment value has been changed (may be null).
         */
        void stateChanged(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR variable);
    }
}
