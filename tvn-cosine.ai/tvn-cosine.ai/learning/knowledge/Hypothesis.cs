using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.learning.knowledge
{ 
    public class Hypothesis
    {
        private Sentence hypothesis = null;

        public Hypothesis(Sentence hypothesis)
        {
            this.hypothesis = hypothesis;
        }
         
        public Sentence getHypothesis()
        {
            return hypothesis;
        }
    }
}
