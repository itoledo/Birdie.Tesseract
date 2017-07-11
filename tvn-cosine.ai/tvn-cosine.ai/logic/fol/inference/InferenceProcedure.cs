using tvn.cosine.ai.logic.fol.kb;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface InferenceProcedure
    {
        /**
         * 
         * @param kb
         *            the knowledge base against which the query is to be made.
         * @param query
         *            the query to be answered.
         * @return an InferenceResult.
         */
        InferenceResult ask(FOLKnowledgeBase kb, Sentence query);
    }
}
