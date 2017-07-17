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
    }
}
