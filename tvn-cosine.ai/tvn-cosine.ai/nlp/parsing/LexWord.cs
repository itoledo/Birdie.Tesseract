namespace tvn.cosine.ai.nlp.parsing
{
    public class LexWord
    {
       public string word;
       public float prob;
       
         public LexWord(string word, float prob)
        {
            this.word = word;
            this.prob = prob;
        }

        public string getWord() { return word; }
        public float getProb() { return prob; }
    }
}
