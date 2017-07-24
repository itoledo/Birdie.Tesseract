using tvn.cosine.ai.common.api;

namespace tvn.cosine.ai.common.datastructures
{
    /**
     * @author Ravi Mohan
     * @author Mike Stampone
     * 
     */
    public class Pair<X, Y> : IEquatable, IHashable, IStringable
    {
        private readonly X a;
        private readonly Y b;

        /**
         * Constructs a Pair from two given elements
         * 
         * @param a
         *            the first element
         * @param b
         *            the second element
         */
        public Pair(X a, Y b)
        {
            this.a = a;
            this.b = b;
        }

        /**
         * Returns the first element of the pair
         * 
         * @return the first element of the pair
         */
        public X getFirst()
        {
            return a;
        }

        /**
         * Returns the second element of the pair
         * 
         * @return the second element of the pair
         */
        public Y getSecond()
        {
            return b;
        }

        public override bool Equals(object o)
        {
            if (o is Pair<X, Y>)
            {
                Pair<X, Y> p = (Pair<X, Y>)o;
                return a.Equals(p.a)
                    && b.Equals(p.b);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() + 31 * b.GetHashCode();
        }

        public override string ToString()
        {
            return "< " 
                  + getFirst().ToString() 
                  + " , " + getSecond().ToString()
                  + " > ";
        }
    } 
}
