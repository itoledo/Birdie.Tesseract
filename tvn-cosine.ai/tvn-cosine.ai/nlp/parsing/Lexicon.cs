using System.Collections.Generic;
using tvn.cosine.ai.nlp.parsing.grammers;

namespace tvn.cosine.ai.nlp.parsing
{
    /**
     * The Lexicon Object appears on pg. 891 of the text and defines a simple
     * set of words for a certain language category and their associated probabilities.
     * 
     * Defining and using a lexicon saves us from listing out a large number of rules to
     * derive terminal strings in a grammar.
     * 
     * @author Jonathon
     *
     */
    public class Lexicon : Dictionary<string, IList<LexWord>>
    {

        public IList<Rule> getTerminalRules(string partOfSpeech)
        {
            IList<LexWord> lexWords = this[partOfSpeech.ToUpper()];
            IList<Rule> rules = new List<Rule>();
            if (lexWords.Count > 0)
            {
                for (int i = 0; i < lexWords.Count; i++)
                {
                    rules.Add(new Rule(partOfSpeech.ToUpper(),
                                               lexWords[i].word,
                                               lexWords[i].prob));
                }
            }
            return rules;
        }

        public IList<Rule> getAllTerminalRules()
        {
            List<Rule> allRules = new List<Rule>();
            foreach (var key in this.Keys)
            {
                allRules.AddRange(this.getTerminalRules(key));
            }

            return allRules;
        }

        public bool addEntry(string category, string word, float prob)
        {
            if (this.ContainsKey(category))
            {
                this[category].Add(new LexWord(word, prob));
            }
            else
            {
                this.Add(category, new List<LexWord>(new[] { new LexWord(word, prob) }));
            }

            return true;
        }

        public bool addLexWords(params string[] vargs)
        {

            string key;
            IList<LexWord> lexWords = new List<LexWord>();
            bool containsKey = false;
            // number of arguments must be key (1) + lexWord pairs ( x * 2 )
            if (vargs.Length % 2 != 1)
            {
                return false;
            }
            key = vargs[0].ToUpper();
            if (this.ContainsKey(key)) { containsKey = true; }

            for (int i = 1; i < vargs.Length; i++)
            {
                if (containsKey)
                {
                    this[key].Add(new LexWord(vargs[i], float.Parse(vargs[i + 1])));
                }
                else
                {
                    lexWords.Add(new LexWord(vargs[i], float.Parse(vargs[i + 1])));
                }
                i++;
            }
            if (!containsKey)
            {
                this.Add(key, lexWords);
            }
            return true;

        }

        /**
         * Add words to an lexicon from an existing lexicon. Using this 
         * you can combine lexicons.
         * @param l
         */
        public void addLexWords(Lexicon l)
        {
            foreach (var v in l)
            {
                if (this.ContainsKey(v.Key))
                {
                    for (int i = 0; i < v.Value.Count; i++)
                    {
                        this[v.Key].Add(v.Value[i]);
                    }
                }
                else
                {
                    this.Add(v.Key, v.Value);
                }
            }
        }
    }
}
