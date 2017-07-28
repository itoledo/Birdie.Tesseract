using System.Text;

namespace tvn_cosine.languagedetector.util
{
    /// <summary>
    /// TagExtractor is a class which extracts inner texts of specified tag.
    /// Users don't use this class directly.
    /// </summary>
    public class TagExtractor
    {
        public StringBuilder buf_;
        public string target_;
        public int threshold_;
        public string tag_;
        public int count_;

        public TagExtractor(string tag, int threshold)
        {
            target_ = tag;
            threshold_ = threshold;
            count_ = 0;
            clear();
        }

        public int count()
        {
            return count_;
        }

        public void clear()
        {
            buf_ = new StringBuilder();
            tag_ = null;
        }

        public void setTag(string tag)
        {
            tag_ = tag;
        }

        public void add(string line)
        {
            if (tag_ == target_ && line != null)
            {
                buf_.Append(line);
            }
        }

        public string closeTag()
        {
            string st = null;
            if (tag_ == target_ && buf_.Length > threshold_)
            {
                st = buf_.ToString();
                ++count_;
            }
            clear();
            return st;
        }
    }
}
