using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace tvn_cosine.languagedetector.util
{
    /// <summary>
    /// Cut out N-gram from text. 
    /// Users don't use this class directly.
    /// </summary>
    public class NGram
    {
        private static readonly string LATIN1_EXCLUDED = Messages.getString("NGram.LATIN1_EXCLUDE");
        public const int N_GRAM = 3;
        public static IDictionary<char, char> cjk_map;

        private StringBuilder grams_;
        private bool capitalword_;

        public NGram()
        {
            grams_ = new StringBuilder(" ");
            capitalword_ = false;
        }

        /// <summary>
        /// Append a character into ngram buffer.
        /// </summary>
        /// <param name="ch"></param>
        public void addChar(char ch)
        {
            ch = normalize(ch);
            char lastchar = grams_[grams_.Length - 1];
            if (lastchar == ' ')
            {
                grams_ = new StringBuilder(" ");
                capitalword_ = false;
                if (ch == ' ') return;
            }
            else if (grams_.Length >= N_GRAM)
            {
                grams_.Remove(0, 1);
            }
            grams_.Append(ch);

            if (char.IsUpper(ch))
            {
                if (char.IsUpper(lastchar))
                {
                    capitalword_ = true;
                }
            }
            else
            {
                capitalword_ = false;
            }
        }

        /// <summary>
        /// Get n-Gram
        /// </summary>
        /// <param name="n">length of n-gram</param>
        /// <returns>string (null if it is invalid)</returns>
        public string get(int n)
        {
            if (capitalword_) return null;
            int len = grams_.Length;
            if (n < 1 || n > 3 || len < n) return null;
            if (n == 1)
            {
                char ch = grams_[len - 1];
                if (ch == ' ') return null;
                return ch.ToString();
            }
            else
            {
                return grams_.ToString(len - n, n);
            }
        }

        /// <summary>
        /// Character Normalization
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>Normalized character</returns>
        static public char normalize(char ch)
        {
            string charAsString = ch.ToString();
            if (Regex.IsMatch(charAsString, @"\p{IsBasicLatin}+"))
            {
                if (ch < 'A' || (ch < 'a' && ch > 'Z') || ch > 'z') ch = ' ';
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsLatin-1Supplement}+"))
            {
                if (LATIN1_EXCLUDED.IndexOf(ch) >= 0) ch = ' ';
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsLatinExtended-B}+"))
            {
                // normalization for Romanian
                if (ch == '\u0219') ch = '\u015f';  // Small S with comma below => with cedilla
                if (ch == '\u021b') ch = '\u0163';  // Small T with comma below => with cedilla
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsGeneralPunctuation}+"))
            {
                ch = ' ';
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsArabic}+"))
            {
                if (ch == '\u06cc') ch = '\u064a';  // Farsi yeh => Arabic yeh
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsLatinExtendedAdditional}+"))
            {
                if (ch >= '\u1ea0') ch = '\u1ec3';
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsHiragana}+"))
            {
                ch = '\u3042';
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsKatakana}+"))
            {
                ch = '\u30a2';
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsBopomofo}+")
                  || Regex.IsMatch(charAsString, @"\p{IsBopomofoExtended}+"))
            {
                ch = '\u3105';
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsCJKUnifiedIdeographs}+"))
            {
                if (cjk_map.ContainsKey(ch)) ch = cjk_map[ch];
            }
            else if (Regex.IsMatch(charAsString, @"\p{IsHangulSyllables}+"))
            {
                ch = '\uac00';
            }
            return ch;
        }

        /*  /// <summary>
          /// Normalizer for Vietnamese.
          /// Normalize Alphabet + Diacritical Mark(U+03xx) into U+1Exx .
          /// </summary>
          /// <param name="text"></param>
          /// <returns>normalized text</returns>
          public static string normalize_vi(string text)
          {
              Matcher m = ALPHABET_WITH_DMARK.matcher(text);
              StringBuffer buf = new StringBuffer();
              while (m.find()) {
                  int alphabet = TO_NORMALIZE_VI_CHARS.indexOf(m.group(1));
                  int dmark = DMARK_CLASS.indexOf(m.group(2)); // Diacritical Mark
                  m.appendReplacement(buf, NORMALIZED_VI_CHARS[dmark].substring(alphabet, alphabet + 1));
              }
              if (buf.length() == 0)
                  return text;
              m.appendTail(buf);
              return buf.toString();
          } */

        private static readonly string[]
            NORMALIZED_VI_CHARS = { Messages.getString("NORMALIZED_VI_CHARS_0300"),
                                    Messages.getString("NORMALIZED_VI_CHARS_0301"),
                                    Messages.getString("NORMALIZED_VI_CHARS_0303"),
                                    Messages.getString("NORMALIZED_VI_CHARS_0309"),
                                    Messages.getString("NORMALIZED_VI_CHARS_0323") };
        private static readonly string TO_NORMALIZE_VI_CHARS = Messages.getString("TO_NORMALIZE_VI_CHARS");
        private static readonly string DMARK_CLASS = Messages.getString("DMARK_CLASS");
        private static readonly Regex ALPHABET_WITH_DMARK = new Regex("([" + TO_NORMALIZE_VI_CHARS + "])([" + DMARK_CLASS + "])");

        /// <summary>
        /// CJK Kanji Normalization Mapping
        /// </summary>
        static readonly string[]
            CJK_CLASS = { Messages.getString("NGram.KANJI_1_0"),
                          Messages.getString("NGram.KANJI_1_2"),
                          Messages.getString("NGram.KANJI_1_4"),
                          Messages.getString("NGram.KANJI_1_8"),
                          Messages.getString("NGram.KANJI_1_11"),
                          Messages.getString("NGram.KANJI_1_12"),
                          Messages.getString("NGram.KANJI_1_13"),
                          Messages.getString("NGram.KANJI_1_14"),
                          Messages.getString("NGram.KANJI_1_16"),
                          Messages.getString("NGram.KANJI_1_18"),
                          Messages.getString("NGram.KANJI_1_22"),
                          Messages.getString("NGram.KANJI_1_27"),
                          Messages.getString("NGram.KANJI_1_29"),
                          Messages.getString("NGram.KANJI_1_31"),
                          Messages.getString("NGram.KANJI_1_35"),
                          Messages.getString("NGram.KANJI_2_0"),
                          Messages.getString("NGram.KANJI_2_1"),
                          Messages.getString("NGram.KANJI_2_4"),
                          Messages.getString("NGram.KANJI_2_9"),
                          Messages.getString("NGram.KANJI_2_10"),
                          Messages.getString("NGram.KANJI_2_11"),
                          Messages.getString("NGram.KANJI_2_12"),
                          Messages.getString("NGram.KANJI_2_13"),
                          Messages.getString("NGram.KANJI_2_15"),
                          Messages.getString("NGram.KANJI_2_16"),
                          Messages.getString("NGram.KANJI_2_18"),
                          Messages.getString("NGram.KANJI_2_21"),
                          Messages.getString("NGram.KANJI_2_22"),
                          Messages.getString("NGram.KANJI_2_23"),
                          Messages.getString("NGram.KANJI_2_28"),
                          Messages.getString("NGram.KANJI_2_29"),
                          Messages.getString("NGram.KANJI_2_30"),
                          Messages.getString("NGram.KANJI_2_31"),
                          Messages.getString("NGram.KANJI_2_32"),
                          Messages.getString("NGram.KANJI_2_35"),
                          Messages.getString("NGram.KANJI_2_36"),
                          Messages.getString("NGram.KANJI_2_37"),
                          Messages.getString("NGram.KANJI_2_38"),
                          Messages.getString("NGram.KANJI_3_1"),
                          Messages.getString("NGram.KANJI_3_2"),
                          Messages.getString("NGram.KANJI_3_3"),
                          Messages.getString("NGram.KANJI_3_4"),
                          Messages.getString("NGram.KANJI_3_5"),
                          Messages.getString("NGram.KANJI_3_8"),
                          Messages.getString("NGram.KANJI_3_9"),
                          Messages.getString("NGram.KANJI_3_11"),
                          Messages.getString("NGram.KANJI_3_12"),
                          Messages.getString("NGram.KANJI_3_13"),
                          Messages.getString("NGram.KANJI_3_15"),
                          Messages.getString("NGram.KANJI_3_16"),
                          Messages.getString("NGram.KANJI_3_18"),
                          Messages.getString("NGram.KANJI_3_19"),
                          Messages.getString("NGram.KANJI_3_22"),
                          Messages.getString("NGram.KANJI_3_23"),
                          Messages.getString("NGram.KANJI_3_27"),
                          Messages.getString("NGram.KANJI_3_29"),
                          Messages.getString("NGram.KANJI_3_30"),
                          Messages.getString("NGram.KANJI_3_31"),
                          Messages.getString("NGram.KANJI_3_32"),
                          Messages.getString("NGram.KANJI_3_35"),
                          Messages.getString("NGram.KANJI_3_36"),
                          Messages.getString("NGram.KANJI_3_37"),
                          Messages.getString("NGram.KANJI_3_38"),
                          Messages.getString("NGram.KANJI_4_0"),
                          Messages.getString("NGram.KANJI_4_9"),
                          Messages.getString("NGram.KANJI_4_10"),
                          Messages.getString("NGram.KANJI_4_16"),
                          Messages.getString("NGram.KANJI_4_17"),
                          Messages.getString("NGram.KANJI_4_18"),
                          Messages.getString("NGram.KANJI_4_22"),
                          Messages.getString("NGram.KANJI_4_24"),
                          Messages.getString("NGram.KANJI_4_28"),
                          Messages.getString("NGram.KANJI_4_34"),
                          Messages.getString("NGram.KANJI_4_39"),
                          Messages.getString("NGram.KANJI_5_10"),
                          Messages.getString("NGram.KANJI_5_11"),
                          Messages.getString("NGram.KANJI_5_12"),
                          Messages.getString("NGram.KANJI_5_13"),
                          Messages.getString("NGram.KANJI_5_14"),
                          Messages.getString("NGram.KANJI_5_18"),
                          Messages.getString("NGram.KANJI_5_26"),
                          Messages.getString("NGram.KANJI_5_29"),
                          Messages.getString("NGram.KANJI_5_34"),
                          Messages.getString("NGram.KANJI_5_39"),
                          Messages.getString("NGram.KANJI_6_0"),
                          Messages.getString("NGram.KANJI_6_3"),
                          Messages.getString("NGram.KANJI_6_9"),
                          Messages.getString("NGram.KANJI_6_10"),
                          Messages.getString("NGram.KANJI_6_11"),
                          Messages.getString("NGram.KANJI_6_12"),
                          Messages.getString("NGram.KANJI_6_16"),
                          Messages.getString("NGram.KANJI_6_18"),
                          Messages.getString("NGram.KANJI_6_20"),
                          Messages.getString("NGram.KANJI_6_21"),
                          Messages.getString("NGram.KANJI_6_22"),
                          Messages.getString("NGram.KANJI_6_23"),
                          Messages.getString("NGram.KANJI_6_25"),
                          Messages.getString("NGram.KANJI_6_28"),
                          Messages.getString("NGram.KANJI_6_29"),
                          Messages.getString("NGram.KANJI_6_30"),
                          Messages.getString("NGram.KANJI_6_32"),
                          Messages.getString("NGram.KANJI_6_34"),
                          Messages.getString("NGram.KANJI_6_35"),
                          Messages.getString("NGram.KANJI_6_37"),
                          Messages.getString("NGram.KANJI_6_39"),
                          Messages.getString("NGram.KANJI_7_0"),
                          Messages.getString("NGram.KANJI_7_3"),
                          Messages.getString("NGram.KANJI_7_6"),
                          Messages.getString("NGram.KANJI_7_7"),
                          Messages.getString("NGram.KANJI_7_9"),
                          Messages.getString("NGram.KANJI_7_11"),
                          Messages.getString("NGram.KANJI_7_12"),
                          Messages.getString("NGram.KANJI_7_13"),
                          Messages.getString("NGram.KANJI_7_16"),
                          Messages.getString("NGram.KANJI_7_18"),
                          Messages.getString("NGram.KANJI_7_19"),
                          Messages.getString("NGram.KANJI_7_20"),
                          Messages.getString("NGram.KANJI_7_21"),
                          Messages.getString("NGram.KANJI_7_23"),
                          Messages.getString("NGram.KANJI_7_25"),
                          Messages.getString("NGram.KANJI_7_28"),
                          Messages.getString("NGram.KANJI_7_29"),
                          Messages.getString("NGram.KANJI_7_32"),
                          Messages.getString("NGram.KANJI_7_33"),
                          Messages.getString("NGram.KANJI_7_35"),
                          Messages.getString("NGram.KANJI_7_37") };

         static NGram()
        {
            cjk_map = new Dictionary<char, char>();
            foreach (string cjk_list in CJK_CLASS)
            {
                char representative = cjk_list[0];
                for (int i = 0; i < cjk_list.Length; ++i)
                {
                    cjk_map[cjk_list[i]] = representative;
                }
            }
        }
    }
}
