using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn_cosine.languagedetector.util
{
    /**
     * {@link TagExtractor} is a class which extracts inner texts of specified tag.
     * Users don't use this class directly.
     * @author Nakatani Shuyo
     */
    public class TagExtractor
    {
        /* package scope */
        string target_;
        /* package scope */
        int threshold_;
        /* package scope */
        StringBuilder buf_;
        /* package scope */
        string tag_;
        private int count_;

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
                buf_.append(line);
            }
        }
        public string closeTag()
        {
            string st = null;
            if (tag_ == target_ && buf_.length() > threshold_)
            {
                st = buf_.toString();
                ++count_;
            }
            clear();
            return st;
        }

    }

}
