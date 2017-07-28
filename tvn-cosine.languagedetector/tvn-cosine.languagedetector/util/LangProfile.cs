using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace tvn_cosine.languagedetector.util
{
    /// <summary>
    /// angProfile is a Language Profile Class.
    /// Users don't use this class directly.
    /// </summary>
    public class LangProfile
    {
        private const int MINIMUM_FREQ = 2;
        private const int LESS_FREQ_RATIO = 100000;
        public string name = null;
        public IDictionary<string, int> freq = new Dictionary<string, int>();
        public int[] n_words = new int[NGram.N_GRAM];

        public LangProfile() { }

        public LangProfile(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Add n-gram to profile
        /// </summary>
        /// <param name="gram"></param>
        public void add(string gram)
        {
            if (name == null || gram == null) return;   // Illegal
            int len = gram.Length;
            if (len < 1 || len > NGram.N_GRAM) return;  // Illegal
            ++n_words[len - 1];
            if (freq.ContainsKey(gram))
            {
                ++freq[gram];
            }
            else
            {
                freq[gram] = 1;
            }
        }

        /// <summary>
        /// Eliminate below less frequency n-grams and noise Latin alphabets
        /// </summary>
        public void omitLessFreq()
        {
            if (name == null) return;   // Illegal
            int threshold = n_words[0] / LESS_FREQ_RATIO;
            if (threshold < MINIMUM_FREQ) threshold = MINIMUM_FREQ;

            int roman = 0;
            foreach (string key in freq.Keys.ToList())
            {
                int count = freq[key];
                if (count <= threshold)
                {
                    n_words[key.Length - 1] -= count;
                    freq.Remove(key);
                }
                else
                {
                    if (Regex.IsMatch(key, "^[A-Za-z]$"))
                    {
                        roman += count;
                    }
                }
            }

            // roman check
            if (roman < n_words[0] / 3)
            {
                foreach (string key in freq.Keys.ToList())
                {
                    if (Regex.IsMatch(key, ".*[A-Za-z].*"))
                    {
                        n_words[key.Length - 1] -= freq[key];
                        freq.Remove(key);
                    }
                }

            }
        }

        /// <summary>
        /// Update the language profile with(fragmented) text.
        /// Extract n-grams from text and add their frequency into the profile.
        /// </summary>
        /// <param name="text">(fragmented) text to extract n-grams</param>
        public void update(string text)
        {
            if (text == null)
            {
                return;
            }

           text = NGram.normalize_vi(text);
            NGram gram = new NGram();
            for (int i = 0; i < text.Length; ++i)
            {
                gram.addChar(text[i]);
                for (int n = 1; n <= NGram.N_GRAM; ++n)
                {
                    add(gram.get(n));
                }
            }
        }
    }
}
