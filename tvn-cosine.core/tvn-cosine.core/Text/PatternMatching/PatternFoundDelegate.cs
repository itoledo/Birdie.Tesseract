using System.Collections.Generic;
using tvn.cosine.text.patternmatching.api;

namespace tvn.cosine.text.patternmatching
{
    public delegate void PatternFoundDelegate(IPatternMatchingMachine sender, ISet<IPattern> patternsFound, uint position);
}
