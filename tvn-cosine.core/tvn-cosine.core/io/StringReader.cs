using tvn.cosine.io.api;

namespace tvn.cosine.io
{
    public class StringReader : TextReader, IStringReader
    {
        public StringReader(string input)
            : base(new System.IO.StringReader(input))
        {

        }
    }
}
