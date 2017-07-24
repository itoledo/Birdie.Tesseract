using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.common.text.multipatternmatching.ahocorasick
{
    internal class RootNode<PatternType> : Node<PatternType>
        where PatternType : IEnumerable<char>
    {
        internal RootNode() : base('ϵ')
        {
            SetFailure(this); // a rootnode fails towards itself
        }

        internal override Node<PatternType> g(char a)
        {
            var state = base.g(a);
            if (state == fail)
                return this;
            else
                return state;
        }
    }
}
