using System;
using System.Collections.Generic;
using System.Text;

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
        private static IDictionary<char, int> _assignedIndexicals = new Dictionary<char, int>();

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
                throw new ArgumentException("Preferred prefix :" + preferredPrefix + " must be a valid a lower case letter.");
            }

            StringBuilder sb = new StringBuilder();
            if (!_assignedIndexicals.ContainsKey(preferredPrefix))
            {
                _assignedIndexicals[preferredPrefix] = 0;
            }
            else
            {
                ++_assignedIndexicals[preferredPrefix];
            }
            sb.Append(_assignedIndexicals[preferredPrefix]);
            for (int i = 0; i < _assignedIndexicals[preferredPrefix]; ++i)
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

        public StandardizeApartIndexicalImpl(String prefix)
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
