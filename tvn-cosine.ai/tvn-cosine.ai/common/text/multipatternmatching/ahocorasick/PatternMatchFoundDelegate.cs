using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.common.text.multipatternmatching.ahocorasick
{
    public delegate void PatternMatchFoundDelegate<PatternType, InputType>(ICollection<PatternType> patternsFound, int position, InputType input);
}
