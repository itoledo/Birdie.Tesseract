using tvn_cosine.ai.v2.common;
using aima.core.util.collect;
using System.Collections.Generic;
using System.Linq;

namespace aima.core.util
{ 
    /**
     * @author Ravi Mohan
     * @author Anurag Rai
     */
    public class Util
    {
        /**
         * Get the first element from a list.
         * 
         * @param l
         *            the list the first element is to be extracted from.
         * @return the first element of the passed in list.
         */
        public static T first<T>(IList<T> l)
        {
            return l[0];
        }

        /**
         * Get a sublist of all of the elements in the list except for first.
         * 
         * @param l
         *            the list the rest of the elements are to be extracted from.
         * @return a list of all of the elements in the passed in list except for
         *         the first element.
         */
        public static IList<T> rest<T>(IList<T> l)
        {
            return l.Skip(1).ToList();
        }

        /**
         * Create a set for the provided values.
         * 
         * @param values
         *            the sets initial values.
         * @return a Set of the provided values.
         */

        public static ISet<V> createSet<V>(params V[] values)
        {
            ISet<V> set = new HashSet<V>();

            foreach (V v in values)
            {
                set.Add(v);
            }

            return set;
        }

        public static double[] normalize(double[] probDist)
        {
            int len = probDist.Length;
            double total = 0.0;
            foreach (double d in probDist)
            {
                total = total + d;
            }

            double[] normalized = new double[len];
            if (total != 0)
            {
                for (int i = 0; i < len; i++)
                {
                    normalized[i] = probDist[i] / total;
                }
            }

            return normalized;
        }

        public static void permuteArguments<T>(T argumentType, IList<IList<T>> individualArgumentValues, Consumer<T[]> argConsumer)
        {
            foreach (T[] args in new CartesianProduct<T>(argumentType, individualArgumentValues))
            {
                argConsumer(args);
            }
        }
    }
}