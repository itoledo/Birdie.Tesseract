namespace tvn_cosine.languagedetector
{
    /// <summary>
    /// Language} is to store the detected language.
    /// </summary>
    public class Language
    {
        public string lang;
        public double prob;

        public Language(string lang, double prob)
        {
            this.lang = lang;
            this.prob = prob;
        }

        public override string ToString()
        {
            if (null == lang)
            {
                return string.Empty;
            }

            return string.Format("{0}:{1:0.0}", lang, prob);
        }
    }
}
