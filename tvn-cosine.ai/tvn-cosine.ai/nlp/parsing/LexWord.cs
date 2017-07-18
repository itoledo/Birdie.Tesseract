namespace tvn.cosine.ai.nlp.parsing
{
    public class LexWord
    {
        string word;
        Float prob;

        public LexWord(string word, Float prob)
        {
            this.word = word;
            this.prob = prob;
        }

        public string getWord() { return word; }
        public Float getProb() { return prob; }
    }
}
