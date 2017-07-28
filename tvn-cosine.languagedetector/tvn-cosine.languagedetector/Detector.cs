using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tvn_cosine.languagedetector.util;

namespace tvn_cosine.languagedetector
{
    /// <summary>
    /// Detector class is to detect language from specified text. 
    /// Its instance is able to be constructed via the factory class DetectorFactory.
    /// <para />
    /// After appending a target text to the Detector instance with #append(Reader) or #append(String),
    /// the detector provides the language detection results for target text via #detect() or #getProbabilities().
    /// #detect() method returns a single language name which has the highest probability.
    /// #getProbabilities() methods returns a list of multiple languages and their probabilities.
    /// <para />  
    /// The detector has some parameters for language detection.
    /// See #setAlpha(double), #setMaxTextLength(int) and #setPriorMap(HashMap). 
    /// </summary>
    public class Detector
    {
        private const double ALPHA_DEFAULT = 0.5;
        private const double ALPHA_WIDTH = 0.05;

        private const int ITERATION_LIMIT = 1000;
        private const double PROB_THRESHOLD = 0.1;
        private const double CONV_THRESHOLD = 0.99999;
        private const int BASE_FREQ = 10000;
        private const string UNKNOWN_LANG = "unknown";

        private static readonly Regex URL_REGEX = new Regex("https?://[-_.?&~;+=/#0-9A-Za-z]{1,2076}");
        private static readonly Regex MAIL_REGEX = new Regex("[-_.0-9A-Za-z]{1,64}@[-_0-9A-Za-z]{1,255}[-_.0-9A-Za-z]{1,255}");

        private readonly IDictionary<string, double[]> wordLangProbMap;
        private readonly IList<string> langlist;

        private StringBuilder text;
        private double[] langprob = null;

        private double alpha = ALPHA_DEFAULT;
        private int n_trial = 7;
        private int max_text_length = 10000;
        private double[] priorMap = null;
        private bool verbose = false;
        private long? seed = null;

        /// <summary>
        /// Detector instance can be constructed via etectorFactory#create().
        /// </summary>
        /// <param name="factory">DetectorFactory instance (only DetectorFactory inside)</param>
        public Detector(DetectorFactory factory)
        {
            this.wordLangProbMap = factory.wordLangProbMap;
            this.langlist = factory.langlist;
            this.text = new StringBuilder();
            this.seed = factory.seed;
        }

        /// <summary>
        /// Set Verbose Mode(use for debug).
        /// </summary>
        public void setVerbose()
        {
            this.verbose = true;
        }

        /// <summary>
        /// Set smoothing parameter.
        /// The default value is 0.5(i.e. Expected Likelihood Estimate).
        /// </summary>
        /// <param name="alpha">the smoothing parameter</param>
        public void setAlpha(double alpha)
        {
            this.alpha = alpha;
        }


        /// <summary>
        /// Set prior information about language probabilities.
        /// </summary>
        /// <param name="priorMap">the priorMap to set</param>
        /// <exception cref="LangDetectException" />
        public void setPriorMap(IDictionary<string, double> priorMap)
        {
            this.priorMap = new double[langlist.Count];
            double sump = 0;
            for (int i = 0; i < this.priorMap.Length; ++i)
            {
                string lang = langlist[i];
                if (priorMap.ContainsKey(lang))
                {
                    double p = priorMap[lang];
                    if (p < 0)
                    {
                        throw new LangDetectException(ErrorCode.InitParamError, "Prior probability must be non-negative.");
                    }
                    this.priorMap[i] = p;
                    sump += p;
                }
            }
            if (sump <= 0)
            {
                throw new LangDetectException(ErrorCode.InitParamError, "More one of prior probability must be non-zero.");
            }

            for (int i = 0; i < this.priorMap.Length; ++i)
            {
                this.priorMap[i] /= sump;
            }
        }

        /// <summary>
        /// Specify max size of target text to use for language detection.
        /// The default value is 10000(10KB).
        /// </summary>
        /// <param name="max_text_length">the max_text_length to set</param>
        public void setMaxTextLength(int max_text_length)
        {
            this.max_text_length = max_text_length;
        }

