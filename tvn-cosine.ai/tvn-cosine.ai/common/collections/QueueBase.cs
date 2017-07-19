﻿using System;
using System.Text;

namespace tvn.cosine.ai.common.collections
{
    public abstract class QueueBase<T> : IEnumerable<T>, IHashable, IToString, IEquatable
    {
        public abstract IEnumerator<T> GetEnumerator();

        public override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            bool first = true;
            foreach (var item in this)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(item.ToString());
            }
            sb.Append(']');
            return sb.ToString();
        }


        public class Comparer : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return System.Collections.Generic.Comparer<T>.Default.Compare(x, y);
            }
        }
         
        protected class ComparerAdaptor : System.Collections.Generic.Comparer<T>
        {
            private readonly IComparer<T> comparer;

            public ComparerAdaptor(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public override int Compare(T x, T y)
            {
                return this.comparer.Compare(x, y);
            }
        }
    }
}
