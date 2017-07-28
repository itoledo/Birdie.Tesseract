using System.IO;
using System.IO.Compression;
using System.Xml;
using tvn_cosine.languagedetector.util;

namespace tvn_cosine.languagedetector
{
    /// <summary> 
    /// Load Wikipedia's abstract XML as corpus and
    /// generate its language profile in JSON format.
    /// </summary>
    public class GenProfile
    {
        /// <summary>
        /// Load Wikipedia abstract database file and generate its language profile
        /// </summary>
        /// <param name="lang">target language name</param>
        /// <param name="file">target database file path</param>
        /// <returns>Language profile instance</returns>
        /// <exception cref="LangDetectException" />
        public static LangProfile loadFromWikipediaAbstract(string lang, string file)
        {
            LangProfile profile = new LangProfile(lang);
            FileInfo fi = new FileInfo(file);
            Stream _is = null;
            try
            {
                _is = fi.OpenRead();
                if (fi.Name.EndsWith(".gz"))
                {
                    _is = new GZipStream(_is, CompressionMode.Decompress);
                }

                using (StreamReader br = new StreamReader(_is, System.Text.Encoding.UTF8))
                {
                    TagExtractor tagextractor = new TagExtractor("abstract", 100);
                    using (XmlReader reader = XmlReader.Create(br))
                    {
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    tagextractor.setTag(reader.Name);
                                    break;
                                case XmlNodeType.Text:
                                    tagextractor.add(reader.Value);
                                    break;
                                case XmlNodeType.EndElement:
                                    string text = tagextractor.closeTag();
                                    if (text != null)
                                    {
                                        profile.update(text);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (null != _is)
                {
                    _is.Close();
                    _is.Dispose();
                }
            }
            return profile;
        }

        /// <summary>
        /// Load text file with UTF-8 and generate its language profile
        /// </summary>
        /// <param name="lang">target language name</param>
        /// <param name="file">target file path</param>
        /// <returns>Language profile instance</returns>
        /// <exception cref="LangDetectException" />
        public static LangProfile loadFromText(string lang, string file)
        {
            LangProfile profile = new LangProfile(lang);

            using (StreamReader _is = new StreamReader(file, System.Text.Encoding.UTF8))
            {
                int count = 0;
                while (!_is.EndOfStream)
                {
                    string line = _is.ReadLine();
                    profile.update(line);
                    ++count;
                }
                System.Console.WriteLine(lang + ":" + count);
            } 
            return profile;
        }
    } 
}
