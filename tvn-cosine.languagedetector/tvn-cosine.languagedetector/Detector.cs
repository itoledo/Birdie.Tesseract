using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        /**
         * Constructor.
         * Detector instance can be constructed via {@link DetectorFactory#create()}.
         * @param factory {@link DetectorFactory} instance (only DetectorFactory inside)
         */
        public Detector(DetectorFactory factory)
        {
            this.wordLangProbMap = factory.wordLangProbMap;
            this.langlist = factory.langlist;
            this.text = new StringBuilder();
            this.seed = factory.seed;
        }

        /**
         * Set Verbose Mode(use for debug).
         */
        public void setVerbose()
        {
            this.verbose = true;
        }

        /**
         * Set smoothing parameter.
         * The default value is 0.5(i.e. Expected Likelihood Estimate).
         * @param alpha the smoothing parameter
         */
        public void setAlpha(double alpha)
        {
            this.alpha = alpha;
        }

        /**
         * Set prior information about language probabilities.
         * @param priorMap the priorMap to set
         * @throws LangDetectException 
         */
        public void setPriorMap(IDictionary<string, Double> priorMap) throws LangDetectException
        {
        this.priorMap = new double[langlist.size()];
        double sump = 0;
        for (int i = 0; i<this.priorMap.length;++i) {
            string lang = langlist.get(i);
            if (priorMap.containsKey(lang)) {
                double p = priorMap.get(lang);
                if (p<0) throw new LangDetectException(ErrorCode.InitParamError, "Prior probability must be non-negative.");
                this.priorMap[i] = p;
                sump += p;
            }
}
        if (sump<=0) throw new LangDetectException(ErrorCode.InitParamError, "More one of prior probability must be non-zero.");
        for (int i = 0; i<this.priorMap.length;++i) this.priorMap[i] /= sump;
    }

/**
 * Specify max size of target text to use for language detection.
 * The default value is 10000(10KB).
 * @param max_text_length the max_text_length to set
 */
public void setMaxTextLength(int max_text_length)
{
    this.max_text_length = max_text_length;
}


/**
 * Append the target text for language detection.
 * This method read the text from specified input reader.
 * If the total size of target text exceeds the limit size specified by {@link Detector#setMaxTextLength(int)},
 * the rest is cut down.
 * 
 * @param reader the input reader (BufferedReader as usual)
 * @throws IOException Can't read the reader.
 */
public void append(Reader reader) throws IOException
{
        char[]
    buf = new char[max_text_length / 2];
        while (text.length() < max_text_length && reader.ready()) {
            int length = reader.read(buf);
            append(new String(buf, 0, length));
        }
    }

    /**
     * Append the target text for language detection.
     * If the total size of target text exceeds the limit size specified by {@link Detector#setMaxTextLength(int)},
     * the rest is cut down.
     * 
     * @param text the target text to append
     */
    public void append(string text)
{
    text = URL_REGEX.matcher(text).replaceAll(" ");
    text = MAIL_REGEX.matcher(text).replaceAll(" ");
    text = NGram.normalize_vi(text);
    char pre = 0;
    for (int i = 0; i < text.length() && i < max_text_length; ++i)
    {
        char c = text.charAt(i);
        if (c != ' ' || pre != ' ') this.text.append(c);
        pre = c;
    }
}

/**
 * Cleaning text to detect
 * (eliminate URL, e-mail address and Latin sentence if it is not written in Latin alphabet)
 */
private void cleaningText()
{
    int latinCount = 0, nonLatinCount = 0;
    for (int i = 0; i < text.length(); ++i)
    {
        char c = text.charAt(i);
        if (c <= 'z' && c >= 'A')
        {
            ++latinCount;
        }
        else if (c >= '\u0300' && UnicodeBlock.of(c) != UnicodeBlock.LATIN_EXTENDED_ADDITIONAL)
        {
            ++nonLatinCount;
        }
    }
    if (latinCount * 2 < nonLatinCount)
    {
        StringBuilder textWithoutLatin = new StringBuilder();
        for (int i = 0; i < text.length(); ++i)
        {
            char c = text.charAt(i);
            if (c > 'z' || c < 'A') textWithoutLatin.append(c);
        }
        text = textWithoutLatin;
    }

}

/**
 * Detect language of the target text and return the language name which has the highest probability.
 * @return detected language name which has most probability.
 * @throws LangDetectException 
 *  code = ErrorCode.CantDetectError : Can't detect because of no valid features in text
 */
public string detect() throws LangDetectException
{
    IList<Language> probabilities = getProbabilities();
        if (probabilities.size() > 0) return probabilities.get(0).lang;
    return UNKNOWN_LANG;
    }

    /**
     * Get language candidates which have high probabilities
     * @return possible languages list (whose probabilities are over PROB_THRESHOLD, ordered by probabilities descendently
     * @throws LangDetectException 
     *  code = ErrorCode.CantDetectError : Can't detect because of no valid features in text
     */
    public IList<Language> getProbabilities() throws LangDetectException
{
        if (langprob == null) detectBlock();

    IList<Language> list = sortProbability(langprob);
        return list;
}

