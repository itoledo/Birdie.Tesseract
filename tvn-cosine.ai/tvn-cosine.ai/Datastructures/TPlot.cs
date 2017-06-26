using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tvn.cosine.ai.Datastructures
{
    public class TPlot : IEnumerable<long[]>
    {
        private readonly object syncLock;
        private readonly int featureCount;
        private readonly IList<long[]> table;

        public TPlot(int featureCount)
        {
            table = new List<long[]>();
            syncLock = new object();
            this.featureCount = featureCount;
        }

        public void Add(long[] tuple)
        {
            if (tuple.Length != featureCount)
            {
                throw new ArgumentOutOfRangeException("tuple does not contain similar amount of features.");
            }

            lock (syncLock)
            {
                table.Add(tuple);
            }
        }

        public static double GetDotProduct(long[] item1, long[] item2)
        {
            if (item1.Length != item2.Length)
            {
                throw new ArgumentOutOfRangeException("tuple does not contain similar amount of features.");
            }
            double sum = 0;

            for (int i = 0; i < item1.Length; ++i)
            {
                sum += (item1[i] * item2[i]);
            }

            return sum;
        }

        public static double GetEuclideanDistance(long[] item1, long[] item2)
        {
            if (item1.Length != item2.Length)
            {
                throw new ArgumentOutOfRangeException("tuple does not contain similar amount of features.");
            }
            double sum = 0;

            for (int i = 0; i < item1.Length; ++i)
            {
                sum += (item1[i] - item2[i]) * (item1[i] - item2[i]);
            }

            return Math.Sqrt(sum);
        }

        public static double GetEuclideanDistance(long[] x)
        {
            double sum = 0;

            for (int i = 0; i < x.Length; ++i)
            {
                sum += x[i] * x[i];
            }

            return Math.Sqrt(sum);
        }

        public static double GetAngle(long[] x, long[] y)
        {
            if (x.Length != y.Length)
            {
                throw new ArgumentOutOfRangeException("value does not contain similar amount of features.");
            }

            var dotProduct = GetDotProduct(x, y);
            var xabs = GetEuclideanDistance(x);
            var yabs = GetEuclideanDistance(y);

            return Math.Acos((dotProduct / (xabs * yabs)));
        }
        
        public double GetEuclideanDistanceFromCentre(long[] x)
        {
            if (x.Length != featureCount)
            {
                throw new ArgumentOutOfRangeException("value does not contain similar amount of features.");
            }

            var centre = CalculateCentrePoint();
            return GetEuclideanDistance(x, centre);
        }

        public long[] CalculateCentrePoint()
        {
            long[] centre = new long[featureCount];

            for (int i = 0; i < featureCount; ++i)
            {
                foreach (var tuple in table)
                {
                    centre[i] += tuple[i];
                }
                centre[i] = centre[i] / table.Count;
            }
            return centre;
        } 

        public long[] this[int i]
        {
            get
            {
                return table[i];
            }
            set
            {
                if (value.Length != featureCount)
                {
                    throw new ArgumentOutOfRangeException("value does not contain similar amount of features.");
                }
                table[i] = value;
            }
        }

        public IEnumerator<long[]> GetEnumerator()
        {
            lock (syncLock)
            {
                return table.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (syncLock)
            {
                return table.GetEnumerator();
            }
        }
    }
}