        /// <summary>
        /// Append the target text for language detection.
        /// This method read the text from specified input reader.
        /// If the total size of target text exceeds the limit size specified by Detector#setMaxTextLength(int)},
        /// the rest is cut down.
        /// </summary>
        /// <param name="reader">the input reader (BufferedReader as usual)</param>
        public void append(StreamReader reader)
        {
            char[] buf = new char[max_text_length / 2];
            while (text.Length < max_text_length && !reader.EndOfStream)
            {
                int length = reader.Read(buf, 0, buf.Length);
                append(new string(buf, 0, length));
            }
        }

        /// <summary>
        /// Append the target text for language detection.
        /// If the total size of target text exceeds the limit size specified by {@link Detector#setMaxTextLength(int)},
        /// the rest is cut down.
        /// </summary>
        /// <param name="text">the target text to append</param>
        public void append(string text)
        {
            text = URL_REGEX.Replace(text, " ");
            text = MAIL_REGEX.Replace(text, " ");
            // text = NGram.normalize_vi(text);
            char? pre = null;
            for (int i = 0; i < text.Length && i < max_text_length; ++i)
            {
                char c = text[i];
                if (c != ' ' || pre.Value != ' ')
                {
                    this.text.Append(c);
                }
                pre = c;
            }
        }

        /// <summary>
        /// Cleaning text to detect
        /// (eliminate URL, e-mail address and Latin sentence if it is not written in Latin alphabet)
        /// </summary>
        private void cleaningText()
        {
            int latinCount = 0, nonLatinCount = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                char c = text[i];
                if (c <= 'z' && c >= 'A')
                {
                    ++latinCount;
                }
                else if (c >= '\u0300' && !Regex.IsMatch(c.ToString(), @"\p{IsLatinExtendedAdditional}+"))
                {
                    ++nonLatinCount;
                }
            }
            if (latinCount * 2 < nonLatinCount)
            {
                StringBuilder textWithoutLatin = new StringBuilder();
                for (int i = 0; i < text.Length; ++i)
                {
                    char c = text[i];
                    if (c > 'z' || c < 'A')
                    {
                        textWithoutLatin.Append(c);
                    }
                }
                text = textWithoutLatin;
            }

        }

        /// <summary>
        /// Detect language of the target text and return the language name which has the highest probability.
        /// </summary>
        /// <returns>detected language name which has most probability.</returns>
        /// <exception cref="LangDetectException">code = ErrorCode.CantDetectError : Can't detect because of no valid features in text</exception>
        public string detect()
        {
            IList<Language> probabilities = getProbabilities();
            if (probabilities.Count > 0)
            {
                return probabilities[0].lang;
            }
            return UNKNOWN_LANG;
        }

        /// <summary>
        /// Get language candidates which have high probabilities
        /// </summary>
        /// <returns>possible languages list (whose probabilities are over PROB_THRESHOLD, ordered by probabilities descendently</returns>
        /// <exception cref="LangDetectException">code = ErrorCode.CantDetectError : Can't detect because of no valid features in text</exception>
        public IList<Language> getProbabilities()
        {
            if (langprob == null)
            {
                detectBlock();
            }

            IList<Language> list = sortProbability(langprob);
            return list;
        }

        private void detectBlock()
        {
            cleaningText();
            IList<string> ngrams = extractNGrams();
            if (ngrams.Count == 0)
            {
                throw new LangDetectException(ErrorCode.CantDetectError, "no features in text");
            }

            langprob = new double[langlist.Count];

            Random rand = new Random();
            if (seed != null)
            {
                rand = new Random((int)seed.Value);
            }
            for (int t = 0; t < n_trial; ++t)
            {
                double[] prob = initProbability();
                double alpha = this.alpha + rand.NextGaussian() * ALPHA_WIDTH;

                for (int i = 0; ; ++i)
                {
                    int r = rand.Next(ngrams.Count);
                    updateLangProb(prob, ngrams[r], alpha);
                    if (i % 5 == 0)
                    {
                        if (normalizeProb(prob) > CONV_THRESHOLD || i >= ITERATION_LIMIT) break;
                        if (verbose)
                        {
                            System.Console.WriteLine("> " + sortProbability(prob));
                        }
                    }
                }
                for (int j = 0; j < langprob.Length; ++j)
                {
                    langprob[j] += prob[j] / n_trial;
                }
                if (verbose)
                {
                    System.Console.WriteLine("==> " + sortProbability(prob));
                }
            }
        }

