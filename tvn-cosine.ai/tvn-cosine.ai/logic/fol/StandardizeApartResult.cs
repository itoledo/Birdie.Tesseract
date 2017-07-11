using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class StandardizeApartResult
    {
        private Sentence originalSentence = null;
        private Sentence standardized = null;
        private IDictionary<Variable, Term> forwardSubstitution = null;
        private IDictionary<Variable, Term> reverseSubstitution = null;

        public StandardizeApartResult(Sentence originalSentence,
                Sentence standardized, IDictionary<Variable, Term> forwardSubstitution,
                IDictionary<Variable, Term> reverseSubstitution)
        {
            this.originalSentence = originalSentence;
            this.standardized = standardized;
            this.forwardSubstitution = forwardSubstitution;
            this.reverseSubstitution = reverseSubstitution;
        }

        public Sentence getOriginalSentence()
        {
            return originalSentence;
        }

        public Sentence getStandardized()
        {
            return standardized;
        }

        public IDictionary<Variable, Term> getForwardSubstitution()
        {
            return forwardSubstitution;
        }

        public IDictionary<Variable, Term> getReverseSubstitution()
        {
            return reverseSubstitution;
        }
    }

}
