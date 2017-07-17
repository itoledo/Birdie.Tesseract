namespace tvn.cosine.ai.common.datastructures
{
    public struct KeyValuePair<KEY, VALUE>
    {
        private readonly KEY key;
        private readonly VALUE value;

        public KeyValuePair(KEY key, VALUE value)
        {
            this.key = key;
            this.value = value;
        }

        public KEY GetKey()
        {
            return this.key;
        }

        public VALUE GetValue()
        {
            return this.value;
        }
    }
}
