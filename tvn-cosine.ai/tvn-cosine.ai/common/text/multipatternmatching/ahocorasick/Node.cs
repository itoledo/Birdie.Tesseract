using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.common.text.multipatternmatching.ahocorasick
{
    internal class Node<PatternType>
        where PatternType : IEnumerable<char>
    {
        private readonly char value;
        internal readonly Node<PatternType> fail = null;
        private readonly IMap<char, Node<PatternType>> gotoStateDictionary;
        private IMap<PatternType, byte> output;
        private Node<PatternType> failure;

        internal Node(char value)
        {
            this.value = value;
            gotoStateDictionary = CollectionFactory.CreateMap<char, Node<PatternType>>();
            output = CollectionFactory.CreateMap<PatternType, byte>();
        }

        internal char GetValue()
        {
            return value;
        }

        internal virtual Node<PatternType> GetFailure()
        {
            return failure;
        }

        internal virtual void SetFailure(Node<PatternType> failure)
        {
            this.failure = failure;
        }

        internal virtual Node<PatternType> g(char a)
        {
            if (!gotoStateDictionary.ContainsKey(a))
            {
                return fail;
            }
            else
            {
                return gotoStateDictionary.Get(a);
            }
        }

        internal void AddOutput(PatternType output)
        {
            this.output.Put(output, 1);
        }

        internal void AddOutput(ICollection<PatternType> output)
        {
            foreach (var o in output)
                AddOutput(o);
        }

        internal void Clear()
        {
            gotoStateDictionary.Clear();
        }

        internal void AddState(Node<PatternType> edge)
        {
            if (!gotoStateDictionary.ContainsKey(edge.GetValue()))
            {
                gotoStateDictionary.Put(edge.GetValue(), edge);
            }
        }

        internal ICollection<Node<PatternType>> GetEdges()
        {
            return gotoStateDictionary.GetValues();
        }

        internal ICollection<PatternType> Output
        {
            get
            {
                return output.GetKeys();
            }
        }
    }
}
