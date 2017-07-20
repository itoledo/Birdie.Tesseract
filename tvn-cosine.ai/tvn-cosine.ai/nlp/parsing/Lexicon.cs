using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.nlp.parsing.grammars;

namespace tvn.cosine.ai.nlp.parsing
{
    /**
     * The Lexicon object appears on pg. 891 of the text and defines a simple
     * set of words for a certain language category and their associated probabilities.
     * 
     * Defining and using a lexicon saves us from listing out a large number of rules to
     * derive terminal strings in a grammar.
     * 
     * @author Jonathon
     *
     */
    public class Lexicon : Map<string, IQueue<LexWord>>
    {
        public IQueue<Rule> getTerminalRules(string partOfSpeech)
        {
            string partOfSpeechUpperCase = partOfSpeech.ToUpper();
            IQueue<Rule> rules = Factory.CreateQueue<Rule>();

            if (this.ContainsKey(partOfSpeechUpperCase))
            {
                foreach (LexWord word in this.Get(partOfSpeechUpperCase))
                {
                    rules.Add(new Rule(partOfSpeechUpperCase, word.getWord(), word.getProb()));
                }
            }

            return rules;
        }

        public IQueue<Rule> getAllTerminalRules()
        {
            IQueue<Rule> allRules = Factory.CreateQueue<Rule>();
            IQueue<string> keys = this.GetKeys();

            foreach (string key in keys)
                allRules.AddAll(this.getTerminalRules(key));

            return allRules;
        }

        public bool addEntry(string category, string word, float prob)
        {
            if (this.ContainsKey(category))
                this.Get(category).Add(new LexWord(word, prob));
            else
                this.Put(category, Factory.CreateQueue<LexWord>(new[] { new LexWord(word, prob) }));

            return true;
        }

        public bool addLexWords(params string[] vargs)
        {
            IQueue<LexWord> lexWords = Factory.CreateQueue<LexWord>();
            bool containsKey = false;
            // number of arguments must be key (1) + lexWord pairs ( x * 2 )
            if (vargs.Length % 2 != 1)
                return false;

            string key = vargs[0].ToUpper();
            if (this.ContainsKey(key)) { containsKey = true; }

            for (int i = 1; i < vargs.Length;++i)
            {
                try
                {
                    if (containsKey)
                        this.Get(key).Add(new LexWord(vargs[i], float.Parse(vargs[i + 1])));
                    else
                        lexWords.Add(new LexWord(vargs[i], float.Parse(vargs[i + 1])));
                   ++i;
                }
                catch (NumberFormatException)
                {
                    System.Console.WriteLine("Supplied args have incorrect format.");
                    return false;
                }
            }
            if (!containsKey) { this.Put(key, lexWords); }
            return true;

        }

        /**
         * Add words to an lexicon from an existing lexicon. Using this 
         * you can combine lexicons.
         * @param lexicon
         */
        public void addLexWords(Lexicon lexicon)
        {
            foreach (var pair in lexicon)
            {
                string key = pair.GetKey();
                IQueue<LexWord> lexWords = pair.GetValue();

                if (this.ContainsKey(key))
                {
                    foreach (LexWord word in lexWords)
                        this.Get(key).Add(word);
                }
                else
                {
                    this.Put(key, lexWords);
                }
            }
        }
    }
}
