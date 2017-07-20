using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.logic.fol
{
    /**
     * This class ensures unique standardize apart indexicals are created.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class StandardizeApartIndexicalFactory
    {
        private static IMap<char, int> _assignedIndexicals = Factory.CreateMap<char, int>();

        // For use in test cases, where predictable behavior is expected.
        public static void flush()
        {
            _assignedIndexicals.Clear();
        }

        public static StandardizeApartIndexical newStandardizeApartIndexical(char preferredPrefix)
        {
            char ch = preferredPrefix;
            if (!(char.IsLetter(ch) && char.IsLower(ch)))
            {
                throw new IllegalArgumentException("Preferred prefix :"
                        + preferredPrefix + " must be a valid a lower case letter.");
            }

            StringBuilder sb = new StringBuilder();
            int currentPrefixCnt = 0;
            if (!_assignedIndexicals.ContainsKey(preferredPrefix))
            {
                currentPrefixCnt = 0;
            }
            else
            {
                currentPrefixCnt = _assignedIndexicals.Get(preferredPrefix);
                currentPrefixCnt += 1;
            }
            _assignedIndexicals.Put(preferredPrefix, currentPrefixCnt);
            sb.Append(preferredPrefix);
            for (int i = 0; i < currentPrefixCnt;++i)
            {
                sb.Append(preferredPrefix);
            }

            return new StandardizeApartIndexicalImpl(sb.ToString());
        }
    }

    class StandardizeApartIndexicalImpl : StandardizeApartIndexical
    {

        private string prefix = null;
        private int index = 0;

        public StandardizeApartIndexicalImpl(string prefix)
        {
            this.prefix = prefix;
        }

        //
        // START-StandardizeApartIndexical
        public string getPrefix()
        {
            return prefix;
        }

        public int getNextIndex()
        {
            return index++;
        }
        // END-StandardizeApartIndexical
        //
    }
}