        /// <summary>
        /// Initialize the map of language probabilities.
        /// If there is the specified prior map, use it as initial map.
        /// </summary>
        /// <returns>initialized map of language probabilities</returns>
        private double[] initProbability()
        {
            double[] prob = new double[langlist.Count];
            if (priorMap != null)
            {
                for (int i = 0; i < prob.Length; ++i) prob[i] = priorMap[i];
            }
            else
            {
                for (int i = 0; i < prob.Length; ++i) prob[i] = 1.0 / langlist.Count;
            }
            return prob;
        }

        /// <summary>
        /// Extract n-grams from target text
        /// </summary>
        /// <returns>n-grams list</returns>
        private IList<string> extractNGrams()
        {
            IList<string> list = new List<string>();
            NGram ngram = new NGram();
            for (int i = 0; i < text.Length; ++i)
            {
                ngram.addChar(text[i]);
                for (int n = 1; n <= NGram.N_GRAM; ++n)
                {
                    string w = ngram.get(n);
                    if (w != null && wordLangProbMap.ContainsKey(w))
                    {
                        list.Add(w);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// update language probabilities with N-gram string(N=1,2,3)
        /// </summary>
        /// <param name="prob"></param>
        /// <param name="word">N-gram string</param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        private bool updateLangProb(double[] prob, string word, double alpha)
        {
            if (word == null || !wordLangProbMap.ContainsKey(word))
            {
                return false;
            }

            double[] langProbMap = wordLangProbMap[word];
            if (verbose)
            {
                System.Console.WriteLine(word + "(" + unicodeEncode(word) + "):" + wordProbToString(langProbMap));
            }

            double weight = alpha / BASE_FREQ;
            for (int i = 0; i < prob.Length; ++i)
            {
                prob[i] *= weight + langProbMap[i];
            }
            return true;
        }

        private string wordProbToString(double[] prob)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < prob.Length; ++j)
            {
                double p = prob[j];
                if (p >= 0.00001)
                {
                    sb.Append(string.Format(" {0}:{1.#####}", langlist[j], p));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// normalize probabilities and check convergence by the maximun probability
        /// </summary>
        /// <param name="prob"></param>
        /// <returns>maximum of probabilities</returns>
        static private double normalizeProb(double[] prob)
        {
            double maxp = 0, sump = 0;
            for (int i = 0; i < prob.Length; ++i)
            {
                sump += prob[i];
            }

            for (int i = 0; i < prob.Length; ++i)
            {
                double p = prob[i] / sump;
                if (maxp < p) maxp = p;
                prob[i] = p;
            }
            return maxp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prob">HashMap</param>
        /// <returns>lanugage candidates order by probabilities descendently</returns>
        private IList<Language> sortProbability(double[] prob)
        {
            IList<Language> list = new List<Language>();
            for (int j = 0; j < prob.Length; ++j)
            {
                double p = prob[j];
                if (p > PROB_THRESHOLD)
                {
                    for (int i = 0; i <= list.Count; ++i)
                    {
                        if (i == list.Count || list[i].prob < p)
                        {
                            list.Insert(i, new Language(langlist[j], p));
                            break;
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// unicode encoding (for verbose mode)
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        static private string unicodeEncode(string word)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < word.Length; ++i)
            {
                char ch = word[i];
                if (ch >= '\u0080')
                {
                    string st = (0x10000 + (int)ch).ToString("X5");
                    while (st.Length < 4)
                    {
                        st = "0" + st;
                    }
                    buf.Append("\\u").Append(st.Substring(1, 5));
                }
                else
                {
                    buf.Append(ch);
                }
            }
            return buf.ToString();
        }
    }
}
