using System;

namespace tvn.cosine.ai.util.datastructure 
{
    public class Pair<X, Y> : IEquatable<Pair<X, Y>>
        where X : IEquatable<X>
        where Y : IEquatable<Y>
    {
        public X First { get; }
        public Y Second { get; }

        /// <summary>
        /// Constructs a Pair from two given elements
        /// </summary>
        /// <param name="first">the first element</param>
        /// <param name="second">the second element</param>
        public Pair(X first, Y second)
        {
            this.First = first;
            this.Second = second;
        }

        public bool Equals(Pair<X, Y> other)
        { 
            return null != other
                && First.Equals(other.First)
                && Second.Equals(other.Second);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pair<X,Y>);
        }

        public override int GetHashCode()
        {
            return First.GetHashCode() 
                 + 31 
                 * Second.GetHashCode();
        } 
    } 
}
