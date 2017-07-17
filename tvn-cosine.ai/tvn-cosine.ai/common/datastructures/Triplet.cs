namespace tvn.cosine.ai.common.datastructures
{
    /**
     * @author Ravi Mohan
     * @author Mike Stampone
     * 
     */
    public class Triplet<X, Y, Z> : IEquatable, IHashable, IToString
    {
        private readonly X x;
        private readonly Y y;
        private readonly Z z;

        /**
         * Constructs a triplet with three specified elements.
         * 
         * @param x
         *            the first element of the triplet.
         * @param y
         *            the second element of the triplet.
         * @param z
         *            the third element of the triplet.
         */
        public Triplet(X x, Y y, Z z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /**
         * Returns the first element of the triplet.
         * 
         * @return the first element of the triplet.
         */
        public X getFirst()
        {
            return x;
        }

        /**
         * Returns the second element of the triplet.
         * 
         * @return the second element of the triplet.
         */
        public Y getSecond()
        {
            return y;
        }

        /**
         * Returns the third element of the triplet.
         * 
         * @return the third element of the triplet.
         */
        public Z getThird()
        {
            return z;
        }

        public override bool Equals(object o)
        {
            if (o is Triplet<X, Y, Z>)
            {
                Triplet<X, Y, Z> other = (Triplet<X, Y, Z>)o;
                return (x.Equals(other.x))
                    && (y.Equals(other.y))
                    && (z.Equals(other.z));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() + 31 * y.GetHashCode() + 31 * z.GetHashCode();
        }

        public override string ToString()
        {
            return "< " 
                 + x.ToString() 
                 + " , " 
                 + y.ToString() + " , "
                 + z.ToString() + " >";
        }
    } 
}