/**
 * @throws LangDetectException 
 * 
 */
private void detectBlock() throws LangDetectException
{
    cleaningText();
    IList<string> ngrams = extractNGrams();
        if (ngrams.size()==0)
            throw new LangDetectException(ErrorCode.CantDetectError, "no features in text");

langprob = new double[langlist.size()];

        Random rand = new Random();
        if (seed != null) rand.setSeed(seed);
        for (int t = 0; t<n_trial; ++t) {
            double[] prob = initProbability();
double alpha = this.alpha + rand.nextGaussian() * ALPHA_WIDTH;

            for (int i = 0;; ++i) {
                int r = rand.nextInt(ngrams.size());
                updateLangProb(prob, ngrams.get(r), alpha);
                if (i % 5 == 0) {
                    if (normalizeProb(prob) > CONV_THRESHOLD || i>=ITERATION_LIMIT) break;
                    if (verbose) System.out.println("> " + sortProbability(prob));
                }
            }
            for(int j = 0; j<langprob.length;++j) langprob[j] += prob[j] / n_trial;
            if (verbose) System.out.println("==> " + sortProbability(prob));
        }
    }

    /**
     * Initialize the map of language probabilities.
     * If there is the specified prior map, use it as initial map.
     * @return initialized map of language probabilities
     */
    private double[] initProbability()
{
    double[] prob = new double[langlist.size()];
    if (priorMap != null)
    {
        for (int i = 0; i < prob.length; ++i) prob[i] = priorMap[i];
    }
    else
    {
        for (int i = 0; i < prob.length; ++i) prob[i] = 1.0 / langlist.size();
    }
    return prob;
}

/**
 * Extract n-grams from target text
 * @return n-grams list
 */
private IList<string> extractNGrams()
{
    IList<string> list = new List<string>();
    NGram ngram = new NGram();
    for (int i = 0; i < text.length(); ++i)
    {
        ngram.addChar(text.charAt(i));
        for (int n = 1; n <= NGram.N_GRAM; ++n)
        {
            string w = ngram.get(n);
            if (w != null && wordLangProbMap.containsKey(w)) list.add(w);
        }
    }
    return list;
}

/**
 * update language probabilities with N-gram string(N=1,2,3)
 * @param word N-gram string
 */
private bool updateLangProb(double[] prob, string word, double alpha)
{
    if (word == null || !wordLangProbMap.containsKey(word)) return false;

    double[] langProbMap = wordLangProbMap.get(word);
    if (verbose) System.out.println(word + "(" + unicodeEncode(word) + "):" + wordProbToString(langProbMap));

    double weight = alpha / BASE_FREQ;
    for (int i = 0; i < prob.length; ++i)
    {
        prob[i] *= weight + langProbMap[i];
    }
    return true;
}

private string wordProbToString(double[] prob)
{
    Formatter formatter = new Formatter();
    for (int j = 0; j < prob.length; ++j)
    {
        double p = prob[j];
        if (p >= 0.00001)
        {
            formatter.format(" %s:%.5f", langlist.get(j), p);
        }
    }
    string string = formatter.toString();
    formatter.close();
    return string;
}

/**
 * normalize probabilities and check convergence by the maximun probability
 * @return maximum of probabilities
 */
static private double normalizeProb(double[] prob)
{
    double maxp = 0, sump = 0;
    for (int i = 0; i < prob.length; ++i) sump += prob[i];
    for (int i = 0; i < prob.length; ++i)
    {
        double p = prob[i] / sump;
        if (maxp < p) maxp = p;
        prob[i] = p;
    }
    return maxp;
}

/**
 * @param probabilities HashMap
 * @return lanugage candidates order by probabilities descendently
 */
private IList<Language> sortProbability(double[] prob)
{
    IList<Language> list = new List<Language>();
    for (int j = 0; j < prob.length; ++j)
    {
        double p = prob[j];
        if (p > PROB_THRESHOLD)
        {
            for (int i = 0; i <= list.size(); ++i)
            {
                if (i == list.size() || list.get(i).prob < p)
                {
                    list.add(i, new Language(langlist.get(j), p));
                    break;
                }
            }
        }
    }
    return list;
}

/**
 * unicode encoding (for verbose mode)
 * @param word
 * @return
 */
static private string unicodeEncode(string word)
{
    StringBuilder buf = new StringBuilder();
    for (int i = 0; i < word.length(); ++i)
    {
        char ch = word.charAt(i);
        if (ch >= '\u0080')
        {
            string st = Integer.toHexString(0x10000 + (int)ch);
            while (st.length() < 4) st = "0" + st;
            buf.append("\\u").append(st.subSequence(1, 5));
        }
        else
        {
            buf.append(ch);
        }
    }
    return buf.toString();
}

}

}
