using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using System.Text;

namespace tvn.cosine.ai.common.text.multipatternmatching.ahocorasick
{
    /// <summary>
    /// Implementation of the Aho corasick pattern matching machine.
    /// </summary>
    /// <typeparam name="PatternType">The Pattern type where the PatternType : IEnumerable of char</typeparam>
    public class PatternMatchingMachine<PatternType, InputType> : IDisposable
        where PatternType : IEnumerable<char>
        where InputType : IEnumerable<char>
    {
        private const char wildcard = '*';
        private const char whitespace = ' ';
        private const Node<PatternType> fail = null;
        private readonly Node<PatternType> root = new RootNode<PatternType>();

        public PatternMatchFoundDelegate<PatternType, InputType> PatternMatchesFound;

        /// <summary>
        /// Creates a new instance of the Aho-corasick pattern matcher.
        /// </summary>
        /// <param name="patternMatchFoundAction">An action delegate that receives a collection of matched patterns as well as the position where it matched.</param>
        /// <param name="patterns">The list of keywords to match. where the PatternType : IEnumerable of char</param>
        /// <exception cref="ArgumentNullException">Thrown when the patternMatchFoundAction or patterns is null.</exception>
        public PatternMatchingMachine(IEnumerable<PatternType> patterns)
        {
            if (patterns == null)
            {
                throw new ArgumentNullException("keywords cannot be null.");
            }

            constructGotoFunction(patterns);
            constructFailureFunction();
        }

        private Node<PatternType> g(Node<PatternType> state, char a)
        {
            return state.g(a);
        }

        private Node<PatternType> f(Node<PatternType> state)
        {
            return state.GetFailure();
        }

        private ICollection<PatternType> output(Node<PatternType> state)
        {
            return state.Output;
        }

        private void constructGotoFunction(IEnumerable<PatternType> k)
        {
            root.Clear();
            foreach (var y in k) enter(y);
        }

        private void constructFailureFunction()
        {
            ICollection<Node<PatternType>> queue = CollectionFactory.CreateFifoQueue<Node<PatternType>>();
            foreach (var a in root.GetEdges())
            {
                queue.Add(a);
                a.SetFailure(root);
            }

            while (queue.Size() > 0)
            {
                Node<PatternType> r = queue.Pop();
                foreach (var s in r.GetEdges())
                {
                    var a = s.GetValue();
                    queue.Add(s);
                    var state = r.GetFailure();
                    while (g(state, a) == fail) state = state.GetFailure();
                    s.SetFailure(g(state, a));
                    s.AddOutput(s.GetFailure().Output);
                }
            }
        }

        private void enter(PatternType pattern)
        {
            Node<PatternType> current = root;
            string tempPattern = preparePattern(pattern);
            foreach (char at in tempPattern)
            {
                char a = at; 

                Node<PatternType> newNode = g(current, a);
                if (newNode == fail ||
                    newNode == root)
                {
                    newNode = new Node<PatternType>(a);
                    current.AddState(newNode);
                }
                current = newNode;
            }
            current.AddOutput(pattern);
        }

        private string preparePattern(PatternType pattern)
        {
            StringBuilder StringBuilder = new StringBuilder();
            foreach (var c in pattern)
            {
                if (StringBuilder.Length == 0)
                {
                    if (c == wildcard)
                    {

                        continue;
                    }
                    else
                    {
                        StringBuilder.Append(whitespace);
                    }
                }

                StringBuilder.Append(c);
            }

            if (StringBuilder[StringBuilder.Length - 1] == wildcard)
            {
                --StringBuilder.Length;
            }
            else
            {
                StringBuilder.Append(whitespace);
            }

            if (string.IsNullOrWhiteSpace(StringBuilder.ToString()))
            {
                throw new ArgumentNullException("keyword is blank");
            }

            return StringBuilder.ToString();
        }

        public virtual void Match(InputType input)
        {
            int position = 0;
            var state = root;
            foreach (char at in input)
            {
                char a = at; 

                ++position;
                while (g(state, a) == fail) state = f(state);
                state = g(state, a);

                if (PatternMatchesFound != null && output(state).Size() > 0)
                    PatternMatchesFound(output(state), position, input);
            }
        }

        public void Dispose()
        {
            root.Clear();
        }
    }
}
