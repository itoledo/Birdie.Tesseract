using tvn.cosine.api;

namespace tvn.cosine.collections
{
    public class DefaultComparer<T> : IComparer<T>
    {
        private readonly System.Collections.Generic.IComparer<T> comparer = System.Collections.Generic.Comparer<T>.Default;

        public int Compare(T x, T y)
        {
            return comparer.Compare(x, y);
        }
    }
}
