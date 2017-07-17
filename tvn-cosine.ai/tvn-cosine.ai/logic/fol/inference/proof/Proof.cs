namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public interface Proof
    {
        /**
         * 
         * @return A list of proof steps that show how an answer was derived.
         */
        List<ProofStep> getSteps();

        /**
         * 
         * @return a Map of bindings for any variables that were in the original
         *         query. Will be an empty Map if no variables existed in the
         *         original query.
         */
        Map<Variable, Term> getAnswerBindings();

        /**
         * 
         * @param updatedBindings
         *            allows for the bindings to be renamed. Note: should not be
         *            used for any other reason.
         */
        void replaceAnswerBindings(Map<Variable, Term> updatedBindings);
    }
}
