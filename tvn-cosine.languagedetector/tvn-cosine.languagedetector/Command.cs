using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using tvn_cosine.languagedetector.util;

namespace tvn_cosine.languagedetector
{
    /// <summary> 
    /// LangDetect Command Line Interface
    /// <para />
    /// This is a command line interface of Language Detection Library "LandDetect".
    /// </summary>
    public class Command
    {
        /// <summary>
        /// smoothing default parameter (ELE)
        /// </summary>
        private const double DEFAULT_ALPHA = 0.5D;

        /** for Command line easy parser */
        private IDictionary<string, string> opt_with_value = new Dictionary<string, string>();
        private IDictionary<string, string> values = new Dictionary<string, string>();
        private ISet<string> opt_without_value = new HashSet<string>();
        private IList<string> arglist = new List<string>();

        /// <summary>
        /// Command line easy parser
        /// </summary>
        /// <param name="args">command line arguments</param>
        private void parse(params string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                if (opt_with_value.ContainsKey(args[i]))
                {
                    string key = opt_with_value[args[i]];
                    values[key] = args[i + 1];
                    ++i;
                }
                else if (args[i].StartsWith("-"))
                {
                    opt_without_value.Add(args[i]);
                }
                else
                {
                    arglist.Add(args[i]);
                }
            }
        }

        private void addOpt(string opt, string key, string value)
        {
            opt_with_value[opt] = key;
            values[key] = value;
        }

        private string get(string key)
        {
            return values[key];
        }

        private long? getLong(string key)
        {
            string value = values[key];
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return long.Parse(value,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture);

        }

