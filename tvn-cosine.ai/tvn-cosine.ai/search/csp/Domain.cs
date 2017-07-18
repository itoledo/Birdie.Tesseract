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
    public class Domain<VAL> : Iterable<VAL> {


    private final object[]  values;

	public Domain(IQueue<VAL> values)
    {
        this.values = values.toArray();
    }

    @SafeVarargs
    public Domain(VAL...values)
    {
        this.values = values;
    }

    public int size()
    {
        return values.Length;
    }


    @SuppressWarnings("unchecked")

    public VAL get(int index)
    {
        return (VAL)values[index];
    }

    public bool isEmpty()
    {
        return values.Length == 0;
    }

    public bool contains(VAL value)
    {
        for (object v : values)
            if (value.Equals(v))
                return true;
        return false;
    }

     
    @SuppressWarnings("unchecked")

    public Iterator<VAL> iterator()
    {
        return new ArrayIterator<>((VAL[])values);
    }

    /** Not very efficient... */
    @SuppressWarnings("unchecked")

    public IQueue<VAL> asList()
    {
        return Arrays.asList((VAL[])values);
    }

     
    public bool equals(object obj)
    {
        if (obj != null && getClass() == obj.GetType())
        {
            Domain d = (Domain)obj;
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
        for (object value : values)
            hash = hash * multiplier + value.GetHashCode();
        return hash;
    }

     
    public override string ToString()
    {
        StringBuilder result = new StringBuilder("{");
        bool comma = false;
        for (object value : values)
        {
            if (comma)
                result.Append(", ");
            result.Append(value.ToString());
            comma = true;
        }
        result.Append("}");
        return result.ToString();
    }
}
}
