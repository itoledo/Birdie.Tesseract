using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tvn.cosine.ai.util
{
    public class Util
    {
        public static bool ValuesEquals<T>(IList<T> list, IList<T> list2)
        {
            if (null == list && null == list2) return true;
            if (null == list || null == list2) return false;
            if (list.Count != list2.Count) return false;
            for (int i = 0; i < list.Count; ++i)
            {
                if (!list[i].Equals(list2[i])) return false;
            }
            return true;
        }

        public static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double ToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public const string NO = "No";
        public const string YES = "Yes";
        //
        private static Random random = new Random();

        private const double EPSILON = 0.000000000001;

        /**
	 * Get the first element from a list.
	 * 
	 * @param l
	 *            the list the first element is to be extracted from.
	 * @return the first element of the passed in list.
	 */
        public static T first<T>(List<T> l)
        {
            return l.First();
        }

        /**
         * Get a sublist of all of the elements in the list except for first.
         * 
         * @param l
         *            the list the rest of the elements are to be extracted from.
         * @return a list of all of the elements in the passed in list except for
         *         the first element.
         */
        public static List<T> rest<T>(List<T> l)
        {
            return l.Skip(1).ToList();
        }

        /**
         * Create a IDictionary<K, V> with the passed in keys having their values
         * initialized to the passed in value.
         * 
         * @param keys
         *            the keys for the newly constructed map.
         * @param value
         *            the value to be associated with each of the maps keys.
         * @return a map with the passed in keys initialized to value.
         */
        public static IDictionary<K, V> create<K, V>(ICollection<K> keys, V value)
        {
            IDictionary<K, V> map = new Dictionary<K, V>();

            foreach (K k in keys)
            {
                map.Add(k, value);
            }

            return map;
        }

        /**
         * Create a set for the provided values.
         * @param values
         *        the sets initial values.
         * @return a Set of the provided values.
         */
        public static ISet<V> createSet<V>(params V[] values)
        {
            return new HashSet<V>(values);
        }

        /**
         * Randomly select an element from a list.
         * 
         * @param <T>
         *            the type of element to be returned from the list l.
         * @param l
         *            a list of type T from which an element is to be selected
         *            randomly.
         * @return a randomly selected element from l.
         */
        public static T selectRandomlyFromList<T>(IList<T> l)
        {
            return l[random.Next(l.Count)];
        }

        public static T selectRandomlyFromSet<T>(ISet<T> set)
        {
            int i = 0;
            int max = random.Next(set.Count);
            foreach (var item in set)
            {
                if (i == max)
                    return item;
                ++i;
            }
            return default(T);
        }

        public static bool randombool()
        {
            int trueOrFalse = random.Next(2);
            return (!(trueOrFalse == 0));
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

        public static List<double> normalize(List<double> values)
        {
            double[] valuesAsArray = new double[values.Count];
            for (int i = 0; i < valuesAsArray.Length; i++)
                valuesAsArray[i] = values[i];
            double[] normalized = normalize(valuesAsArray);
            List<double> results = new List<double>();
            foreach (double aNormalized in normalized)
                results.Add(aNormalized);
            return results;
        }

        public static int min(int i, int j)
        {
            return (i > j ? j : i);
        }

        public static int max(int i, int j)
        {
            return (i < j ? j : i);
        }

        public static int max(int i, int j, int k)
        {
            return max(max(i, j), k);
        }

        public static int min(int i, int j, int k)
        {
            return min(min(i, j), k);
        }

        public static T mode<T>(IList<T> l)
        {
            IDictionary<T, int> hash = new Dictionary<T, int>();
            foreach (T obj in l)
            {
                if (hash.ContainsKey(obj))
                {
                    hash[obj] = hash[obj] + 1;
                }
                else
                {
                    hash.Add(obj, 1);
                }
            }

            T maxkey = hash.Keys.First();
            foreach (T key in hash.Keys)
            {
                if (hash[key] > hash[maxkey])
                {
                    maxkey = key;
                }
            }
            return maxkey;
        }

        public static string[] yesno()
        {
            return new string[] { YES, NO };
        }

        public static double log2(double d)
        {
            return Math.Log(d) / Math.Log(2);
        }

        public static double information(double[] probabilities)
        {
            double total = 0.0;
            foreach (double d in probabilities)
            {
                total += (-1.0 * log2(d) * d);
            }
            return total;
        }

        public static IList<T> removeFrom<T>(IList<T> list, T member)
        {
            IList<T> newList = new List<T>(list);
            newList.Remove(member);
            return newList;
        }

        public static double sumOfSquares(List<double> list)
        {
            double accum = 0;
            foreach (double item in list)
            {
                accum = accum + (item * item);
            }
            return accum;
        }

        public static string ntimes(string s, int n)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                builder.Append(s);
            }
            return builder.ToString();
        }

        public static void checkForNanOrInfinity(double d)
        {
            if (double.IsNaN(d))
            {
                throw new Exception("Not a Number");
            }
            if (double.IsInfinity(d))
            {
                throw new Exception("Infinite Number");
            }
        }

        public static int randomNumberBetween(int i, int j)
        {
            /* i,j bothinclusive */
            return random.Next(j - i + 1) + i;
        }

        public static double calculateMean(IList<double> lst)
        {
            double sum = 0.0;
            foreach (double d in lst)
            {
                sum = sum + d;
            }
            return sum / lst.Count;
        }

        public static double calculateStDev(IList<double> values, double mean)
        {

            int listSize = values.Count;

            double sumOfDiffSquared = 0.0;
            foreach (double value in values)
            {
                double diffFromMean = value - mean;
                sumOfDiffSquared += ((diffFromMean * diffFromMean) / (listSize - 1));
                // division moved here to avoid sum becoming too big if this
                // doesn't work use incremental formulation

            }
            double variance = sumOfDiffSquared;
            // (listSize - 1);
            // assumes at least 2 members in list.
            return Math.Sqrt(variance);
        }

        public static IList<double> normalizeFromMeanAndStdev(IList<double> values, double mean, double stdev)
        {
            return values.Select(d => (d - mean) / stdev).ToList();
        }

        /**
         * Generates a random double between two limits. Both limits are inclusive.
         * @param lowerLimit the lower limit.
         * @param upperLimit the upper limit.
         * @return a random double bigger or equals {@code lowerLimit} and smaller or equals {@code upperLimit}.
         */
        public static double generateRandomDoubleBetween(double lowerLimit, double upperLimit)
        {
            return lowerLimit + ((upperLimit - lowerLimit) * random.NextDouble());
        }

        /**
         * Generates a random float between two limits. Both limits are inclusive.
         * @param lowerLimit the lower limit.
         * @param upperLimit the upper limit.
         * @return a random float bigger or equals {@code lowerLimit} and smaller or equals {@code upperLimit}.
         */
        public static float generateRandomFloatBetween(float lowerLimit, float upperLimit)
        {
            return lowerLimit + ((upperLimit - lowerLimit) * (float)random.NextDouble());
        }

        /**
         * Compares two doubles for equality.
         * @param a the first double.
         * @param b the second double.
         * @return true if both doubles contain the same value or the absolute deviation between them is below {@code EPSILON}.
         */
        public static bool compareDoubles(double a, double b)
        {
            if (double.IsNaN(a) && double.IsNaN(b)) return true;
            if (!double.IsInfinity(a) && !double.IsInfinity(b)) return Math.Abs(a - b) <= EPSILON;
            return a == b;
        }

        /**
         * Compares two floats for equality.
         * @param a the first floats.
         * @param b the second floats.
         * @return true if both floats contain the same value or the absolute deviation between them is below {@code EPSILON}.
         */
        public static bool compareFloats(float a, float b)
        {
            if (float.IsNaN(a) && float.IsNaN(b)) return true;
            if (!float.IsInfinity(a) && !float.IsInfinity(b)) return Math.Abs(a - b) <= EPSILON;
            return a == b;
        }
    }
}