        private double getDouble(string key, double defaultValue)
        {

            string value = values[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return double.Parse(value,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture);
        }

        private bool hasOpt(string opt)
        {
            return opt_without_value.Contains(opt);
        }

        /// <summary>
        /// File search (easy glob)
        /// </summary>
        /// <param name="directory">directory path</param>
        /// <param name="pattern">searching file pattern with regular representation</param>
        /// <returns>matched file</returns>
        private string searchFile(string directory, string pattern)
        {
            foreach (string file in System.IO.Directory.GetFiles(directory))
            {
                if (Regex.IsMatch(file, pattern))
                {
                    return file;
                }
            }
            return null;
        }

        /// <summary>
        /// load profiles
        /// </summary>
        /// <returns>false if load success</returns>
        private bool loadProfile()
        {
            string profileDirectory = get("directory") + "/";
            try
            {
                DetectorFactory.loadProfile(profileDirectory);
                long? seed = getLong("seed");
                if (seed != null)
                {
                    DetectorFactory.setSeed(seed.Value);
                }
                return false;
            }
            catch (LangDetectException e)
            {
                System.Console.WriteLine("ERROR: " + e.StackTrace);
                return true;
            }
        }

        /// <summary>
        /// Generate Language Profile from Wikipedia Abstract Database File
        /// <para /> 
        /// usage: --genprofile -d [abstracts directory] [language names] 
        /// </summary>
        public void generateProfile()
        {
            string directory = get("directory");
            foreach (string lang in arglist)
            {
                string file = searchFile(directory, lang + "wiki-.*-abstract\\.xml.*");
                if (file == null)
                {
                    System.Console.WriteLine("Not Found abstract xml : lang = " + lang);
                    continue;
                }

                LangProfile profile = GenProfile.loadFromWikipediaAbstract(lang, file);
                profile.omitLessFreq();

                string profile_path = get("directory") + "/profiles/" + lang;
                using (var os = System.IO.File.CreateText(profile_path))
                {
                    os.Write(JsonConvert.SerializeObject(profile));
                }
            }
        }

        /// <summary>
        /// Generate Language Profile from Text File
        /// <para />
        /// usage: --genprofile-text -l [language code] [text file path]
        /// </summary>
        private void generateProfileFromText()
        {
            if (arglist.Count != 1)
            {
                System.Console.WriteLine("Need to specify text file path");
                return;
            }
            string file = arglist[0];
            if (!System.IO.File.Exists(file))
            {
                System.Console.WriteLine("Need to specify existing text file path");
                return;
            }

            string lang = get("lang");
            if (lang == null)
            {
                System.Console.WriteLine("Need to specify langage code(-l)");
                return;
            }

            LangProfile profile = GenProfile.loadFromText(lang, file);
            profile.omitLessFreq();

            using (var os = System.IO.File.CreateText(lang))
            {
                os.Write(JsonConvert.SerializeObject(profile));
            }
        }

        /// <summary>
        /// Language detection test for each file (--detectlang option) 
        /// <para />
        /// usage: --detectlang -d [profile directory] -a [alpha] -s [seed] [test file(s)] 
        /// </summary>
        public void detectLang()
        {
            if (loadProfile()) return;
            foreach (string filename in arglist)
            {
                using (StreamReader _is = new StreamReader(filename, System.Text.Encoding.UTF8))
                {
                    Detector detector = DetectorFactory.create(getDouble("alpha", DEFAULT_ALPHA));
                    if (hasOpt("--debug"))
                    {
                        detector.setVerbose();
                    }

                    detector.append(_is);
                    System.Console.WriteLine(filename + ":" + detector.getProbabilities());
                }
            }
        }

        /// <summary>
        /// Batch Test of Language Detection (--batchtest option)
        /// <para />
        /// usage: --batchtest -d [profile directory] -a [alpha] -s [seed] [test data(s)]
        /// <para />
        /// The format of test data(s):
        /// <para />
        ///   [correct language name]\t[text body for test]\n 
        /// </summary>
        public void batchTest()
        {
            if (loadProfile()) return;
            IDictionary<string, IList<string>> result = new Dictionary<string, IList<string>>();
            foreach (string filename in arglist)
            {

                using (StreamReader _is = new StreamReader(filename, System.Text.Encoding.UTF8))
                {
                    while (!_is.EndOfStream)
                    {
                        string line = _is.ReadLine();
                        int idx = line.IndexOf('\t');
                        if (idx <= 0) continue;
                        string correctLang = line.Substring(0, idx);
                        string text = line.Substring(idx + 1);

                        Detector detector = DetectorFactory.create(getDouble("alpha", DEFAULT_ALPHA));
                        detector.append(text);
                        string lang = "";

                        lang = detector.detect();

                        if (!result.ContainsKey(correctLang))
                        {
                            result[correctLang] = new List<string>();
                        }
                        result[correctLang].Add(lang);
                        if (hasOpt("--debug"))
                        {
                            System.Console.WriteLine(correctLang + "," + lang + "," + (text.Length > 100 ? text.Substring(0, 100) : text));
                        }
                    }

                    List<string> langlist = new List<string>(result.Keys);
                    langlist.Sort();

                    int totalCount = 0, totalCorrect = 0;
                    foreach (string lang in langlist)
                    {
                        IDictionary<string, int> resultCount = new Dictionary<string, int>();
                        int count = 0;
                        IList<string> list = result[lang];
                        foreach (string detectedLang in list)
                        {
                            ++count;
                            if (resultCount.ContainsKey(detectedLang))
                            {
                                ++resultCount[detectedLang];
                            }
                            else
                            {
                                resultCount[detectedLang] = 1;
                            }
                        }
                        int correct = resultCount.ContainsKey(lang) ? resultCount[lang] : 0;
                        double rate = correct / (double)count;
                        System.Console.WriteLine(string.Format("{0} ({1}/{2}={3:##}): {4}", lang, correct, count, rate, resultCount));
                        totalCorrect += correct;
                        totalCount += count;
                    }
                    System.Console.WriteLine(string.Format("total: %d/%d = %.3f", totalCorrect, totalCount, totalCorrect / (double)totalCount));

                } 
            }
        }

        /// <summary>
        /// Command Line Interface
        /// </summary>
        /// <param name="args">command line arguments</param>
        public static void Main(params string[] args)
        {
            Command command = new Command();
            command.addOpt("-d", "directory", "./");
            command.addOpt("-a", "alpha", "" + DEFAULT_ALPHA);
            command.addOpt("-s", "seed", null);
            command.addOpt("-l", "lang", null);
            command.parse(args);

            if (command.hasOpt("--genprofile"))
            {
                command.generateProfile();
            }
            else if (command.hasOpt("--genprofile-text"))
            {
                command.generateProfileFromText();
            }
            else if (command.hasOpt("--detectlang"))
            {
                command.detectLang();
            }
            else if (command.hasOpt("--batchtest"))
            {
                command.batchTest();
            }
        } 
    } 
}
