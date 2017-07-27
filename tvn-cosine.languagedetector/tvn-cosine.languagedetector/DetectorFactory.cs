using System.Collections.Generic;

namespace tvn_cosine.languagedetector
{
    /// <summary>
    /// Language Detector Factory Class.
    /// <para /> 
    /// This class manages an initialization and constructions of Detector. 
    /// <para /> 
    /// Before using language detection library, 
    /// load profiles with DetectorFactory#loadProfile(string) method
    /// and set initialization parameters.
    /// <para />
    /// When the language detection,
    /// construct Detector instance via DetectorFactory#create().
    /// <para />
    /// See also Detector's sample code. 
    /// </summary>
    public class DetectorFactory
    {
        static private DetectorFactory instance_ = new DetectorFactory();

        public IDictionary<string, double[]> wordLangProbMap;
        public IList<string> langlist;
        public long? seed = null;

        private DetectorFactory()
        {
            wordLangProbMap = new Dictionary<string, double[]>();
            langlist = new List<string>();
        }

        /**
         * Load profiles from specified directory.
         * This method must be called once before language detection.
         *  
         * @param profileDirectory profile directory path
         * @throws LangDetectException  Can't open profiles(error code = {@link ErrorCode#FileLoadError})
         *                              or profile's format is wrong (error code = {@link ErrorCode#FormatError})
         */
        public static void loadProfile(string profileDirectory) throws LangDetectException
        {
            loadProfile(new File(profileDirectory));
    }

    /**
     * Load profiles from specified directory.
     * This method must be called once before language detection.
     *  
     * @param profileDirectory profile directory path
     * @throws LangDetectException  Can't open profiles(error code = {@link ErrorCode#FileLoadError})
     *                              or profile's format is wrong (error code = {@link ErrorCode#FormatError})
     */
    public static void loadProfile(File profileDirectory) throws LangDetectException
    {
        File []
        listFiles = profileDirectory.listFiles();
        if (listFiles == null)
            throw new LangDetectException(ErrorCode.NeedLoadProfileError, "Not found profile: " + profileDirectory);

    int langsize = listFiles.length, index = 0;
        for (File file: listFiles) {
            if (file.getName().startsWith(".") || !file.isFile()) continue;
            FileInputStream is = null;
            try {
                is = new FileInputStream(file);
    LangProfile profile = JSON.decode(is, LangProfile.class);
                addProfile(profile, index, langsize);
                ++index;
            } catch (JSONException e) {
                throw new LangDetectException(ErrorCode.FormatError, "profile format error in '" + file.getName() + "'");
            } catch (IOException e) {
                throw new LangDetectException(ErrorCode.FileLoadError, "can't open '" + file.getName() + "'");
            } finally {
                try {
                    if (is!=null) is.close();
                } catch (IOException e) {}
            }
        }
    }

    /**
     * Load profiles from specified directory.
     * This method must be called once before language detection.
     *  
     * @param profileDirectory profile directory path
     * @throws LangDetectException  Can't open profiles(error code = {@link ErrorCode#FileLoadError})
     *                              or profile's format is wrong (error code = {@link ErrorCode#FormatError})
     */
    public static void loadProfile(List<string> json_profiles) throws LangDetectException
{
        int index = 0;
        int langsize = json_profiles.size();
        if (langsize < 2)
            throw new LangDetectException(ErrorCode.NeedLoadProfileError, "Need more than 2 profiles");
            
        for (string json: json_profiles) {
            try {
                LangProfile profile = JSON.decode(json, LangProfile.class);
                addProfile(profile, index, langsize);
                ++index;
            } catch (JSONException e) {
                throw new LangDetectException(ErrorCode.FormatError, "profile format error");
            }
        }
    }

    /**
     * @param profile
     * @param langsize 
     * @param index 
     * @throws LangDetectException 
     */
    static /* package scope */ void addProfile(LangProfile profile, int index, int langsize) throws LangDetectException
{
    string lang = profile.name;
        if (instance_.langlist.contains(lang)) {
        throw new LangDetectException(ErrorCode.DuplicateLangError, "duplicate the same language profile");
    }
    instance_.langlist.add(lang);
        for (string word: profile.freq.keySet()) {
        if (!instance_.wordLangProbMap.containsKey(word))
        {
            instance_.wordLangProbMap.put(word, new double[langsize]);
        }
        int length = word.length();
        if (length >= 1 && length <= 3)
        {
            double prob = profile.freq.get(word).doubleValue() / profile.n_words[length - 1];
            instance_.wordLangProbMap.get(word)[index] = prob;
        }
    }
}

/**
 * Clear loaded language profiles (reinitialization to be available)
 */
static public void clear()
{
    instance_.langlist.clear();
    instance_.wordLangProbMap.clear();
}

/**
 * Construct Detector instance
 * 
 * @return Detector instance
 * @throws LangDetectException 
 */
static public Detector create() throws LangDetectException
{
        return createDetector();
}

/**
 * Construct Detector instance with smoothing parameter 
 * 
 * @param alpha smoothing parameter (default value = 0.5)
 * @return Detector instance
 * @throws LangDetectException 
 */
public static Detector create(double alpha) throws LangDetectException
{
    Detector detector = createDetector();
    detector.setAlpha(alpha);
        return detector;
}

static private Detector createDetector() throws LangDetectException
{
        if (instance_.langlist.size()==0)
            throw new LangDetectException(ErrorCode.NeedLoadProfileError, "need to load profiles");
Detector detector = new Detector(instance_);
        return detector;
    }
    
    public static void setSeed(long seed)
{
    instance_.seed = seed;
}

public static final List<string> getLangList()
{
    return Collections.unmodifiableList(instance_.langlist);
}
}

}
