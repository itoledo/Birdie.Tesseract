using System.Collections.Generic;
using tvn.cosine.text.patternmatching;

namespace tvn.cosine.text.patternmatching.api
{
    public interface IPatternMatchingMachine
    {
        ICollection<IPattern> Match(IEnumerable<char> input); 
        void Match(IEnumerable<char> input, PatternFoundDelegate patternMatchFound);
    }
}
