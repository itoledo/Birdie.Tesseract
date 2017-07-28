using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using tvn_cosine.languagedetector.util;

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
        private static DetectorFactory instance_ = new DetectorFactory();

        public IDictionary<string, double[]> wordLangProbMap;
        public IList<string> langlist;
        public long? seed = null;

        private DetectorFactory()
        {
            wordLangProbMap = new Dictionary<string, double[]>();
            langlist = new List<string>();
        }

        /// <summary>
        /// Load profiles from specified directory.
        /// This method must be called once before language detection.
        /// </summary>
        /// <param name="profileDirectory">profile directory path</param>
        /// <exception cref="LangDetectException">
        /// Can't open profiles(error code = ErrorCode#FileLoadError)
        /// or profile's format is wrong (error code = ErrorCode#FormatError)
        /// </exception>
        public static void loadProfile(string profileDirectory)
        {
            string[] listFiles = System.IO.Directory.GetFiles(profileDirectory);
            if (listFiles == null)
            {
                throw new LangDetectException(ErrorCode.NeedLoadProfileError, "Not found profile: " + profileDirectory);
            }

            int langsize = listFiles.Length;
            int index = 0;
            foreach (string file in listFiles)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Name.StartsWith("."))
                {
                    continue;
                }

                using (StreamReader sr = new StreamReader(file))
                {
                    LangProfile profile = JsonConvert.DeserializeObject<LangProfile>(sr.ReadToEnd());

                    addProfile(profile, index, langsize);
                    ++index;
                }
            }
        }

        /// <summary>
        /// Load profiles from specified directory.
        /// This method must be called once before language detection.
        /// </summary>
        /// <param name="json_profiles">profile directory path</param>
        /// <exception cref="LangDetectException">
        /// Can't open profiles(error code = ErrorCode#FileLoadError)
        /// or profile's format is wrong (error code = ErrorCode#FormatError)
        /// </exception>
        public static void loadProfile(IList<string> json_profiles)
        {
            int index = 0;
            int langsize = json_profiles.Count;
            if (langsize < 2)
            {
                throw new LangDetectException(ErrorCode.NeedLoadProfileError, "Need more than 2 profiles");
            }

            foreach (string json in json_profiles)
            {
                LangProfile profile = JsonConvert.DeserializeObject<LangProfile>(json);
                addProfile(profile, index, langsize);
                ++index;
            }
        }

        public static void addProfile(LangProfile profile, int index, int langsize)
        {
            string lang = profile.name;
            if (instance_.langlist.Contains(lang))
            {
                throw new LangDetectException(ErrorCode.DuplicateLangError, "duplicate the same language profile");
            }

            instance_.langlist.Add(lang);
            foreach (string word in profile.freq.Keys)
            {
                if (!instance_.wordLangProbMap.ContainsKey(word))
                {
                    instance_.wordLangProbMap[word] = new double[langsize];
                }
                int length = word.Length;
                if (length >= 1 && length <= 3)
                {
                    double prob = (double)profile.freq[word] / (double)profile.n_words[length - 1];
                    instance_.wordLangProbMap[word][index] = prob;
                }
            }
        }

        /// <summary>
        /// Clear loaded language profiles (reinitialization to be available)
        /// </summary>
        static public void clear()
        {
            instance_.langlist.Clear();
            instance_.wordLangProbMap.Clear();
        }

        /// <summary>
        /// Construct Detector instance
        /// </summary>
        /// <returns>Detector instance</returns>
        /// <exception cref="LangDetectException" />
        public static Detector create()
        {
            return createDetector();
        }

        /// <summary>
        /// Construct Detector instance with smoothing parameter 
        /// </summary>
        /// <param name="alpha">smoothing parameter (default value = 0.5)</param>
        /// <returns>Detector instance</returns>
        /// <exception cref="LangDetectException" />
        public static Detector create(double alpha)
        {
            Detector detector = createDetector();
            detector.setAlpha(alpha);
            return detector;
        }

        private static Detector createDetector()
        {
            if (instance_.langlist.Count == 0)
            {
                throw new LangDetectException(ErrorCode.NeedLoadProfileError, "need to load profiles");
            }

            Detector detector = new Detector(instance_);
            return detector;
        }

        public static void setSeed(long seed)
        {
            instance_.seed = seed;
        }

        public static IList<string> getLangList()
        {
            return new ReadOnlyCollection<string>(instance_.langlist);
        }
    } 
}
