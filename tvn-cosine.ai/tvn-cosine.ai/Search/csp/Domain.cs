using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tvn.cosine.ai.search.csp
{
    /**
     * A domain Di consists of a set of allowable values {v1, ... , vk} for the
     * corresponding variable Xi and defines a default order on those values. This
     * implementation guarantees, that domains are never changed after they have
     * been created. Domain reduction is implemented by replacement instead of
     * modification. So previous states can easily and safely be restored.
     * 
     * @author Ruediger Lunde
     */
    public class Domain<VAL> : IEnumerable<VAL>
    {
        private readonly VAL[] values;

        public Domain(List<VAL> values)
        {
            this.values = values.ToArray();
        }

        public Domain(IEnumerable<VAL> values)
        {
            this.values = values.ToArray();
        }

        public int size()
        {
            return values.Length;
        }



        public VAL get(int index)
        {
            return values[index];
        }

        public bool isEmpty()
        {
            return values.Length == 0;
        }

        public bool contains(VAL value)
        {
            foreach (var v in values)
                if (value.Equals(v))
                    return true;
            return false;
        }

        public List<VAL> asList()
        {
            return values.ToList();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Domain<VAL>)
            {
                Domain<VAL> d = (Domain<VAL>)obj;
                if (d.values.Length != values.Length)
                    return false;
                for (int i = 0; i < values.Length; i++)
                    if (!values[i].Equals(d.values[i]))
                        return false;
                return true;
            }
            return false;
        }


        public override int GetHashCode()
        {
            int hash = 9; // arbitrary seed value
            int multiplier = 13; // arbitrary multiplier value
            foreach (var value in values)
                hash = hash * multiplier + value.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("{");
            bool comma = false;
            foreach (var value in values)
            {
                if (comma)
                    result.Append(", ");
                result.Append(value.ToString());
                comma = true;
            }
            result.Append("}");
            return result.ToString();
        }

        public IEnumerator<VAL> GetEnumerator()
        {
            return values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
