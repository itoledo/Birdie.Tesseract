using System;

namespace tvn.cosine.ai.util.datastructure 
{
    public class Triplet<X, Y, Z> : IEquatable<Triplet<X, Y, Z>>
        where X : IEquatable<X>
        where Y : IEquatable<Y>
        where Z : IEquatable<Z>
    {
        public X First { get; }
        public Y Second { get; }
        public Z Third { get; }

        /// <summary>
        /// Constructs a triplet with three specified elements.
        /// </summary>
        /// <param name="first">the first element of the triplet.</param>
        /// <param name="second">the second element of the triplet.</param>
        /// <param name="third">the third element of the triplet.</param>
        public Triplet(X first, Y second, Z third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }

        public bool Equals(Triplet<X, Y, Z> other)
        {
            return null != other
                && First.Equals(other.First)
                && Second.Equals(other.Second)
                && Third.Equals(other.Third);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Triplet<X, Y, Z>);
        }

        public override int GetHashCode()
        {
            return First.GetHashCode()
                 + 31
                 * Second.GetHashCode() 
                 + 31
                 * Third.GetHashCode();
        } 
    }

}
